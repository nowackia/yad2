using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Yad.NativeMethods {
	public class NativeMethods {
		[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

		[StructLayout(LayoutKind.Sequential)]
		public struct Message {
			public IntPtr hWnd;
			//public WindowMessage msg;
			public uint msg;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public System.Drawing.Point p;
		}

		static Message msg = new Message();

		public static bool ApplicationIsIdle {
			get { return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0); }
		}
	}
}
