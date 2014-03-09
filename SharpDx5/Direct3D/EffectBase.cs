using System.Collections;
using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace SharpDx5.Direct3D
{
	public abstract class EffectBase
	{
		public abstract void PrepareInputAssembler(DeviceContext context, ModelDeviceData modelData);
		public abstract void Draw(DeviceContext context, ModelDeviceData modelData, long elapsedMilliseconds);
		public abstract void UpdateConstants(ViewTransformMatrices matrices);

		protected void Draw(DeviceContext context, IEnumerable<EffectPass> passes, ModelDeviceData modelData, long elapsedMilliseconds)
		{
			foreach(var pass in passes)
			{
				pass.Apply(context);
				modelData.Draw(context, elapsedMilliseconds);
			}
		}

	}
}
