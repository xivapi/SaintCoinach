using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics {
    public sealed class TextureCache : IContentComponent, IDisposable {
        #region Static
        private static Dictionary<Device, TextureCache> _DeviceCaches = new Dictionary<Device, TextureCache>();

        public static TextureCache Get(Device device) {
            TextureCache c;
            if (_DeviceCaches.TryGetValue(device, out c))
                return c;
            return null;
        }

        private static void Register(Device device, TextureCache c) {
            _DeviceCaches.Add(device, c);
        }
        private static void Unregister(Device device) {
            _DeviceCaches.Remove(device);
        }
        #endregion

        #region Fields
        private Device _Device;
        private Dictionary<string, Texture2D> _Textures = new Dictionary<string, Texture2D>();
        private Dictionary<string, ShaderResourceView> _TextureResourceViews = new Dictionary<string, ShaderResourceView>();
        #endregion

        #region Properties
        public Device Device { get { return _Device; } }
        #endregion

        #region Get
        public unsafe ShaderResourceView GetResource(Imaging.ImageFile file) {
            var key = file.Path;
            ShaderResourceView view;
            if (_TextureResourceViews.TryGetValue(key, out view))
                return view;

            view = new ShaderResourceView(Device, GetTexture(file));
            return view;
        }
        public unsafe Texture2D GetTexture(Imaging.ImageFile file) {
            var key = file.Path;
            Texture2D tex;
            if (_Textures.TryGetValue(key, out tex))
                return tex;

            byte[] buffer;
            var desc = new Texture2DDescription {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Height = file.Height,
                Width = file.Width,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = ResourceUsage.Immutable
            };
            if (file.Format == Imaging.ImageFormat.A16R16G16B16_Float) {
                buffer = file.GetData();
                desc.Format = SharpDX.DXGI.Format.R16G16B16A16_Float;
            } else {
                buffer = Imaging.ImageConverter.GetA8R8G8B8(file);
                for (var i = 0; i < buffer.Length; i += 4) {
                    var r = buffer[i + 0];
                    var b = buffer[i + 2];

                    buffer[i + 0] = b;
                    buffer[i + 2] = r;
                }
                desc.Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm;
            }

            fixed (byte* p = buffer) {
                var ptr = (IntPtr)p;
                var pitch = SharpDX.DXGI.FormatHelper.SizeOfInBytes(desc.Format) * file.Width;
                var dataRect = new DataRectangle(ptr, pitch);
                tex = new Texture2D(Device, desc, dataRect);
            }
            _Textures.Add(key, tex);
            return tex;
        }
        #endregion

        #region IContentComponent Members

        public bool IsLoaded {
            get { return Device != null; }
        }

        public void Load(Device device) {
            _Device = device;
            Register(Device, this);
        }

        public void Unload() {
            Unregister(Device);
            _Device = null;

            foreach (var tex in _Textures.Values)
                tex.Dispose();
            _Textures.Clear();
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Unload();
        }

        #endregion
    }
}
