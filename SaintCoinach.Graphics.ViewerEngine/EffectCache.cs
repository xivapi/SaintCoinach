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
        private Effects.SkinEffect _SkinEffect;
        private Effects.HairEffect _HairEffect;
        #endregion

        #region Properties
        public Device Device { get { return _Device; } }

        public Effects.BgEffect BgEffect { get { return _BgEffect; } }
        public Effects.CharacterEffect CharacterEffect { get { return _CharacterEffect; } }
        public Effects.SkinEffect SkinEffect { get { return _SkinEffect; } }
        public Effects.HairEffect HairEffect { get { return _HairEffect; } }
        #endregion

        #region Apply
        public void ApplyAll(ref Matrix world, ref Matrix view, ref Matrix proj) {
            CharacterEffect.Apply(ref world, ref view, ref proj);
            BgEffect.Apply(ref world, ref view, ref proj);
            SkinEffect.Apply(ref world, ref view, ref proj);
            HairEffect.Apply(ref world, ref view, ref proj);
        }
        #endregion

        #region IContentComponent Members

        public bool IsLoaded {
            get { return Device != null; }
        }

        public void Load(ViewerEngine engine) {
            var device = engine.Device;
            _CharacterEffect = new Effects.CharacterEffect(device);
            _BgEffect = new Effects.BgEffect(device);
            _SkinEffect = new Effects.SkinEffect(device);
            _HairEffect = new Effects.HairEffect(device);

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

            if (_SkinEffect != null)
                _SkinEffect.Dispose();
            _SkinEffect = null;

            if (_HairEffect != null)
                _HairEffect.Dispose();
            _HairEffect = null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Unload();
        }

        #endregion
    }
}
