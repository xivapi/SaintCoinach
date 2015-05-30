using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;

    public class ContentModel : Drawable3DComponent {
        #region Fields
        private ComponentContainer _MeshContainer = new ComponentContainer();
        #endregion

        #region Properties
        public ModelDefinition Definition { get; private set; }
        public ModelVariantIdentifier Variant { get; private set; }
        public ModelQuality Quality { get; private set; }
        public ContentMesh[] Meshes { get; private set; }
        public Matrix Transformation { get; set; }
        public PrimitiveModel Primitive { get; private set; }
        public Data.ParametersBase Parameters { get; set; }
        public Matrix[] JointMatrixArray { get; private set; }
        #endregion

        #region Constructor
        public ContentModel(Engine engine, ModelVariantIdentifier variant, ModelFile file) : this(engine, variant, file.GetModelDefinition(), ModelQuality.High) { }
        public ContentModel(Engine engine, ModelVariantIdentifier variant, ModelFile file, ModelQuality quality) : this(engine, variant, file.GetModelDefinition(), quality) { }
        public ContentModel(Engine engine, ModelVariantIdentifier variant, ModelDefinition definition) : this(engine, variant, definition, ModelQuality.High) { }
        public ContentModel(Engine engine, ModelVariantIdentifier variant, ModelDefinition definition, ModelQuality quality)
            : base(engine) {
            this.Definition = definition;
            this.Quality = quality;
            this.Variant = variant;
            this.Transformation = Matrix.Identity;

            Init();
        }
        public ContentModel(Engine engine, TransformedModel transformedModel) : this(engine, transformedModel, ModelQuality.High) { }
        public ContentModel(Engine engine, TransformedModel transformedModel, ModelQuality quality) : base(engine) {
            this.Parameters = new Data.ParametersBase();
            this.Definition = transformedModel.Model;
            this.Quality = Quality;
            this.Variant = new ModelVariantIdentifier {
                ImcVariant = ImcVariant.Default,
                StainKey = null
            };
            this.Transformation =
                Matrix.Scaling(transformedModel.Scale.ToDx())
                * Matrix.RotationX(transformedModel.Rotation.X)
                * Matrix.RotationY(transformedModel.Rotation.Y)
                * Matrix.RotationZ(transformedModel.Rotation.Z)
                * Matrix.Translation(transformedModel.Translation.ToDx());

            Init();
        }

        private void Init() {
            JointMatrixArray = new Matrix[Definition.Bones.Length];
            for(var i = 0; i < JointMatrixArray.Length; ++i) {
                var b = Definition.Bones[i];
                JointMatrixArray[i] = Matrix.Identity;
            }
        }
        #endregion

        #region Content
        public override void LoadContent() {
            try {
                Primitive = Engine.ModelFactory.Get(Definition, Quality);
                Meshes = new ContentMesh[Primitive.Meshes.Length];
                for (var i = 0; i < Primitive.Meshes.Length; ++i) {
                    Meshes[i] = new ContentMesh(this, Primitive.Meshes[i], Variant);
                    _MeshContainer.Add(Meshes[i]);
                }

                _MeshContainer.LoadContent();
                base.LoadContent();
            }catch(Exception e) {
                System.Diagnostics.Trace.WriteLine(string.Format("Failed to load model '{0}': {1}", Definition.File.Path, e));
                IsLoaded = false;
            }
        }
        public override void UnloadContent() {
            base.UnloadContent();
            _MeshContainer.UnloadContent();
            _MeshContainer.Clear();
            Meshes = null;
        }
        #endregion

        #region Draw
        public override void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
            if (!IsLoaded)
                return;
            var transformedWorld = Transformation * world;
            _MeshContainer.Draw(time, ref transformedWorld, ref view, ref projection);
        }
        #endregion

        #region Update
        public override void Update(EngineTime engineTime) {
            _MeshContainer.Update(engineTime);
            base.Update(engineTime);
        }
        #endregion
    }
}
