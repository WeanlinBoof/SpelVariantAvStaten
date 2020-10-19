using ImGuiNET;

using Microsoft.Xna.Framework;

using System;
using System.Collections;
using System.Collections.Generic;

using Num = System.Numerics;


namespace Nez.ImGuiTools.TypeInspectors {
	public class ListInspector : AbstractTypeInspector {
		public static Type[] KSupportedTypes =
			{typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(string), typeof(Vector2)};
		private IList _list;
		private Type _elementType;
		private bool _isArray;

		public override void Initialize() {
			base.Initialize();

			_list = (IList)_getter(_target);
			_isArray = _valueType.IsArray;
			_elementType = _valueType.IsArray ? _valueType.GetElementType() : _valueType.GetGenericArguments()[0];

			// null check. we just create an instance if null
			if (_list == null) {
				if (_isArray) {
					_list = Array.CreateInstance(_elementType, 1);
				}
				else {
					Type listType = typeof(List<>);
					Type constructedListType = listType.MakeGenericType(_elementType);
					_list = (IList)Activator.CreateInstance(constructedListType);
				}
			}
		}

		public override void DrawMutable() {
			ImGui.Indent();
			if (ImGui.CollapsingHeader($"{_name} [{_list.Count}]###{_name}", ImGuiTreeNodeFlags.FramePadding)) {
				ImGui.Indent();

				if (!_isArray) {
					if (ImGui.Button("Add Element")) {
						if (_elementType == typeof(string)) {
							_list.Add("");
						}
						else {
							_list.Add(Activator.CreateInstance(_elementType));
						}
					}

					ImGui.SameLine(ImGui.GetWindowWidth() - ImGui.GetItemRectSize().X -
								   ImGui.GetStyle().ItemInnerSpacing.X);

					// ImGui.SameLine( 0, ImGui.GetWindowWidth() * 0.65f - ImGui.GetItemRectSize().X + ImGui.GetStyle().ItemInnerSpacing.X - ImGui.GetStyle().IndentSpacing );
					if (ImGui.Button("Clear")) {
						ImGui.OpenPopup("Clear Data");
					}

					if (NezImGui.SimpleDialog("Clear Data", "Are you sure you want to clear the data?")) {
						_list.Clear();
						Debug.Log($"list count: {_list.Count}");
					}
				}

				ImGui.PushItemWidth(-ImGui.GetStyle().IndentSpacing);
				for (int i = 0; i < _list.Count; i++) {
					if (_elementType == typeof(int)) {
						DrawWidget((int)Convert.ChangeType(_list[i], _elementType), i);
					}
					else if (_elementType == typeof(float)) {
						DrawWidget((float)Convert.ChangeType(_list[i], _elementType), i);
					}
					else if (_elementType == typeof(string)) {
						DrawWidget((string)Convert.ChangeType(_list[i], _elementType), i);
					}
					else if (_elementType == typeof(Vector2)) {
						DrawWidget((Vector2)Convert.ChangeType(_list[i], _elementType), i);
					}
				}

				ImGui.PopItemWidth();
				ImGui.Unindent();
			}

			ImGui.Unindent();
		}

		private void DrawWidget(int value, int index) {
			if (ImGui.DragInt($"{index}", ref value)) {
				_list[index] = value;
			}
		}

		private void DrawWidget(float value, int index) {
			if (ImGui.DragFloat($"{index}", ref value)) {
				_list[index] = value;
			}
		}

		private void DrawWidget(string value, int index) {
			if (ImGui.InputText($"{index}", ref value, 200)) {
				_list[index] = value;
			}
		}

		private void DrawWidget(Vector2 value, int index) {
			Num.Vector2 vec = value.ToNumerics();
			if (ImGui.DragFloat2($"{index}", ref vec)) {
				_list[index] = vec.ToXNA();
			}
		}
	}
}