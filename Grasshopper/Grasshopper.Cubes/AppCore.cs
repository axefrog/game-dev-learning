using System;
using System.Diagnostics;
using System.Windows.Forms;
using Grasshopper.Engine;
using SharpDX;
using SharpDX.Windows;

namespace Grasshopper.Cubes
{
	class AppCore : GrasshopperApp
	{
		private readonly Form _form;
		private readonly GameCore _game;

		public AppCore(Form form, GameCore game) : base(form)
		{
			_form = form;
			_game = game;
		}

		public void Run()
		{
			var renderFps = new TickCounter();
			var gameFps = new TickCounter();
			var fpsLimiter = new FpsLimiter(60);
			var debugLimiter = new FpsLimiter(20);

			var gamePanel = Debug["Game Core"];
			var rendererPanel = Debug["Renderer"];
			var sw = Stopwatch.StartNew();

			RenderLoop.Run(_form, () =>
			{
				_game.Run();
				gameFps.Tick();

				if(!fpsLimiter.CanRun)
					return;

				ClearBackground(Color.CornflowerBlue);

				if(debugLimiter.CanRun)
				{
					gamePanel.Set("Elapsed", sw.Elapsed);
					gamePanel.Set("Game", "{0:#,##0}fps ({1:0.####}ms)", gameFps.FramesPerSecond, gameFps.AverageTickLength);
					rendererPanel.Set("Render", "{0:#,##0.#}fps ({1:0.#}ms)", renderFps.FramesPerSecond, renderFps.AverageTickLength);
				}
				Debug.Render();

				Pipeline.Present();

				renderFps.Tick();
			});
		}

		private void ClearBackground(Color4 color)
		{
			var context = DeviceManager.Direct3D.Context;
			var renderTarget = Pipeline.RenderTargetView;
			context.ClearRenderTargetView(renderTarget, color);
		}
	}
}
