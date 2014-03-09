using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace Grasshopper.Engine.Rendering.UserInterface
{
	public class DebugInfoPanel : PanelRenderer
	{
		private readonly List<GroupRenderer> _groups = new List<GroupRenderer>();
		private readonly Dictionary<string, int> _indices = new Dictionary<string, int>();
		private SolidColorBrush _panelBrush;

		public static float GroupSpacing { get; set; }
		public static float PanelMargin { get; set; }

		static DebugInfoPanel()
		{
			GroupSpacing = 5f;
			PanelMargin = 10f;
		}

		protected override float CalculateWidth()
		{
			var width = 0f;
			var maxLabelWidth = 0f;
			foreach(var g in _groups)
			{
				if(g.Empty)
					continue;
				width = Math.Max(width, g.Width);
				maxLabelWidth = Math.Max(maxLabelWidth, g.CalculateMaxLabelWidth());
			}
			_groups.ForEach(g => g.MaxLabelWidth = maxLabelWidth);
			if(width > 0)
				width += PanelMargin * 2;
			return width;
		}

		protected override float CalculateHeight()
		{
			var height = 0f;
			foreach(var g in _groups)
			{
				if(g.Empty)
					continue;
				if(height > 0)
					height += GroupSpacing;
				height += g.Height;
			}
			if(height > 0)
				height += PanelMargin * 2;
			return height;
		}

		protected override void CreateDeviceDependentResources()
		{
			base.CreateDeviceDependentResources();
			_panelBrush = new SolidColorBrush(App.DeviceManager.Direct2D.Context, new Color4(Color3.Black, 0.2f));
		}

		private GroupRenderer GetGroup(string title)
		{
			lock(_groups)
			{
				int n;
				if(_indices.TryGetValue(title ?? "", out n))
					return _groups[n];
				var group = Attach(new GroupRenderer(title));
				group.Initialize(App);
				_indices.Add(title ?? "", _groups.Count);
				_groups.Add(group);
				return group;
			}
		}

		public GroupRenderer this[string title]
		{
			get { return GetGroup(title); }
		}

		public void Clear()
		{
			lock(_groups)
			{
				_indices.Clear();
				foreach(var group in _groups)
					Release(group);
				_groups.Clear();
			}
		}

		public void Clear(string title)
		{
			lock(_groups)
			{
				int n;
				if(!_indices.TryGetValue(title, out n))
					return;

				_indices.Remove(title ?? "");
				Release(_groups[n]);
				_groups.RemoveAt(n);
			}
		}

		protected override void OnRender()
		{
			lock(_groups)
			{
				var context = DeviceManager.Direct2D.Context;
				context.BeginDraw();

				context.Transform = Matrix.Identity;

				const float left = 10f;
				const float top = 10f;
				const float radius = 3f;

				var panelRect = new RoundedRectangle
				{
					RadiusX = radius,
					RadiusY = radius,
					Rect = new RectangleF(left, top, Width, Height)
				};

				context.FillRoundedRectangle(ref panelRect, _panelBrush);

				var groupTop = top + PanelMargin;
				foreach(var group in _groups)
				{
					group.Left = left + PanelMargin;
					group.Top = groupTop;
					group.Render();
					groupTop += group.Height + GroupSpacing;
				}

				context.EndDraw();
			}
		}

		public class GroupRenderer : PanelRenderer
		{
			public static float TitleMargin { get; set; }
			public static float LineSpacing { get; set; }

			static GroupRenderer()
			{
				TitleMargin = 2f;
				LineSpacing = 2f;
			}

			private readonly SortedDictionary<string, LabelPairRenderer> _pairs = new SortedDictionary<string, LabelPairRenderer>();
			public float MaxLabelWidth { get; set; }

			public float Left { get; set; }
			public float Top { get; set; }

			public GroupTitleRenderer Title { get; private set; }

			protected override float CalculateWidth()
			{
				var maxValueWidth = _pairs.Values.Max(v => v.Value.Width);
				return MaxLabelWidth + maxValueWidth + LabelPairRenderer.LabelMargin;
			}

			public float CalculateMaxLabelWidth()
			{
				return _pairs.Values.Max(v => v.Label.Width);
			}

			protected override float CalculateHeight()
			{
				var height = _pairs.Values.Sum(v => v.Height);
				if(height > 0)
				{
					if(Title != null)
						height += Title.Height + TitleMargin;
					height += (_pairs.Count - 1) * LineSpacing;
				}
				return height;
			}

			public GroupRenderer Set(string label, string value)
			{
				lock(_pairs)
				{
					LabelPairRenderer renderer;
					if(_pairs.TryGetValue(label, out renderer))
						renderer.Value.Text = value;
					else
					{
						renderer = Attach(new LabelPairRenderer(label, value));
						renderer.Initialize(App);
						_pairs.Add(label, renderer);
					}
				}
				return this;
			}

			public GroupRenderer Set(string label, object value)
			{
				return Set(label, (value ?? "").ToString());
			}

			public GroupRenderer Set(string label, string format, params object[] args)
			{
				return Set(label, string.Format(format, args));
			}

			public void Remove(string label)
			{
				lock(_pairs)
				{
					LabelPairRenderer renderer;
					if(_pairs.TryGetValue(label, out renderer))
					{
						_pairs.Remove(label);
						Release(renderer);
					}
				}
				ResetSize();
			}

			public void Clear()
			{
				lock(_pairs)
				{
					var values = _pairs.Values.ToArray();
					_pairs.Clear();
					foreach(var renderer in values)
						Release(renderer);
				}
				ResetSize();
			}

			public bool Empty
			{
				get { return _pairs.Count == 0; }
			}

			public GroupRenderer(string title)
			{
				Title = Attach(new GroupTitleRenderer(title));
			}

			public override void Initialize(GrasshopperApp app)
			{
				base.Initialize(app);
				Title.Initialize(app);
			}

			protected override void OnRender()
			{
				Title.Top = Top;
				Title.Left = Left;
				Title.Render();

				var top = Top + Title.Height + TitleMargin;
				foreach(var pair in _pairs.Values)
				{
					pair.Top = top;
					pair.Left = Left;
					pair.MaxLabelWidth = MaxLabelWidth;
					pair.Render();
					top += pair.Height + LineSpacing;
				}
			}
		}

		public class LabelPairRenderer : PanelRenderer
		{
			public static float LabelMargin { get; set; }

			static LabelPairRenderer()
			{
				LabelMargin = 10f;
			}

			public float Left { get; set; }
			public float Top { get; set; }
			public float MaxLabelWidth { get; set; }

			public LabelRenderer Label { get; private set; }
			public ValueRenderer Value { get; private set; }

			protected override float CalculateWidth()
			{
				return Label.Width + Value.Width + LabelMargin;
			}

			protected override float CalculateHeight()
			{
				return Math.Max(Label.Height, Value.Height);
			}

			public LabelPairRenderer(string label, string value)
			{
				Label = Attach(new LabelRenderer(label));
				Value = Attach(new ValueRenderer(value));
			}

			public override void Initialize(GrasshopperApp app)
			{
				base.Initialize(app);
				
				Label.Initialize(app);
				Value.Initialize(app);
			}

			protected override void OnRender()
			{
				Label.Left = Left;
				Label.Top = Top;
				Label.Render();
				Value.Left = Left + MaxLabelWidth + LabelMargin;
				Value.Top = Top;
				Value.Render();
			}
		}

		public abstract class TextRenderer : PanelRenderer
		{
			protected TextRenderer(string text)
			{
				_text = text;
			}

			public float Left { get; set; }
			public float Top { get; set; }

			public string Text
			{
				get { return _text; }
				set
				{
					_text = value;
					ResetSize();
					CreateTextLayout();
				}
			}

			private TextLayout _textLayout;
			private TextFormat _textFormat;
			private Brush _brush;
			private string _text;

			protected override void CreateDeviceDependentResources()
			{
				base.CreateDeviceDependentResources();

				RemoveAndDispose(ref _textFormat);
				RemoveAndDispose(ref _brush);

				OnCreate();

				CreateTextLayout();
			}

			private void CreateTextLayout()
			{
				RemoveAndDispose(ref _textLayout);
				_textLayout = ToDispose(new TextLayout(DeviceManager.Direct2D.DirectWriteFactory, FormatText(), _textFormat, App.Width, App.Height));
			}

			protected abstract void OnCreate();

			public TextFormat TextFormat
			{
				get { return _textFormat; }
				protected set { _textFormat = value; }
			}

			public Brush Brush
			{
				get { return _brush; }
				protected set { _brush = value; }
			}

			protected override void OnRender()
			{
				var context = DeviceManager.Direct2D.Context;
				context.DrawText(FormatText(), _textFormat, new RectangleF(Left, Top, App.Width - Left, App.Height - Top), _brush);
			}

			protected virtual string FormatText()
			{
				return Text;
			}

			protected override float CalculateWidth()
			{
				return _textLayout.Metrics.Width;
			}

			protected override float CalculateHeight()
			{
				return _textLayout.Metrics.Height;
			}
		}

		public class GroupTitleRenderer : TextRenderer
		{
			public GroupTitleRenderer(string text) : base(text) { }

			protected override void OnCreate()
			{
				TextFormat = ToDispose(new TextFormat(DeviceManager.Direct2D.DirectWriteFactory, "Consolas", FontWeight.Bold, FontStyle.Normal, 13f));
				Brush = ToDispose(new SolidColorBrush(App.DeviceManager.Direct2D.Context, new Color4(Color3.Black, 0.5f)));
			}
		}

		public class LabelRenderer : TextRenderer
		{
			public LabelRenderer(string text) : base(text) { }

			protected override void OnCreate()
			{
				TextFormat = ToDispose(new TextFormat(DeviceManager.Direct2D.DirectWriteFactory, "Consolas", FontWeight.Normal, FontStyle.Normal, 13f));
				Brush = ToDispose(new SolidColorBrush(App.DeviceManager.Direct2D.Context, new Color4(new Color3(1f, 1f, .5f))));
			}

			protected override string FormatText()
			{
				return Text + ":";
			}
		}

		public class ValueRenderer : TextRenderer
		{
			public ValueRenderer(string text) : base(text) { }

			protected override void OnCreate()
			{
				TextFormat = ToDispose(new TextFormat(DeviceManager.Direct2D.DirectWriteFactory, "Consolas", FontWeight.Normal, FontStyle.Normal, 13f));
				Brush = ToDispose(new SolidColorBrush(App.DeviceManager.Direct2D.Context, new Color4(Color3.White)));
			}
		}
	}
}