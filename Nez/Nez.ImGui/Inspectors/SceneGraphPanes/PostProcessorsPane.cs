using ImGuiNET;

using Nez.ImGuiTools.ObjectInspectors;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Nez.ImGuiTools.SceneGraphPanes {
	/// <summary>
	/// manages displaying the current PostProcessors in the Scene and provides a means to add PostProcessors
	/// </summary>
	public class PostProcessorsPane {
		private List<PostProcessorInspector> _postProcessorInspectors = new List<PostProcessorInspector>();
		private bool _isPostProcessorListInitialized;

		private void UpdatePostProcessorInspectorList() {
			// first, we check our list of inspectors and sync it up with the current list of PostProcessors in the Scene.
			// we limit the check to once every 60 fames
			if (!_isPostProcessorListInitialized || Time.FrameCount % 60 == 0) {
				_isPostProcessorListInitialized = true;
				for (int i = 0; i < Core.Scene._postProcessors.Length; i++) {
					PostProcessor postProcessor = Core.Scene._postProcessors.Buffer[i];
					if (_postProcessorInspectors.Where(inspector => inspector.PostProcessor == postProcessor).Count() ==
						0) {
						_postProcessorInspectors.Add(new PostProcessorInspector(postProcessor));
					}
				}
			}
		}

		public void OnSceneChanged() {
			_postProcessorInspectors.Clear();
			_isPostProcessorListInitialized = false;
			UpdatePostProcessorInspectorList();
		}

		public void Draw() {
			UpdatePostProcessorInspectorList();

			ImGui.Indent();
			for (int i = 0; i < _postProcessorInspectors.Count; i++) {
				if (_postProcessorInspectors[i].PostProcessor._scene != null) {
					_postProcessorInspectors[i].Draw();
					NezImGui.SmallVerticalSpace();
				}
			}

			if (_postProcessorInspectors.Count == 0) {
				NezImGui.SmallVerticalSpace();
			}

			if (NezImGui.CenteredButton("Add PostProcessor", 0.6f)) {
				ImGui.OpenPopup("postprocessor-selector");
			}

			ImGui.Unindent();

			NezImGui.MediumVerticalSpace();
			DrawPostProcessorSelectorPopup();
		}

		private void DrawPostProcessorSelectorPopup() {
			if (ImGui.BeginPopup("postprocessor-selector")) {
				foreach (Type subclassType in InspectorCache.GetAllPostProcessorSubclassTypes()) {
					if (ImGui.Selectable(subclassType.Name)) {
						PostProcessor postprocessor = (PostProcessor)Activator.CreateInstance(subclassType,
							new object[] { _postProcessorInspectors.Count });
						Core.Scene.AddPostProcessor(postprocessor);
						_isPostProcessorListInitialized = false;
					}
				}

				ImGui.EndPopup();
			}
		}
	}
}