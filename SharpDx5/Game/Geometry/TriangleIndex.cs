using System.Runtime.InteropServices;

namespace SharpDx5.Game.Geometry
{
	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleIndex
	{
		public TriangleIndex(uint a, uint b, uint c)
		{
			A = a;
			B = b;
			C = c;
		}

		public uint A;
		public uint B;
		public uint C;
	}
}