﻿using System;
using SharpDX;
using SharpDx4.Game.Geometry;
using Model = SharpDx4.Game.Geometry.Model;

namespace SharpDx4.Game
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
			for(var i = 0; i < 5000; i++)
				model.Instances.Add(new ModelInstance(new Vector3(
					rand.NextFloat(-30f, 30f), rand.NextFloat(-30f, 30f), rand.NextFloat(-30f, 30f)),
					rand.NextFloat(0.01f, .75f),
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
			get { return "SharpDx4: Cubes"; }
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
				new ColoredVertex(-1.0f, 1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f),
				new ColoredVertex(1.0f, 1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 1.0f),
				new ColoredVertex(1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f),
				new ColoredVertex(-1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f),
				new ColoredVertex(-1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f),
				new ColoredVertex(1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 1.0f),
				new ColoredVertex(1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f),
				new ColoredVertex(-1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f)
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
