using ImGuiNET;

using System.Collections.Generic;
using System.Reflection;


namespace Nez.ImGuiTools.TypeInspectors {
	public class StructInspector : AbstractTypeInspector {
		private List<AbstractTypeInspector> _inspectors = new List<AbstractTypeInspector>();
		private bool _isHeaderOpen;

		public override void Initialize() {
			base.Initialize();

			// figure out which fields and properties are useful to add to the inspector
			IEnumerable<FieldInfo> fields = ReflectionUtils.GetFields(_valueType);
			foreach (FieldInfo field in fields) {
				if (!field.IsPublic && !field.IsDefined(typeof(InspectableAttribute))) {
					continue;
				}

				AbstractTypeInspector inspector = TypeInspectorUtils.GetInspectorForType(field.FieldType, _target, field);
				if (inspector != null) {
					inspector.SetStructTarget(_target, this, field);
					inspector.Initialize();
					_inspectors.Add(inspector);
				}
			}

			IEnumerable<PropertyInfo> properties = ReflectionUtils.GetProperties(_valueType);
			foreach (PropertyInfo prop in properties) {
				if (!prop.CanRead || !prop.CanWrite) {
					continue;
				}

				bool isPropertyUndefinedOrPublic = !prop.CanWrite || (prop.CanWrite && prop.SetMethod.IsPublic);
				if ((!prop.GetMethod.IsPublic || !isPropertyUndefinedOrPublic) &&
					!prop.IsDefined(typeof(InspectableAttribute))) {
					continue;
				}

				AbstractTypeInspector inspector = TypeInspectorUtils.GetInspectorForType(prop.PropertyType, _target, prop);
				if (inspector != null) {
					inspector.SetStructTarget(_target, this, prop);
					inspector.Initialize();
					_inspectors.Add(inspector);
				}
			}
		}

		public override void DrawMutable() {
			ImGui.Indent();
			NezImGui.BeginBorderedGroup();

			_isHeaderOpen = ImGui.CollapsingHeader($"{_name}");
			if (_isHeaderOpen) {
				foreach (AbstractTypeInspector i in _inspectors) {
					i.Draw();
				}
			}

			NezImGui.EndBorderedGroup(new System.Numerics.Vector2(4, 1), new System.Numerics.Vector2(4, 2));
			ImGui.Unindent();
		}

		/// <summary>
		/// we need to override here so that we can keep the header enabled so that it can be opened
		/// </summary>
		public override void DrawReadOnly() {
			DrawMutable();
		}
	}
}