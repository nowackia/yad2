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
			for (int i = 0; i < indices.Length; i++) {
				indices[i] = (ushort)i;
			}

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

	class VertexDataArray {
		public float[] vertices;// = new float[3 * 4];
		public float[] uv;// = new float[2 * 4];
		public UInt16[] indices;// = new UInt16[4];
		GCHandle[] _handles = new GCHandle[3];
		IntPtr[] _intPointers = new IntPtr[3];

		public VertexDataArray(int length) {
			vertices = new float[3 * length];
			uv = new float[2 * length];
			indices = new UInt16[length];

			for (int i = 0; i < indices.Length; i++) {
				indices[i] = (ushort)i;
			}

			_handles[0] = GCHandle.Alloc(vertices, GCHandleType.Pinned);
			_handles[1] = GCHandle.Alloc(uv, GCHandleType.Pinned);
			_handles[2] = GCHandle.Alloc(indices, GCHandleType.Pinned);
			for (int i = 0; i < 3; i++)
				_intPointers[i] = _handles[i].AddrOfPinnedObject();
		}

		~VertexDataArray() {
			for (int i = 0; i < 3; i++)
				_handles[i].Free();
		}

		public IntPtr VerticesPtr {
			get { return _intPointers[0]; }
		}

		public IntPtr IndicesPtr {
			get { return _intPointers[2]; }
		}

		public IntPtr UVPtr {
			get { return _intPointers[1]; }
		}
	}
}
