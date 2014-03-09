using System.Runtime.InteropServices;
using SharpDX;

namespace Grasshopper.Engine.Rendering
{
	public class ConstantBuffers
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PerFrame
		{
			public Vector3 CameraPosition;
			float _padding0; // pad the structure
		}
	}
}
