using System;
using SharpDX.Direct3D;
using SharpDX.Windows;
using SharpDx4.Direct3D;
using SharpDx4.Game;
using SharpDx4.Game.Geometry;

namespace SharpDx4
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
