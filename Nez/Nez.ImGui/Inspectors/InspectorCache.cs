using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;


namespace Nez.ImGuiTools {
	public static class InspectorCache {
		private static Type[] _componentSubclasses;
		private static Type[] _effectSubclasses;
		private static Type[] _postProcessorSubclasses;

		/// <summary>
		/// gets all the Component subclasses that have a parameterless constructor
		/// </summary>
		/// <returns></returns>
		public static Type[] GetAllComponentSubclassTypes() {
			if (_componentSubclasses == null) {
				List<Type> subclasses = ReflectionUtils.GetAllSubclasses(typeof(Component), true);

				// filter out any Components that have generic parameters
				for (int i = subclasses.Count - 1; i >= 0; i--) {
					if (subclasses[i].ContainsGenericParameters) {
						subclasses.RemoveAt(i);
					}
				}

				// sort so the Colliders are on the bottom
				subclasses.Sort((t, u) => {
					bool tIsCollider = typeof(Collider).IsAssignableFrom(t);
					bool uIsCollider = typeof(Collider).IsAssignableFrom(u);

					if (tIsCollider && uIsCollider) {
						return t.Name.CompareTo(u.Name);
					}

					if (!tIsCollider && !uIsCollider) {
						bool tIsNez = t.Namespace.StartsWith("Nez");
						bool uIsNez = u.Namespace.StartsWith("Nez");

						if (tIsNez && uIsNez || !tIsNez && !uIsNez) {
							return t.Name.CompareTo(u.Name);
						}

						if (tIsNez && !uIsNez) {
							return 1;
						}

						return -1;
					}

					if (tIsCollider && !uIsCollider) {
						return 1;
					}

					return -1;
				});
				_componentSubclasses = subclasses.ToArray();
			}

			return _componentSubclasses;
		}

		/// <summary>
		/// gets all the Effect subclasses that have a parameterless constructor
		/// </summary>
		/// <returns></returns>
		public static Type[] GetAllEffectSubclassTypes() {
			if (_effectSubclasses == null) {
				List<Type> subclasses = ReflectionUtils.GetAllSubclasses(typeof(Effect), true);
				subclasses.Sort((t, u) => { return t.Name.CompareTo(u.Name); });
				_effectSubclasses = subclasses.ToArray();
			}

			return _effectSubclasses;
		}

		/// <summary>
		/// gets all the Effect subclasses that have a parameterless constructor
		/// </summary>
		/// <returns></returns>
		public static Type[] GetAllPostProcessorSubclassTypes() {
			if (_postProcessorSubclasses == null) {
				List<Type> subclasses = ReflectionUtils.GetAllSubclasses(typeof(PostProcessor));

				// filter out all except those with a single parameter constructor
				Type[] constructorParams = new Type[] { typeof(int) };
				for (int i = subclasses.Count - 1; i >= 0; i--) {
					if (subclasses[i].GetConstructor(constructorParams) == null) {
						subclasses.RemoveAt(i);
					}
				}

				subclasses.Sort((t, u) => { return t.Name.CompareTo(u.Name); });
				_postProcessorSubclasses = subclasses.ToArray();
			}

			return _postProcessorSubclasses;
		}
	}
}