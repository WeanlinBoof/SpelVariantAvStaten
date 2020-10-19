using ImGuiNET;

using Nez.ImGuiTools.TypeInspectors;

using System;
using System.Collections.Generic;


namespace Nez.ImGuiTools.ObjectInspectors {
	public class ObjectInspector : AbstractTypeInspector {
		private List<AbstractTypeInspector> _inspectors;

		public override void Initialize() {
			// we need something to inspect here so if we have a null object create a new one
			object obj = GetValue();
			if (obj == null && _valueType.GetConstructor(Type.EmptyTypes) != null) {
				obj = Activator.CreateInstance(_valueType);
			}

			if (obj != null) {
				_inspectors = TypeInspectorUtils.GetInspectableProperties(obj);
			}
			else {
				_inspectors = new List<AbstractTypeInspector>();
			}
		}

		public override void DrawMutable() {
			if (ImGui.CollapsingHeader(_name)) {
				foreach (AbstractTypeInspector inspector in _inspectors) {
					inspector.Draw();
				}
			}
		}
	}
}