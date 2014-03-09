using System;
using System.Collections.Generic;

namespace Grasshopper.Engine.Geometry
{
	public class Model
	{
		public Vertex[] Vertices { get; set; }
		public Triangle[] Triangles { get; set; }
		public LinkedList<WorldPosition> Instances { get; set; }
		public string VertexShader { get; set; }
		public string PixelShader { get; set; }

		public event Action InstancesUpdated;
	}
}
