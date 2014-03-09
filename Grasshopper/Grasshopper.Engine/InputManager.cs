using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Multimedia;
using SharpDX.RawInput;

namespace Grasshopper.Engine
{
	public class InputManager : IDisposable
	{
		static InputManager()
		{
			Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None);
		}

		private static readonly object _lock = new object();
		private static InputManager _instance;
		public static InputManager Default
		{
			get
			{
				lock(_lock)
					return _instance ?? (_instance = new InputManager());
			}
		}

		public event KeyEventHandler KeyDown;
		public event KeyEventHandler KeyUp;

		public InputManager()
		{
			Device.KeyboardInput += OnKeyboardInput;
		}

		private readonly HashSet<Keys> _keysDown = new HashSet<Keys>();
		private void OnKeyboardInput(object sender, KeyboardInputEventArgs args)
		{
			switch(args.State)
			{
				case KeyState.KeyUp:
				{
					_keysDown.Remove(args.Key);
					var handler = KeyUp;
					if(handler != null)
						handler(args.Key);
					break;
				}

				case KeyState.KeyDown:
				{
					_keysDown.Add(args.Key);
					var handler = KeyDown;
					if(handler != null)
						handler(args.Key);
					break;
				}
			}
		}

		public bool IsKeyDown(Keys key)
		{
			return _keysDown.Contains(key);
		}

		public void Dispose()
		{
			Device.KeyboardInput -= OnKeyboardInput;
		}
	}

	public delegate void KeyEventHandler(Keys key);
}
