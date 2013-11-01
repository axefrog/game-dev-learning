using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace SharpDx4.Direct3D
{
	public class SceneRenderData : IDisposable
	{
		public Scene Scene { get; private set; }
		public Dictionary<int, ModelDeviceData> ModelRenderData { get; private set; }

		public SceneRenderData(Scene scene, Device device)
		{
			ModelRenderData = new Dictionary<int, ModelDeviceData>();
			Scene = scene;

			foreach (var model in Scene.Models)
			{
				var buffer = new ModelDeviceData(model, device);
				ModelRenderData.Add(model.Id, buffer);
			}
		}

		public void Dispose()
		{
			foreach(var value in ModelRenderData.Values)
				value.Dispose();
		}
	}
}
