using System.Runtime.InteropServices;
using SharpDX;

namespace SharpDx5.Direct3D
{
	[StructLayout(LayoutKind.Sequential)]
	public struct ViewTransformMatrices
	{
		public Matrix World;
		public Matrix View;
		public Matrix Projection;
	}
}
