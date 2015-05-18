using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    public class EffectFactory : IDisposable {
        #region Compiler include
        class ShaderCompilerInclude : Include {
        #region Fields
        private DirectoryInfo _SourceDirectory;
        private List<Stream> _OpenStreams = new List<Stream>();
        private IDisposable _Shadow;
        #endregion

        #region Constructor
        public ShaderCompilerInclude(string path) {
            _SourceDirectory = new DirectoryInfo(path);
        }
        #endregion

        #region Include Members

        public void Close(Stream stream) {
            stream.Dispose();
            _OpenStreams.Remove(stream);
        }

        public Stream Open(IncludeType type, string fileName, Stream parentStream) {
            var s = File.OpenRead(Path.Combine(_SourceDirectory.FullName, fileName));
            _OpenStreams.Add(s);
            return s;
        }

        #endregion

        #region ICallbackable Members

        IDisposable SharpDX.ICallbackable.Shadow {
            get {
                return _Shadow;
            }
            set {
                _Shadow = value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            foreach (var s in _OpenStreams)
                s.Dispose();
            _OpenStreams.Clear();

            if (_Shadow != null)
                _Shadow.Dispose();
            _Shadow = null;
        }

        #endregion
    }
        #endregion

        #region Fields
        private Engine _Engine;
        private Dictionary<string, CompilationResult> _CompilationResults = new Dictionary<string, CompilationResult>();
        private Dictionary<string, Effects.EffectBase> _Effects = new Dictionary<string, Effects.EffectBase>();
        #endregion

        #region Constructor
        public EffectFactory(Engine engine) {
            _Engine = engine;
        }
        #endregion

        #region Get
        public Effects.EffectBase GetEffect(string name) {
            switch (name) {
                case "character.shpk":
                    return GetEffect(
                        name,
                        "./Effects/HLSL/Character.fx",
                        (bc, ef) => new Effects.CharacterEffect(_Engine.Device, bc, ef));
                case "hair.shpk":
                    return GetEffect(
                        name,
                        "./Effects/HLSL/Hair.fx",
                        (bc, ef) => new Effects.HairEffect(_Engine.Device, bc, ef));
                case "skin.shpk":
                    return GetEffect(
                        name,
                        "./Effects/HLSL/Skin.fx",
                        (bc, ef) => new Effects.SkinEffect(_Engine.Device, bc, ef));
                case "bg.shpk":
                case "bguvscroll.shpk":     // TODO: Actually make it UV-scroll.
                    return GetEffect(
                        name,
                        "./Effects/HLSL/Bg.fx",
                        (bc, ef) => new Effects.BgEffect(_Engine.Device, bc, ef));
                case "iris.shpk":
                    return GetEffect(
                        name,
                        "./Effects/HLSL/Iris.fx",
                        (bc, ef) => new Effects.IrisEffect(_Engine.Device, bc, ef));
                case "bgcolorchange.shpk":
                    return GetEffect(
                        name,
                        "./Effects/HLSL/BgColorChange.fx",
                        (bc, ef) => new Effects.BgColorChangeEffect(_Engine.Device, bc, ef));
                case "crystal.shpk":
                    return GetEffect(
                        name,
                        "./Effects/HLSL/Crystal.fx",
                        (bc, ef) => new Effects.CrystalEffect(_Engine.Device, bc, ef));
                default:
                    throw new NotSupportedException();
            }
        }
        private Effects.EffectBase GetEffect(string key, string file, Func<CompilationResult, EffectFlags, Effects.EffectBase> factory) {
            Effects.EffectBase effect;
            if (_Effects.TryGetValue(key, out effect))
                return effect;

            var cr = GetCompilationResult(key, file);
            effect = factory(cr, EffectFlags.None);
            _Effects.Add(key, effect);
            return effect;
        }
        private CompilationResult GetCompilationResult(string key, string file) {
            CompilationResult result;
            if (_CompilationResults.TryGetValue(key, out result))
                return result;
            result = ShaderBytecode.CompileFromFile(
                        file,
                        "fx_5_0",
                        ShaderFlags.None,
                        EffectFlags.None,
                        new SharpDX.Direct3D.ShaderMacro[] { new SharpDX.Direct3D.ShaderMacro("SM4", "SM4") },
                        new ShaderCompilerInclude(System.IO.Path.Combine(".", "Effects", "HLSL")));
            _CompilationResults.Add(key, result);

            return result;
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            UnloadAll();
        }

        public void UnloadAll() {
            foreach (var v in _CompilationResults.Values)
                v.Dispose();
            foreach (var v in _Effects.Values)
                v.Dispose();
            _CompilationResults.Clear();
            _Effects.Clear();
        }
        #endregion
    }
}
