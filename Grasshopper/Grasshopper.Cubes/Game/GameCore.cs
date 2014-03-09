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
		private MovableCamera _camera;

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
			const float range = 75f;
			_cubes.Instances = new LinkedList<WorldPosition>(new[] { new AnimatedCube(new Vector3(0, 0, 0), 1.5f, .2f, .2f, .2f) });
			_cubes.Instances = new LinkedList<WorldPosition>(Enumerable.Range(0, 10000).Select(i => new AnimatedCube(new Vector3(
				rand.NextFloat(-range, range), rand.NextFloat(-range, range), rand.NextFloat(-range, range)),
				rand.NextFloat(0.01f, 2.5f),
				rand.NextFloat(0, (float)Math.PI / 2),
				rand.NextFloat(0, (float)Math.PI / 2),
				rand.NextFloat(0, (float)Math.PI / 2))));

			_camera = new MovableCamera();

			Models = new[] { _cubes };
			ActiveCamera = _camera;
		}

		private FpsLimiter _cubeUpdateLimiter = new FpsLimiter(60);
		private Stopwatch _sw = Stopwatch.StartNew();
		public void Update(long elapsedMilliseconds)
		{
			_camera.Update(elapsedMilliseconds);
			if(_cubeUpdateLimiter.CanRun)
				Parallel.ForEach(_cubes.Instances, cube => cube.Update(elapsedMilliseconds));
		}

		public void Dispose()
		{
		}
	}
}
