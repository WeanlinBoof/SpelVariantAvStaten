using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using TI = Nez.ImGuiTools.TypeInspectors;


namespace Nez.ImGuiTools.TypeInspectors {
	public static class TypeInspectorUtils {
		// Type cache seeing as how typeof isnt free and this will be hit a lot
		private static readonly Type notInspectableAttrType = typeof(NotInspectableAttribute);
		private static readonly Type inspectableAttrType = typeof(InspectableAttribute);
		private static readonly Type componentType = typeof(Component);
		private static readonly Type transformType = typeof(Transform);
		private static readonly Type materialType = typeof(Material);
		private static readonly Type effectType = typeof(Effect);
		private static readonly Type iListType = typeof(IList);
		private static readonly Type abstractTypeInspectorType = typeof(AbstractTypeInspector);
		private static readonly Type objectType = typeof(object);
		private static readonly Type serializationAttrType = typeof(SerializableAttribute);


		/// <summary>
		/// fetches all the relevant AbstractTypeInspectors for target including fields, properties and methods.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public static List<AbstractTypeInspector> GetInspectableProperties(object target) {
			List<AbstractTypeInspector> inspectors = new List<AbstractTypeInspector>();
			Type targetType = target.GetType();
			bool isComponentSubclass = target is Component;

			IEnumerable<FieldInfo> fields = ReflectionUtils.GetFields(targetType);
			foreach (FieldInfo field in fields) {
				if (field.IsStatic || field.IsDefined(notInspectableAttrType)) {
					continue;
				}

				bool hasInspectableAttribute = field.IsDefined(inspectableAttrType);

				// private fields must have the InspectableAttribute
				if (!field.IsPublic && !hasInspectableAttribute) {
					continue;
				}

				// similarly, readonly fields must have the InspectableAttribute
				if (field.IsInitOnly && !hasInspectableAttribute) {
					continue;
				}

				// skip enabled and entity which is handled elsewhere if this is a Component
				if (isComponentSubclass && (field.Name == "Enabled" || field.Name == "Entity")) {
					continue;
				}

				AbstractTypeInspector inspector = GetInspectorForType(field.FieldType, target, field);
				if (inspector != null) {
					inspector.SetTarget(target, field);
					inspector.Initialize();
					inspectors.Add(inspector);
				}
			}

			IEnumerable<PropertyInfo> properties = ReflectionUtils.GetProperties(targetType);
			foreach (PropertyInfo prop in properties) {
				if (prop.IsDefined(notInspectableAttrType)) {
					continue;
				}

				// Transforms and Component subclasses arent useful to inspect
				if (prop.PropertyType == transformType || prop.PropertyType.IsSubclassOf(componentType)) {
					continue;
				}

				if (!prop.CanRead || prop.GetGetMethod(true).IsStatic) {
					continue;
				}

				bool hasInspectableAttribute = prop.IsDefined(inspectableAttrType);

				// private props must have the InspectableAttribute
				if (!prop.GetMethod.IsPublic && !hasInspectableAttribute) {
					continue;
				}

				// similarly, readonly props must have the InspectableAttribute
				if (!prop.CanWrite && !hasInspectableAttribute) {
					continue;
				}

				// skip Component.enabled  and entity which is handled elsewhere
				if (isComponentSubclass && (prop.Name == "Enabled" || prop.Name == "Entity")) {
					continue;
				}

				AbstractTypeInspector inspector = GetInspectorForType(prop.PropertyType, target, prop);
				if (inspector != null) {
					inspector.SetTarget(target, prop);
					inspector.Initialize();
					inspectors.Add(inspector);
				}
			}

			IEnumerable<MethodInfo> methods = GetAllMethodsWithAttribute<InspectorCallableAttribute>(targetType);
			foreach (MethodInfo method in methods) {
				if (!MethodInspector.AreParametersValid(method.GetParameters())) {
					continue;
				}

				MethodInspector inspector = new MethodInspector();
				inspector.SetTarget(target, method);
				inspector.Initialize();
				inspectors.Add(inspector);
			}

			return inspectors;
		}

		public static IEnumerable<MethodInfo> GetAllMethodsWithAttribute<T>(Type type) where T : Attribute {
			IEnumerable<MethodInfo> methods = ReflectionUtils.GetMethods(type);
			foreach (MethodInfo method in methods) {
				T attr = method.GetCustomAttribute<T>();
				if (attr == null) {
					continue;
				}

				yield return method;
			}
		}

