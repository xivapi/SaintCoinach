using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Interop
{
    /// <summary>
    /// Obtains an IntPtr to a Mesh object from fbxInterop.dll.
    /// </summary>
    class InteropMesh : IDisposable
    {
        static class Interop
        {
            [DllImport("fbxInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr loadMesh(int index,
                                                [In, Out] InteropVertex[] vertices, int numv,
                                                [In, Out] ushort[] indices, int numi,
                                                [In, Out] ushort[] boneList, int boneListSize);

            [DllImport("fbxInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void unloadMesh(IntPtr m);
        }

        internal IntPtr _UnmanagedPtr;
        public int Index { get; set; }
        public InteropVertex[] Vertices { get; set; }
        public ushort[] Indices { get; set; }
        public ushort[] BoneList { get; set; }
        public int NumVertexes { get; set; }
        public int NumIndices { get; set; }
        public int BoneListSize { get; set; }

        /// <summary>
        /// Merges an array of meshes into one InteropMesh. Unused. Does not include BoneList
        /// </summary>
        /// <param name="ma"></param>
        public InteropMesh(Mesh[] ma)
        {
            int numMeshes = ma.Length;
            int totalVertices = ma.Select(_ => _.Vertices.Length).Sum();
            int totalIndices = ma.Select(_ => _.Indices.Length).Sum();

            int vertAccumulator = 0;
            ushort indexAccumulator = 0;

            Vertices = new InteropVertex[totalVertices];
            Indices = new ushort[totalIndices];
            
            for (int i = 0; i < numMeshes; i++)
            {
                int prevVertSum = 0;
                int prevIndSum = 0;

                // sum previous vertices
                for (int j = 0; j < i; j++)
                {
                    prevVertSum += ma[j].Vertices.Length;
                    prevIndSum += ma[j].Indices.Length;
                }

                foreach (var t in ma[i].Vertices)
                {
                    Vertices[vertAccumulator] = GetInteropVertex(ma[i].Vertices[vertAccumulator - prevVertSum]);
                    vertAccumulator++;
                }

                foreach (var t in ma[i].Indices)
                {
                    if (i == 0)
                    {
                        Indices[indexAccumulator] = ma[i].Indices[indexAccumulator - prevIndSum];
                        indexAccumulator++;
                    }
                    else
                    {
                        Indices[indexAccumulator] = (ushort) (ma[i].Indices[indexAccumulator - prevIndSum] + prevVertSum);
                        indexAccumulator++;
                    }
                }
            }

//            _UnmanagedPtr = Interop.loadMesh(Index,
//                Vertices, Vertices.Length,
//                Indices, Indices.Length);
        }

        public InteropMesh(Mesh m)
        {
            Vertices = new InteropVertex[m.Vertices.Length];
            Indices = new ushort[m.Indices.Length];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = GetInteropVertex(m.Vertices[i]);

            for (int i = 0; i < Indices.Length; i++)
                Indices[i] = m.Indices[i];
            
            NumVertexes = m.Vertices.Length;
            NumIndices = m.Indices.Length;

            BoneListSize = (int) m.Model.Definition.BoneLists[m.Header.BoneListIndex].ActualCount;
            BoneList = new ushort[BoneListSize];

            // copy actual amount to boneList
            for (int i = 0; i < BoneListSize; i++)
                BoneList[i] = m.Model.Definition.BoneLists[m.Header.BoneListIndex].Bones[i];
            
            _UnmanagedPtr = Interop.loadMesh(Index,
                                            Vertices, m.Vertices.Length,
                                            Indices, m.Indices.Length,
                                            BoneList, BoneListSize);
        }

        private InteropVector4 GetInteropVector4(Vector3 v3)
        {
            Vector4 v = new Vector4();
            v.X = v3.X;
            v.Y = v3.Y;
            v.Z = v3.Z;
            v.W = 1;

            return GetInteropVector4(v);
        }

        private InteropVector4 GetInteropVector4(Vector4 v4)
        {
            InteropVector4 iv = new InteropVector4();
            
            iv.X = v4.X;
            iv.Y = v4.Y;
            iv.Z = v4.Z;
            iv.W = v4.W;

            return iv;
        }

        private InteropVector4 GetInteropVector4(Vector4? v4, InteropVector4 dv)
        {
            if (v4.HasValue)
                return GetInteropVector4(v4.Value);
            return dv;
        }

        private InteropVector4 GetInteropVector4(Vector3? v3, InteropVector4 dv) {
            if (v3.HasValue)
                return GetInteropVector4(v3.Value);
            return dv;
        }

        private InteropVertex GetInteropVertex(Vertex v)
        {
            InteropVertex iv = new InteropVertex();

            InteropVector4 dvOne = new InteropVector4();
            dvOne.X = 1;
            dvOne.Y = 1;
            dvOne.Z = 1;
            dvOne.W = 1;

            InteropVector4 dvZero = new InteropVector4();
            dvZero.X = 0;
            dvZero.Y = 0;
            dvZero.W = 0;
            dvZero.W = 0;

            iv.Position = GetInteropVector4(v.Position, dvZero);
            iv.Normal = GetInteropVector4(v.Normal, dvZero);
            iv.UV = GetInteropVector4(v.UV, dvZero);
            iv.BlendWeights = GetInteropVector4(v.BlendWeights, dvZero);
            iv.BlendIndices = v.BlendIndices.GetValueOrDefault(0);
            iv.Color = GetInteropVector4(v.Color, dvOne);
            iv.Tangent1 = GetInteropVector4(v.Tangent1, dvZero);
            iv.Tangent2 = GetInteropVector4(v.Tangent2, dvZero);

            return iv;
        }

        private bool _IsDisposed = false;

        protected virtual void Dispose(bool disposing) {

            if (!_IsDisposed)
            {
                if (_UnmanagedPtr != IntPtr.Zero)
                    Interop.unloadMesh(_UnmanagedPtr);
                _UnmanagedPtr = IntPtr.Zero;

                _IsDisposed = true;
            }
        }

        ~InteropMesh() {
            Dispose(false);
        }
        
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
