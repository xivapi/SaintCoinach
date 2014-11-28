using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics {
    public sealed class EffectCache : IContentComponent, IDisposable {
        #region Static
        private static Dictionary<Device, EffectCache> _DeviceCaches = new Dictionary<Device, EffectCache>();

        public static EffectCache Get(Device device) {
            EffectCache c;
            if (_DeviceCaches.TryGetValue(device, out c))
                return c;
            return null;
        }

        private static void Register(Device device, EffectCache c) {
            _DeviceCaches.Add(device, c);
        }
        private static void Unregister(Device device) {
            _DeviceCaches.Remove(device);
        }
        #endregion

        #region Fields
        private Device _Device;
        private Effects.CharacterEffect _CharacterEffect;
        private Effects.BgEffect _BgEffect;
        #endregion

        #region Properties
        public Device Device { get { return _Device; } }

        public Effects.BgEffect BgEffect { get { return _BgEffect; } }
        public Effects.CharacterEffect CharacterEffect { get { return _CharacterEffect; } }
        #endregion

        #region Apply
        public void ApplyAll(ref Matrix world, ref Matrix view, ref Matrix proj) {
            CharacterEffect.Apply(ref world, ref view, ref proj);
            _BgEffect.Apply(ref world, ref view, ref proj);
        }
        #endregion

        #region IContentComponent Members

        public bool IsLoaded {
            get { return Device != null; }
        }

        public void Load(Device device) {
            _CharacterEffect = new Effects.CharacterEffect(device);
            _BgEffect = new Effects.BgEffect(device);

            _Device = device;
            Register(device, this);
        }

        public void Unload() {
            Unregister(Device);
            _Device = null;

            if (_CharacterEffect != null)
                _CharacterEffect.Dispose();
            _CharacterEffect = null;

            if (_BgEffect != null)
                _BgEffect.Dispose();
            _BgEffect = null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Unload();
        }

        #endregion
    }
}
