//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;
using System.IO;
#if !(SI_DOTNET_1x)
using Microsoft.Win32.SafeHandles;
#endif
using System.Runtime.InteropServices;

namespace Gurock.SmartInspect
{
	internal class PipeStream: FileStream
	{
#if !(SI_DOTNET_1x)
		public PipeStream(PipeHandle handle) : 
			base(CheckHandle(handle), FileAccess.ReadWrite)
		{
		}

		private static SafeFileHandle CheckHandle(PipeHandle handle)
		{
			if (handle.IsInvalid)
			{
				Marshal.ThrowExceptionForHR(handle.HResult);
			}

			return handle.Handle;
		}
#else
		public PipeStream(PipeHandle handle) : 
			base(CheckHandle(handle), FileAccess.ReadWrite, true)
		{
		}

		private static IntPtr CheckHandle(PipeHandle handle)
		{
			if (handle.IsInvalid)
			{
				Marshal.ThrowExceptionForHR(handle.HResult);
			}

			return handle.Handle;
		}
#endif
	}
}
