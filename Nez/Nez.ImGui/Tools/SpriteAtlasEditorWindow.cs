using ImGuiNET;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez.Sprites;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Num = System.Numerics;

namespace Nez.ImGuiTools {
	public class SpriteAtlasEditorWindow {
		private enum Origin {
			TopLeft,
			Top,
			TopRight,
			Left,
			Center,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
			Custom
		}

		private struct StartEndInt { public int Start, End; }

		/// <summary>
		/// default location of SpriteAtlasPacker.exe. If you use a non-standard Nez install location set this before using the atlas editor
		/// </summary>
		public static string PathToSpritePacker = "../../../Nez/Nez.SpriteAtlasPacker/PrebuiltExecutable/SpriteAtlasPacker.exe";

		/// <summary>
		/// default export path for atlases generated from a folder
		/// </summary>
		public static string AtlasExportFolder = "../../Content/Atlases";
		private string _sourceImageFile;
		private string _sourceAtlasFile;
		private Num.Vector2 _textureSize;
		private float _textureAspectRatio;
		private IntPtr _texturePtr;
		private bool _textureLoadedThisFrame;
		private float _imageZoom = 1;
		private Num.Vector2 _imagePosition;
		private SpriteAtlasData _spriteAtlasData = new SpriteAtlasData();
		private string[] _originEnumNames;
		private string _stringBuffer = "";
		private StartEndInt _startEndInt;
		private int _animationPreviewSize = 50;
		private bool _hasSlicedContent;
		private bool _atlasAllowsAnimationEditing;
		private bool _showSourceRectIndexes = true;
		private List<int> _nonEditableAnimations = new List<int>();

		public SpriteAtlasEditorWindow() {
			_originEnumNames = Enum.GetNames(typeof(Origin));

			PathToSpritePacker = Path.GetFullPath(PathToSpritePacker);
			if (!File.Exists(PathToSpritePacker)) {
				Debug.Warn("PathToSpritePacker doesnt exist! " + PathToSpritePacker);
			}

			// ensure out export folder exists
			AtlasExportFolder = Path.GetFullPath(AtlasExportFolder);
			Directory.CreateDirectory(AtlasExportFolder);
		}

