using System;
using System.Collections.Generic;
using Grasshopper.Engine.Rendering.Shaders.Basic;
using SharpDX;

namespace Grasshopper.Engine.Rendering.Shaders
{
	public class ShaderManager : Component
	{
		private readonly Dictionary<string, RegisteredVertexShader> _vertexShaders = new Dictionary<string, RegisteredVertexShader>();
		private readonly Dictionary<string, RegisteredPixelShader> _pixelShaders = new Dictionary<string, RegisteredPixelShader>();
		private readonly GrasshopperApp _app;

		public ShaderManager(GrasshopperApp app)
		{
			_app = app;
			_app.Pipeline.DeviceChanged += OnDeviceChanged;

			RegisterVertexShaderFromSource("Default", BasicShaderSources.Common, BasicShaderSources.DefaultVertexShader);
			RegisterPixelShaderFromSource("Default", BasicShaderSources.Common, BasicShaderSources.DefaultPixelShader);
		}

		private const string VertexShaderEntryPoint = "VSMain";
		private const string VertexShaderProfile = "vs_5_0";
		private const string PixelShaderEntryPoint = "PSMain";
		private const string PixelShaderProfile = "ps_5_0";
		private const string ErrorVertexShaderAlreadyExists = "A vertex shader with the specified id is already registered";
		private const string ErrorPixelShaderAlreadyExists = "A pixel shader with the specified id is already registered";

		private void OnDeviceChanged()
		{
			var device = _app.DeviceManager.Direct3D.Device;
			foreach(var shader in _vertexShaders.Values)
				shader.Create(device);
			foreach(var shader in _pixelShaders.Values)
				shader.Create(device);
		}

		public void RegisterVertexShaderFromFile(string id, string filename)
		{
			if(_vertexShaders.ContainsKey(id))
				throw new ArgumentException(ErrorVertexShaderAlreadyExists, "id");
			var shader = new RegisteredVertexShader(HlslCompiler.CompileFromFile(filename, VertexShaderEntryPoint, VertexShaderProfile));
			_vertexShaders.Add(id, ToDispose(shader));
		}

		public void RegisterVertexShaderFromSource(string id, params string[] sources)
		{
			if(_vertexShaders.ContainsKey(id))
				throw new ArgumentException(ErrorVertexShaderAlreadyExists, "id");
			var source = string.Join(Environment.NewLine, sources);
			var shader = new RegisteredVertexShader(HlslCompiler.CompileFromSource(source, VertexShaderEntryPoint, VertexShaderProfile));
			_vertexShaders.Add(id, ToDispose(shader));
		}

		public void RegisterPixelShaderFromFile(string id, string filename)
		{
			if(_pixelShaders.ContainsKey(id))
				throw new ArgumentException(ErrorPixelShaderAlreadyExists, "id");
			var shader = new RegisteredPixelShader(HlslCompiler.CompileFromFile(filename, PixelShaderEntryPoint, PixelShaderProfile));
			_pixelShaders.Add(id, ToDispose(shader));
		}

		public void RegisterPixelShaderFromSource(string id, params string[] sources)
		{
			if(_pixelShaders.ContainsKey(id))
				throw new ArgumentException(ErrorPixelShaderAlreadyExists, "id");
			var source = string.Join(Environment.NewLine, sources);
			var shader = new RegisteredPixelShader(HlslCompiler.CompileFromSource(source, PixelShaderEntryPoint, PixelShaderProfile));
			_pixelShaders.Add(id, ToDispose(shader));
		}

		public RegisteredVertexShader GetVertexShader(string id)
		{
			RegisteredVertexShader shader;
			if(!_vertexShaders.TryGetValue(id ?? "Default", out shader))
				throw new Exception("No vertex shader exists with the id \"" + id + "\"");
			return shader;
		}

		public RegisteredPixelShader GetPixelShader(string id)
		{
			RegisteredPixelShader shader;
			if(!_pixelShaders.TryGetValue(id ?? "Default", out shader))
				throw new Exception("No vertex shader exists with the id \"" + id + "\"");
			return shader;
		}
	}
}
