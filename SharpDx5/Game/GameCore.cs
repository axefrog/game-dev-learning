using System;
using SharpDX;
using SharpDx5.Game.Geometry;

namespace SharpDx5.Game
{
	public class GameCore : IDisposable
	{
		private readonly Scene _scene;

		public GameCore()
		{
			_scene = new Scene();
			var model = GetCubeModel();
			Scene.Models.Add(model);
			var rand = new Random();
			for(var i = 0; i < 1000; i++)
				model.Instances.Add(new ModelInstance(new Vector3(
					rand.NextFloat(-30f, 30f), rand.NextFloat(-30f, 30f), rand.NextFloat(-30f, 30f)),
					rand.NextFloat(0.1f, .75f),
					rand.NextFloat(0, (float)Math.PI / 2),
					rand.NextFloat(0, (float)Math.PI / 2),
					rand.NextFloat(0, (float)Math.PI / 2)));
		}

		public Scene Scene
		{
			get { return _scene; }
		}

		public string Title
		{
			get { return "SharpDx5: Cubes"; }
		}

		public void Run()
		{
			_scene.Update();
		}

		public void Dispose()
		{
		}

		private static Model GetCubeModel()
		{
			var vertices = new[]
			{
				new TexturedVertex(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f), // Front
				new TexturedVertex(-1.0f, 1.0f, -1.0f, 0.0f, 0.0f),
				new TexturedVertex(1.0f, 1.0f, -1.0f, 1.0f, 0.0f),
				new TexturedVertex(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f),
				new TexturedVertex(1.0f, 1.0f, -1.0f, 1.0f, 0.0f),
				new TexturedVertex(1.0f, -1.0f, -1.0f, 1.0f, 1.0f),
				new TexturedVertex(-1.0f, -1.0f, 1.0f, 1.0f, 0.0f), // BACK
				new TexturedVertex(1.0f, 1.0f, 1.0f, 0.0f, 1.0f),
				new TexturedVertex(-1.0f, 1.0f, 1.0f, 1.0f, 1.0f),
				new TexturedVertex(-1.0f, -1.0f, 1.0f, 1.0f, 0.0f),
				new TexturedVertex(1.0f, -1.0f, 1.0f, 0.0f, 0.0f),
				new TexturedVertex(1.0f, 1.0f, 1.0f, 0.0f, 1.0f),
				new TexturedVertex(-1.0f, 1.0f, -1.0f, 0.0f, 1.0f), // Top
				new TexturedVertex(-1.0f, 1.0f, 1.0f, 0.0f, 0.0f),
				new TexturedVertex(1.0f, 1.0f, 1.0f, 1.0f, 0.0f),
				new TexturedVertex(-1.0f, 1.0f, -1.0f, 0.0f, 1.0f),
				new TexturedVertex(1.0f, 1.0f, 1.0f, 1.0f, 0.0f),
				new TexturedVertex(1.0f, 1.0f, -1.0f, 1.0f, 1.0f),
				new TexturedVertex(-1.0f, -1.0f, -1.0f, 1.0f, 0.0f), // Bottom
				new TexturedVertex(1.0f, -1.0f, 1.0f, 0.0f, 1.0f),
				new TexturedVertex(-1.0f, -1.0f, 1.0f, 1.0f, 1.0f),
				new TexturedVertex(-1.0f, -1.0f, -1.0f, 1.0f, 0.0f),
				new TexturedVertex(1.0f, -1.0f, -1.0f, 0.0f, 0.0f),
				new TexturedVertex(1.0f, -1.0f, 1.0f, 0.0f, 1.0f),
				new TexturedVertex(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f), // Left
				new TexturedVertex(-1.0f, -1.0f, 1.0f, 0.0f, 0.0f),
				new TexturedVertex(-1.0f, 1.0f, 1.0f, 1.0f, 0.0f),
				new TexturedVertex(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f),
				new TexturedVertex(-1.0f, 1.0f, 1.0f, 1.0f, 0.0f),
				new TexturedVertex(-1.0f, 1.0f, -1.0f, 1.0f, 1.0f),
				new TexturedVertex(1.0f, -1.0f, -1.0f, 1.0f, 0.0f), // Right
				new TexturedVertex(1.0f, 1.0f, 1.0f, 0.0f, 1.0f),
				new TexturedVertex(1.0f, -1.0f, 1.0f, 1.0f, 1.0f),
				new TexturedVertex(1.0f, -1.0f, -1.0f, 1.0f, 0.0f),
				new TexturedVertex(1.0f, 1.0f, -1.0f, 0.0f, 0.0f),
				new TexturedVertex(1.0f, 1.0f, 1.0f, 0.0f, 1.0f)
			};

			var indices = new[]
			{
				new TriangleIndex(3, 1, 0),
				new TriangleIndex(2, 1, 3),
				new TriangleIndex(0, 5, 4),
				
				new TriangleIndex(1, 5, 0),
				new TriangleIndex(3, 4, 7),
				new TriangleIndex(0, 4, 3),
				
				new TriangleIndex(1, 6, 5),
				new TriangleIndex(2, 6, 1),
				new TriangleIndex(2, 7, 6),
				
				new TriangleIndex(3, 7, 2),
				new TriangleIndex(6, 4, 5),
				new TriangleIndex(7, 4, 6)
			};

			return new Model(vertices, indices);
		}
	}
}
