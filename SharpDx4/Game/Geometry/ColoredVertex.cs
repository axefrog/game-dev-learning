using System.Runtime.InteropServices;
using SharpDX;

namespace SharpDx4.Game.Geometry
{
	[StructLayout(LayoutKind.Sequential)]
	public struct ColoredVertex
	{
		public ColoredVertex(float x, float y, float z, float r, float g, float b, float a)
		{
			Position = new Vector3(x, y, z);
			Color = new Color4(r, g, b, a);
		}

		public ColoredVertex(Vector3 position, Color4 color)
		{
			Position = position;
			Color = color;
		}

		public Vector3 Position;
		public Color4 Color;
	}
}