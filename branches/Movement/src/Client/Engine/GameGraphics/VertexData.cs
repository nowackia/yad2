using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Yad.Engine.GameGraphics.Client {
	class VertexData {
		public float[] vertex = new float[3 * 4];
		public float[] uv = new float[2 * 4];
		public UInt16[] indices = new UInt16[4];
		public IntPtr[] intPointers = new IntPtr[3];
		public GCHandle[] handles = new GCHandle[3];
		public VertexData() {
			handles[0] = GCHandle.Alloc(vertex, GCHandleType.Pinned);
			handles[1] = GCHandle.Alloc(uv, GCHandleType.Pinned);
			handles[2] = GCHandle.Alloc(indices, GCHandleType.Pinned);
			for (int i = 0; i < 3; i++)
				intPointers[i] = handles[i].AddrOfPinnedObject();
		}

		~VertexData() {
			for (int i = 0; i < 3; i++)
				handles[i].Free();
		}
	}
}
