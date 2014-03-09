using System;

namespace Grasshopper.Engine.Rendering.UserInterface
{
	public abstract class PanelRenderer : Renderer
	{
		private float? _height;
		private float? _width;
		private readonly object _lock = new object();

		public event Action HeightChanged;
		public event Action WidthChanged;

		protected PanelRenderer()
		{
			ChildRendererAttached += OnChildRendererAttached;
			ChildRendererReleased += OnChildRendererReleased;
		}

		public float Height
		{
			get
			{
				lock(_lock)
					return _height ?? CalculateHeightInternal();
			}
		}

		public float Width
		{
			get
			{
				lock(_lock)
					return _width ?? CalculateWidthInternal();
			}
		}

		private float CalculateHeightInternal()
		{
			_height = CalculateHeight();
			var handler = HeightChanged;
			if(handler != null)
				handler();
			return _height.Value;
		}

		private float CalculateWidthInternal()
		{
			_width = CalculateWidth();
			var handler = WidthChanged;
			if(handler != null)
				handler();
			return _width.Value;
		}

		protected abstract float CalculateWidth();
		protected abstract float CalculateHeight();

		private void OnChildRendererAttached(Renderer renderer)
		{
			var r = renderer as PanelRenderer;
			if(r != null)
			{
				r.WidthChanged += OnChildWidthChanged;
				r.HeightChanged += OnChildHeightChanged;
			}
		}

		private void OnChildRendererReleased(Renderer renderer)
		{
			var r = renderer as PanelRenderer;
			if(r != null)
			{
				r.WidthChanged -= OnChildWidthChanged;
				r.HeightChanged -= OnChildHeightChanged;
			}
		}

		private void OnChildWidthChanged()
		{
			ResetWidth();
		}

		private void OnChildHeightChanged()
		{
			ResetHeight();
		}

		protected void ResetWidth()
		{
			_width = null;
			var handler = WidthChanged;
			if(handler != null)
				handler();
		}

		protected void ResetHeight()
		{
			_height = null;
			var handler = HeightChanged;
			if(handler != null)
				handler();
		}

		protected void ResetSize()
		{
			ResetWidth();
			ResetHeight();
		}
	}
}
