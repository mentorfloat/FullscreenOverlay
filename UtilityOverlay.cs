using System;
using System.Collections.Generic;
using System.Text;
using GameOverlay.Drawing;
using GameOverlay.Windows;

namespace FullscreenOverlay
{
    class UtilityOverlay
    {
		public StickyWindow Myoverlaywin;
		public Graphics Mygfx;

		private Dictionary<string, SolidBrush> _brushes;
		private Dictionary<string, Font> _fonts;
		private Dictionary<string, Image> _images;

		public void CreateOverlay()
		{
			_brushes = new Dictionary<string, SolidBrush>();
			_fonts = new Dictionary<string, Font>();
			_images = new Dictionary<string, Image>();

			Mygfx = new Graphics()
			{
				MeasureFPS = false,
				PerPrimitiveAntiAliasing = true,
				TextAntiAliasing = true,
				WindowHandle = IntPtr.Zero
			};

			Myoverlaywin = new StickyWindow(MainWindow.gameclient, Mygfx)
			{
				FPS = 30,
				IsTopmost = true,
				IsVisible = true,
				AttachToClientArea = true
			};

			Myoverlaywin.DestroyGraphics += Window_DestroyGraphics;
			Myoverlaywin.DrawGraphics += Window_DrawGraphics;
			Myoverlaywin.SetupGraphics += Window_SetupGraphics;
		}

		private void Window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			if (e.RecreateResources)
			{
				foreach (var pair in _brushes) pair.Value.Dispose();
				foreach (var pair in _images) pair.Value.Dispose();
			}

			_brushes["transparent"] = gfx.CreateSolidBrush(0, 0, 0, 0);
			_brushes["redsemi"] = gfx.CreateSolidBrush(255, 0, 0, 160);
			_brushes["orangesemi"] = gfx.CreateSolidBrush(253, 106, 2, 160);
			_brushes["white"] = gfx.CreateSolidBrush(255, 255, 255);
			_brushes["blacksemi"] = gfx.CreateSolidBrush(0, 0, 0, 100);

			if (e.RecreateResources) return;

			_fonts["corbel"] = gfx.CreateFont("Corbel", 16);

		}

		private void Window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
		{
			foreach (var pair in _brushes) pair.Value.Dispose();
			foreach (var pair in _fonts) pair.Value.Dispose();
			foreach (var pair in _images) pair.Value.Dispose();
		}

		private void Window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;
			gfx.ClearScene();

			DrawText(gfx);
		}

		private void DrawText(Graphics gfx)
		{
			if (MainWindow.ShowOverlayText)
			{
				var overlayText = new StringBuilder()
					.Append(MainWindow.myText)
					.ToString();
				gfx.DrawTextWithBackground(_fonts["corbel"], _brushes["white"], _brushes["blacksemi"], 100, 100, overlayText);
			}
		}
	}
}
