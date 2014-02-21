using System;
using SharpDX.Direct3D;
using SharpDX.Windows;
using SharpDx5.Direct3D;
using SharpDx5.Game;

namespace SharpDx5
{
	public static class App
	{
		[STAThread]
		public static void Main()
		{
			if (SharpDX.Direct3D11.Device.GetSupportedFeatureLevel() != FeatureLevel.Level_11_0)
			{
				System.Windows.Forms.MessageBox.Show("DirectX11 Not Supported");
				return;
			}

			using(var game = new GameCore())
			using(var form = new GameForm(game.Title))
			using(var device = new RenderingDevice(form, true))
			using(var renderer = new Renderer(device))
			{
				form.Width = 1024;
				form.Height = 768;

				device.SetWireframeRasterState();
				renderer.SetScene(game.Scene);

				RenderLoop.Run(form, () =>
				{
					game.Run();
					renderer.Draw();
					form.Text = game.Title + " (" + renderer.FramesPerSecond + "fps)";
				});
			}
		}
	}
}
