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
	internal class PipeHandle
	{
		private int fHResult;

#if !(SI_DOTNET_1x)
		private SafeFileHandle fHandle;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern SafeFileHandle CreateFile(
			string lpFileName,
			int dwDesiredAccess,
			FileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			FileMode dwCreationDisposition,
			int dwFlagsAndAttributes,
			IntPtr hTemplateFile);
#else
		private IntPtr fHandle;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CreateFile(
			string lpFileName, 
			int dwDesiredAccess,
			FileShare dwShareMode, 
			IntPtr lpSecurityAttributes,
			FileMode dwCreationDisposition,
			int dwFlagsAndAttributes, 
			IntPtr hTemplateFile);
#endif

		/// <summary>
		///   Creates and initializes a new PipeHandle instance with the
		///   specified named pipe.
		/// </summary>
		/// <param name="pipeName">The named pipe to open.</param>

		public PipeHandle(string pipeName)
		{
			this.fHandle = OpenPipe(pipeName);
			this.fHResult = Marshal.GetHRForLastWin32Error();
		}

#if !(SI_DOTNET_1x)
		private static SafeFileHandle OpenPipe(string pipeName)
		{
			SafeFileHandle handle;
#else
		private static IntPtr OpenPipe(string pipeName)
		{
			IntPtr handle;
#endif
			string fileName = "\\\\.\\pipe\\" + pipeName;
			int dwDesiredAccess;

			unchecked
			{
				/* GENERIC_READ | GENERIC_WRITE */
				dwDesiredAccess = (int)(0x40000000 | 0x80000000);
			}

			handle = CreateFile(
				fileName,
				dwDesiredAccess,
				FileShare.None,
				IntPtr.Zero,
				FileMode.Open,
				0,
				IntPtr.Zero);

			return handle;
		}

#if !(SI_DOTNET_1x)
		internal bool IsInvalid
		{
			get { return this.fHandle.IsInvalid; }
		}
#else
		internal bool IsInvalid
		{
			get 
			{
				if (this.fHandle != IntPtr.Zero)
				{
					return this.fHandle == new IntPtr(-1);
				}

				return true;
			}
		}
#endif

		internal int HResult
		{
			get { return this.fHResult; }
		}

#if !(SI_DOTNET_1x)
		internal SafeFileHandle Handle
		{
			get { return this.fHandle; }
		}
#else
		internal IntPtr Handle
		{
			get { return this.fHandle; }
		}
#endif
	}
}
