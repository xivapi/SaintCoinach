using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics.Parts {
    public class Mesh : IDrawable, IUpdateable, IContentComponent {
        [Flags]
        enum DirtyFlags {
            None,
            Material,
            Primitive,
            All = Material | Primitive,
        }

        #region Fields
        private DirtyFlags _DirtyFlags = DirtyFlags.None;

        private Assets.Mesh _SourceMesh;
        private Assets.Material _SourceMaterial;
        private Assets.MaterialVersion _SourceMaterialVersion;

        private Type _VertexType;
        private object _Vertices;
        private ushort[] _Indices;

        private Matrix _Transformation;
        private XivMaterial _Material;
        private IGeometricPrimitive _Primitive;

        private SharpDX.Direct3D11.Device _Device;
        private bool _IsLoaded = false;

        private object _Lock = new object();
        #endregion

        #region Properties
        public int VertexCount { get { return _SourceMesh.Header.VertexCount; } }
        public int IndexCount { get { return _SourceMesh.Header.IndexCount; } }
        public IEnumerable<int> AvailableMaterialVersions { get { return _SourceMaterial.AvailableVersions; } }
        public int MaterialVersion {
            get { return _SourceMaterialVersion.Version; }
            set {
                lock (_Lock) {
                    _SourceMaterialVersion = _SourceMaterial.GetVersion(value);
                    _DirtyFlags |= DirtyFlags.Material;
                }
            }
        }
        public IEnumerable<int> AvailableStains { get { return _SourceMaterialVersion.AvailableStains; } }
        public int MaterialStain {
            get { return _SourceMaterialVersion.CurrentStain; }
            set {
                lock (_Lock) {
                    _SourceMaterialVersion.CurrentStain = value;
                    _DirtyFlags |= DirtyFlags.Material;
                }
            }
        }
        public bool CanStain { get { return _SourceMaterialVersion.CanStain; } }
        #endregion

        #region Constructor
        public Mesh(Assets.Mesh sourceMesh) : this(sourceMesh, Matrix.Identity) { }
        public Mesh(Assets.Mesh sourceMesh, Matrix transform) {
            _SourceMesh = sourceMesh;
            _SourceMaterial = sourceMesh.Material;
            MaterialVersion = _SourceMaterial.AvailableVersions.First();
            _Transformation = transform;

            PreProcess();
        }
        #endregion

        #region PreProcess
        private void PreProcess() {
            _Indices = _SourceMesh.GetIndices();

            // XXX: Color too?
            if (_SourceMesh.VertexType == typeof(Assets.VertexBase) || _SourceMesh.VertexType == typeof(Assets.VertexColor)) {
                _VertexType = typeof(Primitives.VertexCommon);
                _Vertices = Utilities.Vertex.Convert(_SourceMesh.GetVertices(), _Indices);
            } else if (_SourceMesh.VertexType == typeof(Assets.VertexBlend) || _SourceMesh.VertexType == typeof(Assets.VertexColorBlend)) {
                _VertexType = typeof(Primitives.VertexDualTexture);
                _Vertices = Utilities.Vertex.Convert(_SourceMesh.GetVertices<Assets.VertexBlend>(), _Indices);
            } else
                throw new NotSupportedException();
        }
        #endregion

        #region IDrawable Members
        public void Draw(SharpDX.Direct3D11.Device device, EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection) {
            var transformedWorld = _Transformation * world;

            var ia = device.ImmediateContext.InputAssembler;

            var tech = _Material.CurrentTechnique;

            _Material.Effect.Apply(ref transformedWorld, ref view, ref projection);
            _Material.Apply();

            ia.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
            ia.SetVertexBuffers(0, _Primitive.VertexBinding);
            ia.SetIndexBuffer(_Primitive.IndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            var inputLayout = _Material.Effect.GetInputLayout(tech.GetPassByIndex(0), _Primitive.VertexType);

            ia.InputLayout = inputLayout;

            for (var i = 0; i < tech.Description.PassCount; ++i) {
                var pass = tech.GetPassByIndex(i);

                pass.Apply(device.ImmediateContext);
                device.ImmediateContext.DrawIndexed(IndexCount, 0, 0);
            }
        }

        #endregion

        #region IContentComponent Members

        public bool IsLoaded {
            get { return _IsLoaded; }
        }

        private void CheckDirty() {
            if (_DirtyFlags == DirtyFlags.None)
                return;

            if (_DirtyFlags.HasFlag(DirtyFlags.Primitive)) {
                if (_Primitive != null)
                    _Primitive.Dispose();
                _Primitive = null;

                if (_VertexType == typeof(Primitives.VertexCommon))
                    _Primitive = new Primitives.GeometricPrimitive<Primitives.VertexCommon>(_Device, (Primitives.VertexCommon[])_Vertices, _Indices);
                else if (_VertexType == typeof(Primitives.VertexDualTexture))
                    _Primitive = new Primitives.GeometricPrimitive<Primitives.VertexDualTexture>(_Device, (Primitives.VertexDualTexture[])_Vertices, _Indices);
                else
                    throw new NotSupportedException();
            }

            if (_DirtyFlags.HasFlag(DirtyFlags.Material)) {
                lock (_Lock) {
                    if (_Material != null)
                        _Material.Dispose();
                    _Material = null;

                    _Material = XivMaterial.Create(_Device, _SourceMaterialVersion);
                }
            }

            _DirtyFlags = DirtyFlags.None;
        }

        public void Load(SharpDX.Direct3D11.Device device) {
            _Device = device;

            _DirtyFlags = DirtyFlags.All;
            CheckDirty();
            _IsLoaded = true;
        }

        public void Unload() {
            if (_Material != null)
                _Material.Dispose();
            _Material = null;

            if (_Primitive != null)
                _Primitive.Dispose();
            _Primitive = null;

            _Device = null;
            _IsLoaded = false;
            _DirtyFlags = DirtyFlags.All;
        }

        #endregion

        #region IUpdateable Members

        public void Update(EngineTime time) {
            CheckDirty();
        }

        #endregion
    }
}
