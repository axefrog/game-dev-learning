using System.Runtime.InteropServices;

namespace SharpDx4.Game.Geometry
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex
	{
		public Vertex(float x, float y, float z, float r, float g, float b, float a)
		{
			Position = new Vector3(x, y, z);
			Color = new Color4(r, g, b, a);
		}

		public Vector3 Position;
		public Color4 Color;
	}
}