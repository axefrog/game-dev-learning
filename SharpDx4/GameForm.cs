using System;
using System.Windows.Forms;
using SharpDX.Windows;

namespace SharpDx4
{
	public class GameForm : RenderForm
	{
		public GameForm(string title)
			: base(title)
		{
			Width = 800;
			Height = 600;
			AllowUserResizing = true;

			KeyDown += OnKeyDown;
			ResizeEnd += OnResizeEnd;
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Alt && e.KeyCode == Keys.Enter)
			{
				var handler = ToggleFullScreen;
				if (handler != null)
					handler();
			}
		}

		private void OnResizeEnd(object sender, EventArgs eventArgs)
		{
			var handler = GameWindowResized;
			if (handler != null)
				handler(ClientSize.Width, ClientSize.Height);
		}

		public event Action ToggleFullScreen;
		public event GameWindowResizedHandler GameWindowResized;
	}

	public delegate void GameWindowResizedHandler(int width, int height);
}