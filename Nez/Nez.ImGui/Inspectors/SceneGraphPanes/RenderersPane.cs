using ImGuiNET;

using Nez.ImGuiTools.ObjectInspectors;

using System.Collections.Generic;
using System.Linq;


namespace Nez.ImGuiTools.SceneGraphPanes {
	/// <summary>
	/// manages displaying the current Renderers in the Scene
	/// </summary>
	public class RenderersPane {
		private List<RendererInspector> _renderers = new List<RendererInspector>();
		private bool _isRendererListInitialized;

		private void UpdateRenderersPaneList() {
			// first, we check our list of inspectors and sync it up with the current list of PostProcessors in the Scene.
			// we limit the check to once every 60 fames
			if (!_isRendererListInitialized || Time.FrameCount % 60 == 0) {
				_isRendererListInitialized = true;
				for (int i = 0; i < Core.Scene._renderers.Length; i++) {
					Renderer renderer = Core.Scene._renderers.Buffer[i];
					if (_renderers.Where(inspector => inspector.Renderer == renderer).Count() == 0) {
						_renderers.Add(new RendererInspector(renderer));
					}
				}
			}
		}

		public void OnSceneChanged() {
			_renderers.Clear();
			_isRendererListInitialized = false;
			UpdateRenderersPaneList();
		}

		public void Draw() {
			UpdateRenderersPaneList();

			ImGui.Indent();
			for (int i = 0; i < _renderers.Count; i++) {
				_renderers[i].Draw();
				NezImGui.SmallVerticalSpace();
			}

			if (_renderers.Count == 0) {
				NezImGui.SmallVerticalSpace();
			}

			ImGui.Unindent();
		}
	}
}