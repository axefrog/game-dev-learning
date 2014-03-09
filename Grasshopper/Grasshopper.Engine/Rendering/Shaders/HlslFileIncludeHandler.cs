using System;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.IO;

namespace Grasshopper.Engine.Rendering.Shaders
{
	internal class HlslFileIncludeHandler : CallbackBase, Include
	{
		public readonly Stack<string> CurrentDirectory;
		public readonly List<string> IncludeDirectories;


		public HlslFileIncludeHandler(string initialDirectory)
		{
			IncludeDirectories = new List<string>();
			CurrentDirectory = new Stack<string>();
			CurrentDirectory.Push(initialDirectory);
		}

		#region Include Members

		public Stream Open(IncludeType type, string fileName, Stream parentStream)
		{
			var currentDirectory = CurrentDirectory.Peek();
			if(currentDirectory == null)
#if NETFX_CORE
                currentDirectory = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
#else
				currentDirectory = Environment.CurrentDirectory;
#endif

			var filePath = fileName;

			if(!Path.IsPathRooted(filePath))
			{
				var directoryToSearch = new List<string> { currentDirectory };
				directoryToSearch.AddRange(IncludeDirectories);
				foreach(var dirPath in directoryToSearch)
				{
					var selectedFile = Path.Combine(dirPath, fileName);
					if(NativeFile.Exists(selectedFile))
					{
						filePath = selectedFile;
						break;
					}
				}
			}

			if(filePath == null || !NativeFile.Exists(filePath))
			{
				throw new FileNotFoundException(String.Format("Unable to find file [{0}]", filePath ?? fileName));
			}

			NativeFileStream fs = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read);
			CurrentDirectory.Push(Path.GetDirectoryName(filePath));
			return fs;
		}

		public void Close(Stream stream)
		{
			stream.Dispose();
			CurrentDirectory.Pop();
		}

		#endregion
	}
}
