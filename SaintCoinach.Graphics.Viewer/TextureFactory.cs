using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class TextureFactory : IDisposable {
        #region Fields
        private Engine _Engine;
        private Dictionary<string, ShaderResourceView> _Resources = new Dictionary<string, ShaderResourceView>();
        private Dictionary<string, Texture2D> _Textures = new Dictionary<string, Texture2D>();
        #endregion

        #region Constructor
        public TextureFactory(Engine engine) {
            _Engine = engine;
        }
        #endregion

        #region Get
        public ShaderResourceView GetResource(Imaging.ImageFile source) {
            var key = source.Path;

            ShaderResourceView res;
            if (_Resources.TryGetValue(key, out res))
                return res;

            res = new ShaderResourceView(_Engine.Device, GetTexture(source));
            _Resources.Add(key, res);
            return res;
        }
        public unsafe Texture2D GetTexture(Imaging.ImageFile source) {
            var key = source.Path;

            Texture2D tex;
            if (_Textures.TryGetValue(key, out tex))
                return tex;

            byte[] buffer;
            var desc = new Texture2DDescription {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Height = source.Height,
                Width = source.Width,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = ResourceUsage.Immutable
            };
            if (source.Format == Imaging.ImageFormat.A16R16G16B16Float) {
                buffer = source.GetData();
                desc.Format = SharpDX.DXGI.Format.R16G16B16A16_Float;
            } else {
                buffer = Imaging.ImageConverter.GetA8R8G8B8(source);
                for (var i = 0; i < buffer.Length; i += 4) {
                    var r = buffer[i + 0];
                    var b = buffer[i + 2];

                    buffer[i + 0] = b;
                    buffer[i + 2] = r;
                }
                desc.Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm;
                if (source.Format == Imaging.ImageFormat.A8R8G8B8_Cube)
                    desc.ArraySize = 6;
            }

            fixed (byte* p = buffer) {
                var ptr = (IntPtr)p;
                var pitch = SharpDX.DXGI.FormatHelper.SizeOfInBytes(desc.Format) * source.Width;
                var dataRects = new DataRectangle[desc.ArraySize];
                for (var i = 0; i < desc.ArraySize; ++i)
                    dataRects[i] = new DataRectangle(ptr + i * pitch * source.Height, pitch);
                tex = new Texture2D(_Engine.Device, desc, dataRects);
            }

            _Textures.Add(key, tex);
            return tex;
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            UnloadAll();
        }

        public void UnloadAll() {
            foreach (var v in _Resources.Values)
                v.Dispose();
            foreach (var v in _Textures.Values)
                v.Dispose();
            _Resources.Clear();
            _Textures.Clear();
        }
        #endregion
    }
}
