using ImGuiNET;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;


namespace Nez.ImGuiTools.TypeInspectors {
	public class EffectInspector : AbstractTypeInspector {
		public bool AllowsEffectRemoval = true;
		private List<AbstractTypeInspector> _inspectors = new List<AbstractTypeInspector>();

		public override void Initialize() {
			base.Initialize();

			Effect effect = GetValue<Effect>();
			_name += $" ({effect.GetType().Name})";

			List<AbstractTypeInspector> inspectors = TypeInspectorUtils.GetInspectableProperties(effect);
			foreach (AbstractTypeInspector inspector in inspectors) {
				// we dont need the Name field. It serves no purpose.
				if (inspector.Name != "Name") {
					_inspectors.Add(inspector);
				}
			}
		}

		public override void DrawMutable() {
			bool isOpen = ImGui.CollapsingHeader($"{_name}", ImGuiTreeNodeFlags.FramePadding);

			if (AllowsEffectRemoval) {
				NezImGui.ShowContextMenuTooltip();
			}

			if (AllowsEffectRemoval && ImGui.BeginPopupContextItem()) {
				if (ImGui.Selectable("Remove Effect")) {
					SetValue(null);
					_isTargetDestroyed = true;
				}

				ImGui.EndPopup();
			}

			if (isOpen && !_isTargetDestroyed) {
				foreach (AbstractTypeInspector i in _inspectors) {
					i.Draw();
				}
			}
		}
	}
}