		/// <summary>
		/// gets an Inspector subclass that can handle valueType. If no default Inspector is available the memberInfo custom attributes
		/// will be checked for the CustomInspectorAttribute.
		/// </summary>
		/// <returns>The inspector for type.</returns>
		/// <param name="valueType">Value type.</param>
		/// <param name="memberInfo">Member info.</param>
		public static AbstractTypeInspector GetInspectorForType(Type valueType, object target, MemberInfo memberInfo) {
			// built-in types
			if (SimpleTypeInspector.KSupportedTypes.Contains(valueType)) {
				return new TI.SimpleTypeInspector();
			}

			if (target is Entity) {
				return new TI.EntityFieldInspector();
			}

			if (target is BlendState) {
				return new TI.BlendStateInspector();
			}

			if (valueType.GetTypeInfo().IsEnum) {
				return new TI.EnumInspector();
			}

			if (valueType.GetTypeInfo().IsValueType) {
				return new TI.StructInspector();
			}

			if (target is IList && ListInspector.KSupportedTypes.Contains(valueType.GetElementType())) {
				return new TI.ListInspector();
			}

			if (valueType.IsArray && valueType.GetArrayRank() == 1 &&
				ListInspector.KSupportedTypes.Contains(valueType.GetElementType())) {
				return new TI.ListInspector();
			}

			if (valueType.IsGenericType && iListType.IsAssignableFrom(valueType) &&
				valueType.GetInterface(nameof(IList)) != null &&
				ListInspector.KSupportedTypes.Contains(valueType.GetGenericArguments()[0])) {
				return new TI.ListInspector();
			}

			// check for custom inspectors before checking Nez types in case a subclass implemented one
			CustomInspectorAttribute customInspectorType = valueType.GetTypeInfo().GetCustomAttribute<CustomInspectorAttribute>();
			if (customInspectorType != null) {
				if (customInspectorType.InspectorType.GetTypeInfo().IsSubclassOf(abstractTypeInspectorType)) {
					return (AbstractTypeInspector)Activator.CreateInstance(customInspectorType.InspectorType);
				}

				Debug.Warn(
					$"found CustomInspector {customInspectorType.InspectorType} but it is not a subclass of AbstractTypeInspector");
			}

			// Nez types
			if (valueType == materialType || valueType.IsSubclassOf(materialType)) {
				return new MaterialInspector();
			}

			if (valueType == effectType || valueType.IsSubclassOf(effectType)) {
				return GetEffectInspector(target, memberInfo);
			}

			// last ditch effort. If the class is serializeable we use a generic ObjectInspector
			if (valueType != objectType && valueType.IsDefined(serializationAttrType)) {
				return new ObjectInspectors.ObjectInspector();
			}

			Debug.Info($"no inspector found for type {valueType} on object {target.GetType()}");

			return null;
		}

		/// <summary>
		/// null checks the Material and ony returns an Inspector if we have data since Material will almost always
		/// be null
		/// </summary>
		/// <returns>The material inspector.</returns>
		/// <param name="target">Target.</param>
		private static AbstractTypeInspector GetMaterialInspector(object target, MemberInfo memberInfo) {
			Material material = null;
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null) {
				material = fieldInfo.GetValue(target) as Material;
			}

			PropertyInfo propInfo = memberInfo as PropertyInfo;
			if (propInfo != null) {
				MethodInfo getter = ReflectionUtils.GetPropertyGetter(propInfo);
				material = getter.Invoke(target, new object[] { }) as Material;
			}

			return new MaterialInspector();
		}

		/// <summary>
		/// null checks the Effect and creates an Inspector only if it is not null
		/// </summary>
		/// <returns>The effect inspector.</returns>
		/// <param name="target">Target.</param>
		/// <param name="memberInfo">Member info.</param>
		private static AbstractTypeInspector GetEffectInspector(object target, MemberInfo memberInfo) {
			// we only want subclasses of Effect. Effect itself is not interesting so we have to fetch the data
			Effect effect = null;
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null) {
				effect = fieldInfo.GetValue(target) as Effect;
			}

			PropertyInfo propInfo = memberInfo as PropertyInfo;
			if (propInfo != null) {
				MethodInfo getter = ReflectionUtils.GetPropertyGetter(propInfo);
				effect = getter.Invoke(target, new object[] { }) as Effect;
			}

			if (effect != null && effect.GetType() != effectType) {
				return new EffectInspector();
			}

			return null;
		}
	}
}