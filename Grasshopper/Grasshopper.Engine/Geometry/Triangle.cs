using System.Runtime.InteropServices;

namespace Grasshopper.Engine.Geometry
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Triangle
	{
		public Triangle(uint a, uint b, uint c)
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
