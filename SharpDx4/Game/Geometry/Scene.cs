using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpDX;

namespace SharpDx4.Game.Geometry
{
	public class Scene
	{
		public List<Model> Models { get; set; }
		public Camera Camera { get; set; }

		public Scene()
		{
			Models = new List<Model>();
			Camera = new Camera(new Vector3(-2, 3, -5), new Vector3(0, 1, 0), Vector3.UnitY);
		}

		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private const float _twoPi = (float)Math.PI*2;
		private const int _xPeriod = 5000;
		private const int _yPeriod = 3000;
		private const int _zPeriod = 8000;

		public void Update()
		{
			var phaseX = (float)Math.Sin((_sw.ElapsedMilliseconds % _xPeriod) * _twoPi / _xPeriod);
			var phaseY = (float)Math.Sin((_sw.ElapsedMilliseconds % _yPeriod) * _twoPi / _yPeriod);
			var phaseZ = (float)Math.Sin((_sw.ElapsedMilliseconds % _zPeriod) * _twoPi / _zPeriod);

			Camera.EyePosition = new Vector3(5+4*phaseX, 3+8*phaseY, 5*phaseZ);
		}
	}
}
