using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Viewer.Effects {
    public class CharacterEffect : Effect {
        #region Static
        private static CompilationResult _EffectByteCode;

        private static CompilationResult EffectByteCode {
            get {
                if (_EffectByteCode == null)
                    _EffectByteCode = ShaderBytecode.CompileFromFile(
                        "Effects/HLSL/Character.fx", 
                        "fx_5_0",
                        ShaderFlags.None,
                        EffectFlags.None,
                        new SharpDX.Direct3D.ShaderMacro[] { new SharpDX.Direct3D.ShaderMacro("SM4", "SM4") },
                        new ShaderCompilerInclude(@"Effects/HLSL"));
                return _EffectByteCode;
            }
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public CharacterEffect(Device device) : this(device, EffectFlags.None) { }
        public CharacterEffect(Device device, EffectFlags flags) : base(device, EffectByteCode, flags) {
            Init();
        }
        #endregion

        #region Init
        private void Init() {
            
        }
        #endregion
    }
}
