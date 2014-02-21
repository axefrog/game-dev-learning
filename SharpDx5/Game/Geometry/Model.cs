using System.Collections.Generic;
using System.Threading;

namespace SharpDx5.Game.Geometry
{
	public class Model
	{
		private static int _id;
		
		public int Id { get; set; }
		public ColoredVertex[] Vertices { get; set; }
		public TriangleIndex[] Triangles { get; set; }
		public List<ModelInstance> Instances { get; set; }
		
		public Model(ColoredVertex[] vertices, TriangleIndex[] triangles)
		{
			Id = Interlocked.Increment(ref _id);
			Vertices = vertices;
			Triangles = triangles;
			Instances = new List<ModelInstance>();
		}

		public void Update(long elapsedMilliseconds)
		{
			foreach(var instance in Instances)
				instance.Update(elapsedMilliseconds);
		}
	}
}