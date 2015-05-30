using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Effects {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public abstract class SkinnedEffect : EffectBase {
        public const int JointMatrixArrayMaxSize = 64;

        #region Fields
        private EffectMatrixVariable _JointMatrixArrayVar;
        #endregion

        #region Properties
        public Matrix[] JointMatrixArray {
            set {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length > JointMatrixArrayMaxSize)
                    throw new ArgumentException();
                _JointMatrixArrayVar.SetMatrix(value);
            }
        }
        #endregion


        #region Constructor
        protected SkinnedEffect(Device device, byte[] byteCode) : this(device, byteCode, EffectFlags.None) { }
        protected SkinnedEffect(Device device, byte[] byteCode, EffectFlags flags)
            : base(device, byteCode, flags) {
            _JointMatrixArrayVar = GetVariableByName("g_JointMatrixArray").AsMatrix();
        }
        #endregion
    }
}
