using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDx5.Game.Geometry;

namespace SharpDx5.Direct3D
{
	public class SceneDeviceData : IDisposable
	{
		public Scene Scene { get; private set; }
		public Dictionary<int, ModelDeviceData> ModelRenderData { get; private set; }

		public SceneDeviceData(Scene scene, Device device)
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
