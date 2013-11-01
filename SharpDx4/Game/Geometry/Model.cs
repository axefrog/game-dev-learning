using System.Collections.Generic;
using System.Threading;
using SharpDX;

namespace SharpDx4.Game.Geometry
{
	public class Model
	{
		private static int _id;
		
		public int Id { get; set; }
		public ColoredVertex[] Vertices { get; set; }
		public TriangleIndex[] Triangles { get; set; }
		public List<ModelInstance> Instances { get; set; }
		
		public Vector3 Position { get; set; }
		public Quaternion Orientation { get; set; }
		public Vector3 Scale { get; set; }

		public Model(ColoredVertex[] vertices, TriangleIndex[] triangles)
		{
			Id = Interlocked.Increment(ref _id);
			Vertices = vertices;
			Triangles = triangles;
			Instances = new List<ModelInstance>();

			Position = Vector3.One;
			Orientation = Quaternion.Identity;
			Scale = Vector3.One;
		}
	}
}