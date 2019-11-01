using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace SaintCoinach.Graphics.Viewer.Content {
    public class ContentSgb : Drawable3DComponent {
        #region Fields
        private ComponentContainer _Content = new ComponentContainer();
        #endregion

        #region Properties
        public Sgb.SgbFile SgbFile{ get; private set; }
        public Data.ParametersBase Parameters { get; set; }
        public Matrix Transformation { get; set; }
        #endregion

        #region Constructor
        public ContentSgb(Engine engine, Sgb.SgbFile sgbFile, Data.ParametersBase parameters) :this(engine, sgbFile, parameters, Matrix.Identity) {  }
        public ContentSgb(Engine engine, Sgb.SgbFile sgbFile):this(engine, sgbFile, null, Matrix.Identity) { }
        public ContentSgb(Engine engine, Sgb.SgbFile sgbFile, Data.ParametersBase parameters, Matrix EObjTransform) : base(engine) {
            this.SgbFile = sgbFile;
            this.Parameters = parameters;
            this.Transformation = Matrix.Identity;

            // todo: fixme!

            bool LoadModels(Sgb.SgbFile file, Matrix rootTransform, Matrix gimTransform) {
                if (file == null)
                    return false;

                foreach (var group in file.Data.OfType<Sgb.SgbGroup>()) {
                    foreach (var mdl in group.Entries.OfType<Sgb.SgbModelEntry>()) {
                        var content = new ContentModel(engine, mdl.Model) { Parameters = parameters };

                        if (EObjTransform == Matrix.Identity)
                            this.Transformation = Matrix.Identity;
                        content.Transformation = content.Transformation * gimTransform * rootTransform * EObjTransform;
                        
                        _Content.Add(content);
                    }
                }
                return true;
            }

            Matrix CreateMatrix(Vector3 translation, Vector3 rotation, Vector3 scale) {
                return (Matrix.Scaling(scale.ToDx())
                    * Matrix.RotationX(rotation.X)
                    * Matrix.RotationY(rotation.Y)
                    * Matrix.RotationZ(rotation.Z)
                    * Matrix.Translation(translation.ToDx()));
            }
            void LoadSgbFile(Sgb.SgbFile file) {
                if (LoadModels(file, Matrix.Identity, Matrix.Identity)) {
                    foreach (var rootGimGroup in file.Data.OfType<Sgb.SgbGroup>()) {
                        foreach (var rootGimEntry in rootGimGroup.Entries.OfType<Sgb.SgbGimmickEntry>()) {
                            var rootGimTransform = CreateMatrix(rootGimEntry.Header.Translation, rootGimEntry.Header.Rotation, rootGimEntry.Header.Scale);
                            if (LoadModels(rootGimEntry.Gimmick, rootGimTransform, Matrix.Identity)) {
                                foreach (var subGimGroup in rootGimEntry.Gimmick.Data.OfType<Sgb.SgbGroup>()) {
                                    foreach (var subGim in subGimGroup.Entries.OfType<Sgb.SgbGimmickEntry>()) {
                                        var subGimTransform = CreateMatrix(subGim.Header.Translation, subGim.Header.Rotation, subGim.Header.Scale);
                                        LoadModels(subGim.Gimmick, rootGimTransform, subGimTransform);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            LoadSgbFile(sgbFile);
        }
        #endregion
        
        public override void LoadContent() {
            _Content.LoadContent();
            base.LoadContent();
        }
        public override void UnloadContent() {
            _Content.UnloadContent();
            base.UnloadContent();
        }
        public override void Update(EngineTime engineTime) {
            _Content.Update(engineTime);
            base.Update(engineTime);
        }
        public override void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
            var adjustedWorld = Transformation * world;
            _Content.Draw(time, ref adjustedWorld, ref view, ref projection);
        }
    }
}
