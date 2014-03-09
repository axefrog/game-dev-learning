using System.Linq;
using SharpDX;

namespace Grasshopper.Engine.Geometry.Primitives
{
	public static class Cube
	{
		private static readonly Vector3[] DefaultVertices =
		{
			new Vector3(-.5f, .5f, -.5f),  // 0 front top left
			new Vector3(.5f, .5f, -.5f),   // 1 front top right
			new Vector3(.5f, -.5f, -.5f),  // 2 front base right
			new Vector3(-.5f, -.5f, -.5f), // 3 front base left
			new Vector3(-.5f, .5f, .5f),   // 4 back top left
			new Vector3(.5f, .5f, .5f),    // 5 back top right
			new Vector3(.5f, -.5f, .5f),   // 6 back base right
			new Vector3(-.5f, -.5f, .5f),  // 7 back base left
		};

		private static readonly Vertex[] RainbowVertices =
		{
			new Vertex(DefaultVertices[0], new Color4(0.0f, 0.0f, 1.0f, 1.0f)), 
			new Vertex(DefaultVertices[1], new Color4(0.0f, 1.0f, 0.0f, 1.0f)), 
			new Vertex(DefaultVertices[2], new Color4(0.0f, 1.0f, 1.0f, 1.0f)), 
			new Vertex(DefaultVertices[3], new Color4(1.0f, 0.0f, 0.0f, 1.0f)), 
			new Vertex(DefaultVertices[4], new Color4(1.0f, 0.0f, 1.0f, 1.0f)), 
			new Vertex(DefaultVertices[5], new Color4(1.0f, 1.0f, 0.0f, 1.0f)), 
			new Vertex(DefaultVertices[6], new Color4(1.0f, 1.0f, 1.0f, 1.0f)), 
			new Vertex(DefaultVertices[7], new Color4(0.0f, 0.0f, 0.0f, 1.0f)),
		};

		private static readonly Triangle[] DefaultTriangles =
		{
			new Triangle(3, 1, 0),
			new Triangle(2, 1, 3),
			new Triangle(0, 5, 4),

			new Triangle(1, 5, 0),
			new Triangle(3, 4, 7),
			new Triangle(0, 4, 3),

			new Triangle(1, 6, 5),
			new Triangle(2, 6, 1),
			new Triangle(2, 7, 6),

			new Triangle(3, 7, 2),
			new Triangle(6, 4, 5),
			new Triangle(7, 4, 6),
		};

		public static Model Rainbow()
		{
			return new Model
			{
				Vertices = RainbowVertices.ToArray(),
				Triangles = DefaultTriangles.ToArray()
			};
		}

		public static Model Create()
		{
			return Create(Color.White);
		}

		public static Model Create(Color color)
		{
			return new Model
			{
				Vertices = DefaultVertices.Select(v => new Vertex(v, color)).ToArray(),
				Triangles = DefaultTriangles.ToArray(),
			};
		}
	}
}
