using System;
using System.Collections.Generic;
using Grasshopper.Engine.Rendering.Pipeline;
using SharpDX;

namespace Grasshopper.Engine.Rendering
{
	public abstract class Renderer : Component
	{
		private List<Renderer> _childRenderers;
		private readonly object _crlock = new object();

		protected DeviceManager DeviceManager { get; private set; }
		protected PipelineManager Pipeline { get; private set; }
		protected GrasshopperApp App { get; private set; }
		public bool Visible { get; set; }

		public event RendererAction ChildRendererAttached;
		public event RendererAction ChildRendererReleased;

		protected List<Renderer> ChildRenderers
		{
			get
			{
				lock(_crlock)
					return _childRenderers ?? (_childRenderers = new List<Renderer>());
			}
		}

		protected T Attach<T>(T child) where T : Renderer
		{
			lock(_crlock)
				ChildRenderers.Add(ToDispose(child));
			var handler = ChildRendererAttached;
			if(handler != null)
				handler(child);
			return child;
		}

		protected void Release<T>(T child) where T : Renderer
		{
			lock(_crlock)
				_childRenderers.Remove(child);
			var handler = ChildRendererReleased;
			if(handler != null)
				handler(child);
			RemoveAndDispose(ref child);
		}

		public virtual void Initialize(GrasshopperApp app)
		{
			if(app == null)
				throw new ArgumentNullException("app");
			if(ReferenceEquals(app, App))
				throw new InvalidOperationException("");
			App = app;

			if(App.DeviceManager == null)
				throw new Exception("Unable to initialize renderer: DeviceManager is null");
			DeviceManager = App.DeviceManager;

			if(App.Pipeline == null)
				throw new Exception("Unable to initialize renderer: Pipeline is null");
			Pipeline = App.Pipeline;

			Visible = true;
			
			if(Pipeline != null)
			{
				Pipeline.DeviceChanged -= CreateDeviceDependentResources;
				Pipeline.SizeChanged -= CreateSizeDependentResources;
			}
			
			Pipeline.DeviceChanged += CreateDeviceDependentResources;
			Pipeline.SizeChanged += CreateSizeDependentResources;

			CreateDeviceDependentResources();
			CreateSizeDependentResources();
		}

		protected virtual void CreateDeviceDependentResources()
		{
		}

		protected virtual void CreateSizeDependentResources()
		{
		}

		public void Render()
		{
			if(Visible)
				OnRender();
		}

		protected abstract void OnRender();

		protected void RenderChildren()
		{
			lock(_crlock)
				foreach(var cr in _childRenderers)
					cr.Render();
		}
	}

	public delegate void RendererAction(Renderer renderer);
}
