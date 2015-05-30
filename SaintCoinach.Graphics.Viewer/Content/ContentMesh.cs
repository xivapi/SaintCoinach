using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    using SharpDX;
    using SharpDX.Direct3D11;

    public class ContentMesh : Drawable3DComponent {
        #region Properties
        public ContentModel Model { get; private set; }
        public ModelVariantIdentifier Variant { get; private set; }
        public PrimitiveMesh Primitive { get; private set; }
        public Matrix Transformation { get; set; }
        public ContentMeshPart[] Parts { get; private set; }
        public MaterialBase Material { get; private set; }
        #endregion

        #region Constructor
        public ContentMesh(ContentModel model, PrimitiveMesh primitive, ModelVariantIdentifier variant)
            : base(model.Engine) {
            this.Model = model;
            this.Primitive = primitive;
            this.Variant = variant;
            this.Transformation = Matrix.Identity;
        }
        #endregion


        #region Content
        public override void LoadContent() {
            Parts = new ContentMeshPart[Primitive.BaseMesh.Parts.Length];
            for (var i = 0; i < Primitive.BaseMesh.Parts.Length; ++i)
                Parts[i] = new ContentMeshPart(this, Primitive.BaseMesh.Parts[i]);

            Material = Engine.MaterialFactory.Get(Primitive.BaseMesh.Material.Get(Variant));

            base.LoadContent();
        }
        public override void UnloadContent() {
            Parts = null;
            Material = null;
            base.UnloadContent();
        }
        #endregion

        #region Draw
        public override void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
            if (Material == null)
                return;
            var transformedWorld = Transformation * world;

            var ia = Engine.Device.ImmediateContext.InputAssembler;

            EffectTechnique tech = Material.CurrentTechnique;

            Material.Effect.Apply(ref world, ref view, ref projection);
            Material.Apply(Model.Parameters);
            
            var skinnedEffect = Material.Effect as Effects.SkinnedEffect;
            if (skinnedEffect != null) {
                var boneList = Model.Definition.BoneLists[Primitive.BaseMesh.Header.BoneListIndex];
                Matrix[] jointMatrix = new Matrix[boneList.ActualCount];
                for (var i = 0; i < boneList.ActualCount; ++i)
                    jointMatrix[i] = Model.JointMatrixArray[boneList.Bones[i]];

                skinnedEffect.JointMatrixArray = jointMatrix;
            }

            ia.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
            ia.SetVertexBuffers(0, Primitive.VertexBufferBinding);
            ia.SetIndexBuffer(Primitive.IndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);

            InputLayout inputLayout = Material.Effect.GetInputLayout(tech.GetPassByIndex(0));
            ia.InputLayout = inputLayout;

            for (var i = 0; i < tech.Description.PassCount; ++i) {
                var pass = tech.GetPassByIndex(i);

                pass.Apply(Engine.Device.ImmediateContext);

                if (Parts.Length == 0) {
                    Engine.Device.ImmediateContext.DrawIndexed(Primitive.BaseMesh.Header.IndexCount, 0, 0);
                } else {
                    foreach (var part in Parts)
                        part.Draw(Engine.Device);
                }
            }
        }
        #endregion
    }
}
