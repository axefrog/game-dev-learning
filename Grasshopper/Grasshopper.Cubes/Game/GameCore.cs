using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grasshopper.Engine;
using Grasshopper.Engine.Geometry;
using Grasshopper.Engine.Geometry.Primitives;
using SharpDX;

namespace Grasshopper.Cubes.Game
{
	public class GameCore : IDisposable
	{
		private Model _cubes;
		private Camera _camera;

		public IEnumerable<Model> Models { get; private set; }
		public Camera ActiveCamera { get; private set; }

		public string Title
		{
			get { return "Cubes"; }
		}

		public void Initialize()
		{
			_cubes = Cube.Rainbow();
			var rand = new Random();
			const float range = 1f;
			_cubes.Instances = new LinkedList<WorldPosition>(new[] { new WorldPosition(new Vector3(0, 0, 0), 1, .2f, .2f, .2f) });
			_cubes.Instances = new LinkedList<WorldPosition>(Enumerable.Range(0, 100).Select(i => new WorldPosition(new Vector3(
				rand.NextFloat(-range, range), rand.NextFloat(-range, range), rand.NextFloat(-range, range)),
				1f, //rand.NextFloat(0.01f, .75f),
				rand.NextFloat(0, (float)Math.PI / 2),
				rand.NextFloat(0, (float)Math.PI / 2),
				rand.NextFloat(0, (float)Math.PI / 2))));

			_camera = new Camera(new Vector3(10, 100, -100), Vector3.Zero);

			Models = new[] { _cubes };
			ActiveCamera = _camera;
		}

		private FpsLimiter _cubeUpdateLimiter = new FpsLimiter(60);
		private Stopwatch _sw = Stopwatch.StartNew();
		public void Update()
		{
			if(_cubeUpdateLimiter.CanRun)
				Parallel.ForEach(_cubes.Instances, cube =>
				{
					cube.Pitch = (float)_sw.Elapsed.TotalSeconds / 2;
					cube.Yaw = (float)_sw.Elapsed.TotalSeconds / 3;
					cube.Roll = (float)_sw.Elapsed.TotalSeconds / 4;
					cube.Update();
				});
		}

		public void Dispose()
		{
		}
	}
}