		public bool Show() {
			ImGui.SetNextWindowPos(new Num.Vector2(0, 25), ImGuiCond.FirstUseEver);
			ImGui.SetNextWindowSize(new Num.Vector2(Screen.Width / 2, Screen.Height / 2), ImGuiCond.FirstUseEver);

			bool isOpen = true;
			if (ImGui.Begin("Sprite Atlas Editor", ref isOpen, ImGuiWindowFlags.MenuBar)) {
				DrawMenuBar();

				if (_hasSlicedContent) {
					ImGui.BeginChild("Slice Origins", new Num.Vector2(250, 0), true);
					DrawLeftPane();
					ImGui.EndChild();

					ImGui.SameLine();
				}

				float frame = ImGui.GetStyle().FramePadding.X + ImGui.GetStyle().FrameBorderSize;
				float rightPanePaddedSize = 300 + frame * 2;
				if (!_hasSlicedContent) {
					rightPanePaddedSize = 0;
				}

				ImGui.BeginChild("item view", new Num.Vector2(ImGui.GetContentRegionAvail().X - rightPanePaddedSize, 0), false, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
				if (_textureLoadedThisFrame) {
					FrameImage();
					_textureLoadedThisFrame = false;
				}
				DrawCenterPane();
				ImGui.EndChild();

				ImGui.SameLine();

				if (_hasSlicedContent) {
					ImGui.BeginChild("Right Pane", new Num.Vector2(300, 0), true);
					DrawRightPane();
					ImGui.EndChild();
				}

				ImGui.End();
			}

			return isOpen;
		}

		private void LoadTextureAndAtlasFiles() {
			if (_texturePtr != IntPtr.Zero) {
				Core.GetGlobalManager<ImGuiManager>().UnbindTexture(_texturePtr);
			}

			_spriteAtlasData.Clear();
			_nonEditableAnimations.Clear();
			_atlasAllowsAnimationEditing = true;
			_hasSlicedContent = false;

			Texture2D _atlasTexture = Texture2D.FromStream(Core.GraphicsDevice, File.OpenRead(_sourceImageFile));
			_textureSize = new Num.Vector2(_atlasTexture.Width, _atlasTexture.Height);
			_textureAspectRatio = _textureSize.X / _textureSize.Y;
			_texturePtr = Core.GetGlobalManager<ImGuiManager>().BindTexture(_atlasTexture);
			_textureLoadedThisFrame = true;

			if (File.Exists(_sourceAtlasFile)) {
				_hasSlicedContent = true;
				_spriteAtlasData = SpriteAtlasLoader.ParseSpriteAtlasData(_sourceAtlasFile, true);

				// ensure animations are contiguous, since that is all we support.
				// First check that all frames are in order in the animations
				for (int j = 0; j < _spriteAtlasData.AnimationFrames.Count; j++) {
					List<int> animation = _spriteAtlasData.AnimationFrames[j];
					for (int i = 0; i < animation.Count; i++) {
						if (i == 0) {
							continue;
						}

						if (animation[i] != animation[i - 1] + 1) {
							_atlasAllowsAnimationEditing = false;
							_nonEditableAnimations.Add(j);
						}
					}
				}

				// Next check that all frames are in order, ghetto style. We check that all the y-values of the rects
				// always increase or stay the same. Not perfect by any means, but we dont know if this is padded in some
				// odd way or contains sprites of odd sizes so this is a quick and dirty solution.
				int lastRectY = -1;
				for (int i = 0; i < _spriteAtlasData.SourceRects.Count; i++) {
					if (i == 0 || lastRectY <= _spriteAtlasData.SourceRects[i].Y) {
						lastRectY = _spriteAtlasData.SourceRects[i].Y;
						continue;
					}

					_atlasAllowsAnimationEditing = false;
					return;
				}
			}
		}

		private void GenerateSpriteAtlasFromFolder(string path) {
			// use the chosen folder name as the atlas name
			string name = Path.GetFileName(path);
			string args = $"{PathToSpritePacker} -image:{AtlasExportFolder}/{name}.png -map:{AtlasExportFolder}/{name}.atlas -fps:7 {path}";
			Process.Start(new ProcessStartInfo {
				FileName = "mono",
				Arguments = args,
				WindowStyle = ProcessWindowStyle.Hidden
			}).WaitForExit();

			_sourceImageFile = Path.Combine(AtlasExportFolder, name + ".png");
			_sourceAtlasFile = Path.Combine(AtlasExportFolder, name + ".atlas");

			LoadTextureAndAtlasFiles();
		}

		#region Drawing Methods

		private int _globalOriginEnumValue;

		private void DrawLeftPane() {
			Origin OriginIndex(Vector2 origin) {
				switch (origin) {
					case Vector2 o when o.X == 0 && o.Y == 0: // tl
						return Origin.TopLeft;
					case Vector2 o when o.X == 0.5f && o.Y == 0: // t
						return Origin.Top;
					case Vector2 o when o.X == 1 && o.Y == 0: // tr
						return Origin.TopRight;
					case Vector2 o when o.X == 0 && o.Y == 0.5f: // l
						return Origin.Left;
					case Vector2 o when o.X == 0.5f && o.Y == 0.5f: // center
						return Origin.Center;
					case Vector2 o when o.X == 1 && o.Y == 0.5f: // right
						return Origin.Right;
					case Vector2 o when o.X == 0 && o.Y == 1: // bl
						return Origin.BottomLeft;
					case Vector2 o when o.X == 0.5f && o.Y == 1: // b
						return Origin.Bottom;
					case Vector2 o when o.X == 1 && o.Y == 1: // br
						return Origin.BottomRight;
					default:
						return Origin.Custom;
				}
			}

			Vector2 OriginValue(Origin origin, Vector2 currentOrigin) {
				switch (origin) {
					case Origin.TopLeft: return new Vector2(0, 0);
					case Origin.Top: return new Vector2(0.5f, 0);
					case Origin.TopRight: return new Vector2(1, 0);
					case Origin.Left: return new Vector2(0, 0.5f);
					case Origin.Center: return new Vector2(0.5f, 0.5f);
					case Origin.Right: return new Vector2(1, 0.5f);
					case Origin.BottomLeft: return new Vector2(0, 1);
					case Origin.Bottom: return new Vector2(0.5f, 1);
					case Origin.BottomRight: return new Vector2(1, 1);
					default: return currentOrigin + new Vector2(0.01f, 0.01f);
				}
			}

			if (NezImGui.CenteredButton("Set All Origins", 0.75f)) {
				_globalOriginEnumValue = 7;
				ImGui.OpenPopup("set-all-origins");
			}

			NezImGui.MediumVerticalSpace();

			if (ImGui.BeginPopup("set-all-origins")) {
				ImGui.Combo("###global-origin", ref _globalOriginEnumValue, _originEnumNames, _originEnumNames.Length);
				if (ImGui.Button("Set All Origins")) {
					for (int i = 0; i < _spriteAtlasData.Origins.Count; i++) {
						_spriteAtlasData.Origins[i] = OriginValue((Origin)_globalOriginEnumValue, _spriteAtlasData.Origins[i]);
					}

					ImGui.CloseCurrentPopup();
				}
				ImGui.EndPopup();
			}

			for (int i = 0; i < _spriteAtlasData.Origins.Count; i++) {
				ImGui.PushID(i);
				string name = _spriteAtlasData.Names[i];
				if (ImGui.InputText("Name", ref name, 25)) {
					_spriteAtlasData.Names[i] = name;
				}

				Num.Vector2 origin = _spriteAtlasData.Origins[i].ToNumerics();
				if (ImGui.SliderFloat2("Origin", ref origin, 0f, 1f)) {
					_spriteAtlasData.Origins[i] = origin.ToXNA();
				}

				Origin originEnum = OriginIndex(_spriteAtlasData.Origins[i]);
				int originEnumValue = (int)originEnum;
				if (ImGui.Combo($"###enum_{i}", ref originEnumValue, _originEnumNames, _originEnumNames.Length)) {
					_spriteAtlasData.Origins[i] = OriginValue((Origin)originEnumValue, _spriteAtlasData.Origins[i]);
				}

				ImGui.Separator();
				ImGui.PopID();
			}
		}

		private void DrawCenterPane() {
			if (ImGui.IsWindowFocused() && ImGui.GetIO().MouseWheel != 0) {
				float minZoom = 0.2f;
				float maxZoom = 10f;

				Num.Vector2 oldSize = _imageZoom * _textureSize;
				float zoomSpeed = _imageZoom * 0.1f;
				_imageZoom += Math.Min(maxZoom - _imageZoom, ImGui.GetIO().MouseWheel * zoomSpeed);
				_imageZoom = Mathf.Clamp(_imageZoom, minZoom, maxZoom);

				// zoom in, move up/left, zoom out the opposite
				Num.Vector2 deltaSize = oldSize - (_imageZoom * _textureSize);
				_imagePosition += deltaSize * 0.5f;
			}

			ImGui.GetIO().ConfigWindowsResizeFromEdges = true;
			if (ImGui.IsWindowFocused() && ImGui.IsMouseDown(0) && ImGui.GetIO().KeyAlt) {
				_imagePosition += ImGui.GetMouseDragDelta(0);
				ImGui.ResetMouseDragDelta(0);
			}

			// clamp in such a way that we keep some part of the image visible
			Num.Vector2 min = -(_textureSize * _imageZoom) * 0.8f;
			Num.Vector2 max = ImGui.GetContentRegionAvail() * 0.8f;
			_imagePosition = Num.Vector2.Clamp(_imagePosition, min, max);
			ImGui.SetCursorPos(_imagePosition);

			Num.Vector2 cursorPosImageTopLeft = ImGui.GetCursorScreenPos();
			ImGui.Image(_texturePtr, _textureSize * _imageZoom);

			for (int i = 0; i < _spriteAtlasData.SourceRects.Count; i++) {
				DrawRect(cursorPosImageTopLeft, _spriteAtlasData.SourceRects[i], i);
				DrawOrigin(cursorPosImageTopLeft, _spriteAtlasData.Origins[i], _spriteAtlasData.SourceRects[i]);
			}

			// now, we draw over the image any controls
			if (_texturePtr != IntPtr.Zero) {
				DrawControlsOverImage();
			}
		}

		private void DrawMenuBar() {
			bool newAtlas = false;
			bool openFile = false;

			if (ImGui.BeginMenuBar()) {
				if (ImGui.BeginMenu("File")) {
					if (ImGui.MenuItem("New Atlas from Folder")) {
						newAtlas = true;
					}

					if (ImGui.MenuItem("Load Atlas or PNG")) {
						openFile = true;
					}

					if (ImGui.MenuItem("Save Atlas", _spriteAtlasData.SourceRects.Count > 0)) {
						_spriteAtlasData.SaveToFile(_sourceAtlasFile);
					}

					ImGui.EndMenu();
				}
				ImGui.EndMenuBar();
			}

			if (newAtlas) {
				ImGui.OpenPopup("new-atlas");
			}

			if (openFile) {
				ImGui.OpenPopup("open-file");
			}

			NewAtlasPopup();
			OpenFilePopup();
		}

		private void NewAtlasPopup() {
			bool isOpen = true;
			if (ImGui.BeginPopupModal("new-atlas", ref isOpen, ImGuiWindowFlags.NoTitleBar)) {
				FilePicker picker = FilePicker.GetFolderPicker(this, new DirectoryInfo(Environment.CurrentDirectory).Parent.FullName);
				picker.DontAllowTraverselBeyondRootFolder = false;
				if (picker.Draw()) {
					GenerateSpriteAtlasFromFolder(picker.SelectedFile);
					FilePicker.RemoveFilePicker(this);
				}
				ImGui.EndPopup();
			}
		}

		private void OpenFilePopup() {
			bool isOpen = true;
			if (ImGui.BeginPopupModal("open-file", ref isOpen, ImGuiWindowFlags.NoTitleBar)) {
				FilePicker picker = FilePicker.GetFilePicker(this, Path.Combine(Environment.CurrentDirectory, "Content"), ".png|.atlas");
				picker.DontAllowTraverselBeyondRootFolder = true;
				if (picker.Draw()) {
					string file = picker.SelectedFile;
					if (file.EndsWith(".png")) {
						_sourceImageFile = file;
						_sourceAtlasFile = file.Replace(".png", ".atlas");
					}
					else {
						_sourceImageFile = file.Replace(".atlas", ".png");
						_sourceAtlasFile = file;
					}
					LoadTextureAndAtlasFiles();
					FilePicker.RemoveFilePicker(this);
				}
				ImGui.EndPopup();
			}
		}

		private int _width = 16, _height = 16, _padding = 1, _frames = 10;

		private void DrawAtlasSlicerPopup() {
			bool isOpen = true;
			if (ImGui.BeginPopupModal("atlas-slicer", ref isOpen, ImGuiWindowFlags.AlwaysAutoResize)) {
				ImGui.InputInt("Width", ref _width);
				ImGui.InputInt("Height", ref _height);
				ImGui.InputInt("Max Frames", ref _frames);
				ImGui.InputInt("Padding", ref _padding);

				if (ImGui.Button("Slice")) {
					GenerateRects(_width, _height, _frames, _padding);
				}

				ImGui.SameLine();

				if (ImGui.Button("Done")) {
					ImGui.CloseCurrentPopup();
				}

				ImGui.EndPopup();
			}
		}

		private void DrawControlsOverImage() {
			ImGui.SetCursorPos(Num.Vector2.Zero);

			if (ImGui.Button("Center")) {
				CenterImage();
			}

			// ImGui.SameLine(ImGui.GetWindowWidth() - 70);
			ImGui.SameLine();

			if (ImGui.Button("Frame")) {
				FrameImage();
			}

			ImGui.SameLine();

			if (ImGui.Button("Slice")) {
				ImGui.OpenPopup("atlas-slicer");
			}

			ImGui.SameLine(ImGui.GetContentRegionAvail().X - 160);

			ImGui.Checkbox("Show Rect Indexes", ref _showSourceRectIndexes);

			DrawAtlasSlicerPopup();
		}

		private void DrawRightPane() {
			if (!_atlasAllowsAnimationEditing) {
				ImGui.PushStyleColor(ImGuiCol.Text, Color.Red.PackedValue);
				ImGui.TextWrapped("Edit/Add animations at your own risk! The loaded atlas either does not have contiguous frames or contains animations that are not contiguous.");
				ImGui.PopStyleColor();
				NezImGui.MediumVerticalSpace();
			}

			if (NezImGui.CenteredButton("Add Animation", 0.5f)) {
				ImGui.OpenPopup("add-animation");
			}

			NezImGui.MediumVerticalSpace();

			for (int i = 0; i < _spriteAtlasData.AnimationNames.Count; i++) {
				bool isEditable = !_nonEditableAnimations.Contains(i);
				ImGui.PushID(i);
				bool didNotDeleteAnimation = true;
				if (ImGui.CollapsingHeader(_spriteAtlasData.AnimationNames[i] + $"###anim{i}", ref didNotDeleteAnimation)) {
					string name = _spriteAtlasData.AnimationNames[i];
					if (ImGui.InputText("Name", ref name, 25)) {
						_spriteAtlasData.AnimationNames[i] = name;
					}

					int fps = _spriteAtlasData.AnimationFps[i];
					if (ImGui.SliderInt("Frame Rate", ref fps, 0, 24)) {
						_spriteAtlasData.AnimationFps[i] = fps;
					}

					List<int> frames = _spriteAtlasData.AnimationFrames[i];
					if (isEditable) {
						if (frames.Count == 0) {
							_startEndInt.Start = _startEndInt.End = 0;
						}
						else if (frames.Count == 1) {
							_startEndInt.Start = frames[0];
							_startEndInt.End = frames[0];
						}
						else {
							_startEndInt.Start = frames[0];
							_startEndInt.End = frames.LastItem();
						}

						bool framesChanged = ImGui.SliderInt("Start Frame", ref _startEndInt.Start, 0, _startEndInt.End);
						framesChanged |= ImGui.SliderInt("End Frame", ref _startEndInt.End, _startEndInt.Start, _spriteAtlasData.SourceRects.Count - 1);

						if (framesChanged) {
							frames.Clear();
							for (int j = _startEndInt.Start; j <= _startEndInt.End; j++) {
								frames.Add(j);
							}
						}
					}

					if (frames.Count > 0) {
						float secondsPerFrame = 1 / (float)fps;
						float iterationDuration = secondsPerFrame * (float)frames.Count;
						float currentElapsed = Time.TotalTime % iterationDuration;
						int desiredFrame = Mathf.FloorToInt(currentElapsed / secondsPerFrame);

						Rectangle rect = _spriteAtlasData.SourceRects[frames[desiredFrame]];
						Num.Vector2 uv0 = rect.Location.ToNumerics() / _textureSize;
						Num.Vector2 uv1 = rect.GetSize().ToNumerics() / _textureSize;

						Num.Vector2 size = CalcBestFitRegion(new Num.Vector2(_animationPreviewSize), rect.GetSize().ToNumerics());
						ImGui.SetCursorPosX((ImGui.GetWindowContentRegionWidth() - size.X) / 2f);
						ImGui.Image(_texturePtr, size, uv0, uv0 + uv1);
					}

					NezImGui.SmallVerticalSpace();
				}

				if (!didNotDeleteAnimation) {
					_spriteAtlasData.AnimationNames.RemoveAt(i);
					_spriteAtlasData.AnimationFrames.RemoveAt(i);
					_spriteAtlasData.AnimationFps.RemoveAt(i);
					break;
				}
				ImGui.PopID();
			}

			NezImGui.SmallVerticalSpace();
			ImGui.SliderInt("Preview Size", ref _animationPreviewSize, 50, 150);

			if (ImGui.BeginPopup("add-animation")) {
				ImGui.Text("Animation Name");
				ImGui.InputText("##animationName", ref _stringBuffer, 25);

				if (ImGui.Button("Cancel")) {
					_stringBuffer = "";
					ImGui.CloseCurrentPopup();
				}

				ImGui.SameLine(ImGui.GetContentRegionAvail().X - ImGui.GetItemRectSize().X);

				ImGui.PushStyleColor(ImGuiCol.Button, Color.Green.PackedValue);
				if (ImGui.Button("Create")) {
					_stringBuffer = _stringBuffer.Length > 0 ? _stringBuffer : Utils.RandomString(8);
					_spriteAtlasData.AnimationNames.Add(_stringBuffer);
					_spriteAtlasData.AnimationFps.Add(8);
					_spriteAtlasData.AnimationFrames.Add(new List<int>());

					_stringBuffer = "";
					ImGui.CloseCurrentPopup();
				}
				ImGui.PopStyleColor();

				ImGui.EndPopup();
			}
		}

		#endregion

		#region Helpers

		private void DrawRect(Num.Vector2 cursorPos, Rectangle rect, int rectIndex) {
			Num.Vector2 topLeftScreen = cursorPos + rect.Location.ToNumerics() * _imageZoom;
			ImGui.GetWindowDrawList().AddRect(topLeftScreen, topLeftScreen + rect.GetSize().ToNumerics() * _imageZoom, Color.Red.PackedValue);

			if (_showSourceRectIndexes) {
				ImGui.GetWindowDrawList().AddText(topLeftScreen, Color.Yellow.PackedValue, rectIndex.ToString());
			}
		}

		private void DrawOrigin(Num.Vector2 cursorPos, Vector2 origin, Rectangle rect) {
			Num.Vector2 topLeftScreen = cursorPos + rect.Location.ToNumerics() * _imageZoom;
			Vector2 offsetInImage = origin * rect.GetSize().ToVector2() * _imageZoom;
			Num.Vector2 center = topLeftScreen + offsetInImage.ToNumerics();
			ImGui.GetWindowDrawList().AddCircleFilled(center, 5, Color.Orange.PackedValue, 4);
		}

		private void CenterImage() {
			Num.Vector2 size = _textureSize * _imageZoom;
			_imagePosition = (ImGui.GetContentRegionAvail() - size) * 0.5f;
		}

		private void FrameImage() {
			Num.Vector2 fitSize = CalcFitToScreen();
			_imageZoom = fitSize.X / _textureSize.X;
			_imagePosition = (ImGui.GetContentRegionAvail() - fitSize) * 0.5f;
		}

		private Num.Vector2 CalcFitToScreen() {
			Num.Vector2 availSize = ImGui.GetContentRegionMax();
			Num.Vector2 autoScale = _textureSize / availSize;
			if (autoScale.X > autoScale.Y) {
				return new Num.Vector2(availSize.X, availSize.X / _textureAspectRatio);
			}

			return new Num.Vector2(availSize.Y * _textureAspectRatio, availSize.Y);
		}

		private Num.Vector2 CalcFillScreen() {
			Num.Vector2 availSize = ImGui.GetContentRegionAvail();
			Num.Vector2 autoScale = _textureSize / availSize;
			if (autoScale.X < autoScale.Y) {
				return new Num.Vector2(availSize.X, availSize.X / _textureAspectRatio);
			}

			return new Num.Vector2(availSize.Y * _textureAspectRatio, availSize.Y);
		}

		private Num.Vector2 CalcBestFitRegion(Num.Vector2 availSize, Num.Vector2 textureSize) {
			float aspectRatio = textureSize.X / textureSize.Y;
			Num.Vector2 autoScale = _textureSize / availSize;
			if (autoScale.X < autoScale.Y) {
				return new Num.Vector2(availSize.X, availSize.X / aspectRatio);
			}

			return new Num.Vector2(availSize.Y * aspectRatio, availSize.Y);
		}

		private void GenerateRects(int cellWidth, int cellHeight, int totalFrames, int padding, int cellOffset = 0) {
			_spriteAtlasData.Clear();
			_hasSlicedContent = true;

			float cols = _textureSize.X / cellWidth;
			float rows = _textureSize.Y / cellHeight;
			int i = 0;

			for (int y = 0; y < rows; y++) {
				for (int x = 0; x < cols; x++) {
					// skip everything before the first cellOffset
					if (i++ < cellOffset) {
						continue;
					}

					int padX = padding * x;
					int padY = padding * y;
					_spriteAtlasData.SourceRects.Add(new Rectangle(x * cellWidth + padX, y * cellHeight + padY, cellWidth, cellHeight));
					_spriteAtlasData.Names.Add($"F{i}");
					_spriteAtlasData.Origins.Add(Vector2Ext.HalfVector());

					// once we hit the max number of cells to include bail out. were done.
					if (_spriteAtlasData.SourceRects.Count == totalFrames) {
						return;
					}
				}
			}
		}

		#endregion

		~SpriteAtlasEditorWindow() {
			if (_texturePtr != IntPtr.Zero) {
				Core.GetGlobalManager<ImGuiManager>().UnbindTexture(_texturePtr);
			}
		}

	}
}