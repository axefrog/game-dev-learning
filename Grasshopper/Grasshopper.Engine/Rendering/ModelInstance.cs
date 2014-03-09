using System.Runtime.InteropServices;
using SharpDX;

namespace Grasshopper.Engine.Rendering
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ModelInstance
	{
		public Matrix World;
		public Matrix WorldInverseTranspose;
		public Matrix WorldViewProjection;

		//public void Transpose()
		//{
		//	World.Transpose();
		//	WorldInverseTranspose.Transpose();
		//	WorldViewProjection.Transpose();
		//}
	}
}