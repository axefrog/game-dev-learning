using System.Runtime.InteropServices;
using SharpDX;

namespace Grasshopper.Engine.Rendering
{
	public class ConstantBuffers
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PerFrame
		{
			public Matrix World;
			public Matrix View;
			public Matrix Projection;
			//public Vector3 CameraPosition;
			//float _padding0; // pad the structure
		}
	}
}
