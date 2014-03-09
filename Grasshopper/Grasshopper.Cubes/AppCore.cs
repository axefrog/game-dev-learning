using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Cubes.Game;
using Grasshopper.Engine;
using Grasshopper.Engine.Rendering;
using SharpDX;
using SharpDX.Windows;

namespace Grasshopper.Cubes
{
	class AppCore : GrasshopperApp
	{
		private readonly Form _form;
		private readonly GameCore _game;
		private bool _exit;

		public AppCore(Form form, GameCore game) : base(form)
		{
			_form = form;
			_game = game;

			InputManager.Default.KeyDown += OnKeyDown;
			InputManager.Default.KeyUp += OnKeyUp;
		}

		private bool _isCounting;
		private Stopwatch _countingTimer;

		private void OnKeyDown(Keys key)
		{
			switch(key)
			{
				case Keys.Escape:
					_exit = true;
					break;

				case Keys.Space:
					if(!_isCounting)
					{
						_isCounting = true;
						_countingTimer = Stopwatch.StartNew();
					}
					break;
			}
		}

		private void OnKeyUp(Keys key)
		{
			switch(key)
			{
				case Keys.Space:
					_countingTimer.Stop();
					_isCounting = false;
					Debug.Clear("Testing");
					break;
			}
		}

		public void Run()
		{
			var renderFps = new TickCounter();
			var gameFps = new TickCounter();
			var fpsLimiter = new FpsLimiter(60);
			var debugLimiter = new FpsLimiter(20);

			var gamePanel = Debug["Game Core"];
			var rendererPanel = Debug["Renderer"];
			var cameraPanel = Debug["Camera"];
			var sw = Stopwatch.StartNew();
			var exiting = false;

			var modelRenderers = _game.Models.Select(m =>
			{
				var r = new ModelRenderer(m);
				r.Initialize(this);
				return r;
			}).ToArray();

			RenderManager.Camera = _game.ActiveCamera;

			RenderLoop.Run(_form, () =>
			{
				if(_exit)
				{
					if(!exiting)
					{
						exiting = true;
						Dispose();
						Application.Exit();
					}
					return;
				}

				_game.Update(sw.ElapsedMilliseconds);
				gameFps.Tick();

				if(!fpsLimiter.CanRun)
					return;

				Pipeline.Clear(Color.CornflowerBlue);

				if(debugLimiter.CanRun)
				{
					if(_isCounting)
						Debug["Testing"].Set("Counter", _countingTimer.Elapsed);

					gamePanel.Set("Elapsed", sw.Elapsed);
					gamePanel.Set("Main Loop", "{0:#,##0} cycles/sec", gameFps.TicksPerSecond);
					rendererPanel.Set("Frame Rate", "{0:###,##0.0}fps ({1:0.0}ms/frame)", renderFps.TicksPerSecond, renderFps.AverageTickDuration);
					var pos = _game.ActiveCamera.Position;
					cameraPanel.Set("Position", "X: {0:0.###}, Y: {1:0.###}, Z: {2:0.###}", pos.X, pos.Y, pos.Z);
					pos = _game.ActiveCamera.Target;
					cameraPanel.Set("Target", "X: {0:0.###}, Y: {1:0.###}, Z: {2:0.###}", pos.X, pos.Y, pos.Z);
				}

				// Draw 3D scene objects
				RenderManager.RenderFrame(modelRenderers);
				Debug.Render();

				// Render everything to the screen
				Pipeline.Present();
				renderFps.Tick();
			});
		}
	}
}
