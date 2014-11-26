using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.D3DCompiler;

namespace SaintCoinach.Graphics.Viewer.Effects {
    public class CharacterEffect {
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
    }
}
