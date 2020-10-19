using ImGuiNET;

using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;


namespace Nez.ImGuiTools.TypeInspectors {
	public class MaterialInspector : AbstractTypeInspector {
		public bool AllowsMaterialRemoval = true;
		private List<AbstractTypeInspector> _inspectors = new List<AbstractTypeInspector>();

		public override void Initialize() {
			base.Initialize();

			_wantsIndentWhenDrawn = true;

			Material material = GetValue<Material>();
			if (material == null) {
				return;
			}

			// fetch our inspectors and let them know who their parent is
			_inspectors = TypeInspectorUtils.GetInspectableProperties(material);

			// if we are a Material<T>, we need to fix the duplicate Effect due to the "new T effect"
			if (ReflectionUtils.IsGenericTypeOrSubclassOfGenericType(material.GetType())) {
				bool didFindEffectInspector = false;
				for (int i = 0; i < _inspectors.Count; i++) {
					bool isEffectInspector = _inspectors[i] is EffectInspector;
					if (isEffectInspector) {
						if (didFindEffectInspector) {
							_inspectors.RemoveAt(i);
							break;
						}

						didFindEffectInspector = true;
					}
				}
			}
		}

		public override void DrawMutable() {
			bool isOpen = ImGui.CollapsingHeader($"{_name}", ImGuiTreeNodeFlags.FramePadding);

			if (GetValue() == null) {
				if (isOpen) {
					DrawNullMaterial();
				}

				return;
			}

			NezImGui.ShowContextMenuTooltip();

			if (ImGui.BeginPopupContextItem()) {
				if (AllowsMaterialRemoval && ImGui.Selectable("Remove Material")) {
					SetValue(null);
					_inspectors.Clear();
					ImGui.CloseCurrentPopup();
				}

				if (ImGui.Selectable("Set Effect", false, ImGuiSelectableFlags.DontClosePopups)) {
					ImGui.OpenPopup("effect-chooser");
				}

				ImGui.EndPopup();
			}

			if (isOpen) {
				ImGui.Indent();

				if (_inspectors.Count == 0) {
					if (NezImGui.CenteredButton("Set Effect", 0.6f)) {
						ImGui.OpenPopup("effect-chooser");
					}
				}

				for (int i = _inspectors.Count - 1; i >= 0; i--) {
					if (_inspectors[i].IsTargetDestroyed) {
						_inspectors.RemoveAt(i);
						continue;
					}

					_inspectors[i].Draw();
				}

				ImGui.Unindent();
			}

			if (DrawEffectChooserPopup()) {
				ImGui.CloseCurrentPopup();
			}
		}

		private void DrawNullMaterial() {
			if (NezImGui.CenteredButton("Create Material", 0.5f, ImGui.GetStyle().IndentSpacing * 0.5f)) {
				Material material = new Material();
				SetValue(material);
				_inspectors = TypeInspectorUtils.GetInspectableProperties(material);
			}
		}

		private bool DrawEffectChooserPopup() {
			bool createdEffect = false;
			if (ImGui.BeginPopup("effect-chooser")) {
				foreach (Type subclassType in InspectorCache.GetAllEffectSubclassTypes()) {
					if (ImGui.Selectable(subclassType.Name)) {
						// create the Effect, remove the existing EffectInspector and create a new one
						Effect effect = Activator.CreateInstance(subclassType) as Effect;
						Material material = GetValue<Material>();
						material.Effect = effect;

						for (int i = _inspectors.Count - 1; i >= 0; i--) {
							if (_inspectors[i].GetType() == typeof(EffectInspector)) {
								_inspectors.RemoveAt(i);
							}
						}

						EffectInspector inspector = new EffectInspector();
						inspector.SetTarget(material, ReflectionUtils.GetFieldInfo(material, "Effect"));
						inspector.Initialize();
						_inspectors.Add(inspector);

						createdEffect = true;
					}
				}

				ImGui.EndPopup();
			}

			return createdEffect;
		}
	}
}