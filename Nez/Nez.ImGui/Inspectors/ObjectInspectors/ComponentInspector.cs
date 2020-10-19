using ImGuiNET;

using Nez.ImGuiTools.TypeInspectors;

using System;
using System.Collections.Generic;


namespace Nez.ImGuiTools.ObjectInspectors {
	public class ComponentInspector : AbstractComponentInspector {
		public override Entity Entity => _component.Entity;
		public override Component Component => _component;

		private Component _component;
		private string _name;
		private List<Action> _componentDelegateMethods = new List<Action>();

		public ComponentInspector(Component component) {
			_component = component;
			_inspectors = TypeInspectorUtils.GetInspectableProperties(component);

			if (_component.GetType().IsGenericType) {
				string genericType = _component.GetType().GetGenericArguments()[0].Name;
				_name = $"{_component.GetType().BaseType.Name}<{genericType}>";
			}
			else {
				_name = _component.GetType().Name;
			}

			IEnumerable<System.Reflection.MethodInfo> methods = TypeInspectorUtils.GetAllMethodsWithAttribute<InspectorDelegateAttribute>(_component.GetType());
			foreach (System.Reflection.MethodInfo method in methods) {
				// only allow zero param methods
				if (method.GetParameters().Length == 0) {
					_componentDelegateMethods.Add((Action)Delegate.CreateDelegate(typeof(Action), _component, method));
				}
			}
		}

		public override void Draw() {
			ImGui.PushID(_scopeId);
			bool isHeaderOpen = ImGui.CollapsingHeader(_name);

			// context menu has to be outside the isHeaderOpen block so it works open or closed
			if (ImGui.BeginPopupContextItem()) {
				if (ImGui.Selectable("Remove Component")) {
					_component.RemoveComponent();
				}

				ImGui.EndPopup();
			}

			if (isHeaderOpen) {
				bool enabled = _component.Enabled;
				if (ImGui.Checkbox("Enabled", ref enabled)) {
					_component.SetEnabled(enabled);
				}

				for (int i = _inspectors.Count - 1; i >= 0; i--) {
					if (_inspectors[i].IsTargetDestroyed) {
						_inspectors.RemoveAt(i);
						continue;
					}

					_inspectors[i].Draw();
				}

				foreach (Action action in _componentDelegateMethods) {
					action();
				}
			}

			ImGui.PopID();
		}
	}
}