using ImGuiNET;

using System;


namespace Nez.ImGuiTools.TypeInspectors {
	public class EnumInspector : AbstractTypeInspector {
		private string[] _enumNames;

		public override void Initialize() {
			base.Initialize();
			_enumNames = Enum.GetNames(_valueType);
		}

		public override void DrawMutable() {
			int index = Array.IndexOf(_enumNames, GetValue<object>().ToString());
			if (ImGui.Combo(_name, ref index, _enumNames, _enumNames.Length)) {
				SetValue(Enum.Parse(_valueType, _enumNames[index]));
			}

			HandleTooltip();
		}
	}
}