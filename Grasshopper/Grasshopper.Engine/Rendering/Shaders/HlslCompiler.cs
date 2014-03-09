using System.IO;
using System.Reflection;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.IO;

namespace Grasshopper.Engine.Rendering.Shaders
{
	public static class HlslCompiler
	{
		/// <summary>
		/// Compile the HLSL file using the provided <paramref name="entryPoint"/>, shader <paramref name="profile"/> and optionally conditional <paramref name="defines"/>
		/// </summary>
		/// <param name="hlslFile">Absolute path to HLSL file, or path relative to application installation location</param>
		/// <param name="entryPoint">Shader function name e.g. VSMain</param>
		/// <param name="profile">Shader profile, e.g. vs_5_0</param>
		/// <param name="defines">An optional list of conditional defines.</param>
		/// <returns>The compiled ShaderBytecode</returns>
		/// <exception cref="CompilationException">Thrown if the compilation failed</exception>
		public static ShaderBytecode CompileFromFile(string hlslFile, string entryPoint, string profile, ShaderMacro[] defines = null)
		{
			if(!Path.IsPathRooted(hlslFile))
				hlslFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? "", hlslFile);
			var shaderSource = NativeFile.ReadAllText(hlslFile);

			// Compile the shader file
			var flags = ShaderFlags.None;
#if DEBUG
			flags |= ShaderFlags.Debug | ShaderFlags.SkipOptimization;
#endif
			var includeHandler = new HlslFileIncludeHandler(Path.GetDirectoryName(hlslFile));
			var result = ShaderBytecode.Compile(shaderSource, entryPoint, profile, flags, EffectFlags.None, defines, includeHandler, Path.GetFileName(hlslFile));

			if(result.ResultCode.Failure)
				throw new CompilationException(result.ResultCode, result.Message);

			return result;
		}

		public static ShaderBytecode CompileFromSource(string source, string entryPoint, string profile)
		{
			var flags = ShaderFlags.None;
#if DEBUG
			flags |= ShaderFlags.Debug | ShaderFlags.SkipOptimization;
#endif
			var result = ShaderBytecode.Compile(source, entryPoint, profile, flags);

			if(result.ResultCode.Failure)
				throw new CompilationException(result.ResultCode, result.Message);

			return result;
		}

		public static PixelShader PixelShader(Device device, string hlslFile, string entryPoint, ShaderMacro[] defines = null, string profile = "ps_5_0")
		{
			using(var bytecode = CompileFromFile(hlslFile, entryPoint, profile, defines))
			{
				return new PixelShader(device, bytecode);
			}
		}

		public static PixelShader PixelShader(Device device, string source, string entryPoint, string profile = "ps_5_0")
		{
			using(var bytecode = CompileFromSource(source, entryPoint, profile))
			{
				return new PixelShader(device, bytecode);
			}
		}

		public static VertexShader VertexShader(Device device, string hlslFile, string entryPoint, ShaderMacro[] defines = null, string profile = "vs_5_0")
		{
			using(var bytecode = CompileFromFile(hlslFile, entryPoint, profile, defines))
			{
				return new VertexShader(device, bytecode);
			}
		}

		public static VertexShader VertexShader(Device device, string hlslFile, string entryPoint, out ShaderBytecode bytecode, ShaderMacro[] defines = null, string profile = "vs_5_0")
		{
			bytecode = CompileFromFile(hlslFile, entryPoint, profile, defines);
			return new VertexShader(device, bytecode);
		}

		public static VertexShader VertexShader(Device device, string source, string entryPoint, string profile = "vs_5_0")
		{
			using(var bytecode = CompileFromSource(source, entryPoint, profile))
			{
				return new VertexShader(device, bytecode);
			}
		}

		public static VertexShader VertexShader(Device device, string source, string entryPoint, out ShaderBytecode bytecode, string profile = "vs_5_0")
		{
			bytecode = CompileFromSource(source, entryPoint, profile);
			return new VertexShader(device, bytecode);
		}

		public static HullShader HullShader(Device device, string hlslFile, string entryPoint, ShaderMacro[] defines = null, string profile = "hs_5_0")
		{
			using(var bytecode = CompileFromFile(hlslFile, entryPoint, profile, defines))
			{
				return new HullShader(device, bytecode);
			}
		}

		public static DomainShader DomainShader(Device device, string hlslFile, string entryPoint, ShaderMacro[] defines = null, string profile = "ds_5_0")
		{
			using(var bytecode = CompileFromFile(hlslFile, entryPoint, profile, defines))
			{
				return new DomainShader(device, bytecode);
			}
		}

		public static GeometryShader GeometryShader(Device device, string hlslFile, string entryPoint, ShaderMacro[] defines = null, string profile = "gs_5_0")
		{
			using(var bytecode = CompileFromFile(hlslFile, entryPoint, profile, defines))
			{
				return new GeometryShader(device, bytecode);
			}
		}
	}
}
