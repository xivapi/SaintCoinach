using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using Viewer;

    public class AnimatedModel : Viewer.Content.ContentModel {
        #region Fields
        private AnimationPlayer _AnimationPlayer;
        private int[] _BoneMap;
        private Matrix[] _InvertedReferencePose;
        private Matrix[] _AnimationPose;
        #endregion

        #region Properties
        public Skeleton Skeleton { get; private set; }
        public bool DisplayReferenceJoints { get; set; }
        public bool DisplayAnimationJoints { get; set; }
        public AnimationPlayer AnimationPlayer { get { return _AnimationPlayer; } }
        #endregion

        #region Constructor
        public AnimatedModel(Engine engine, Skeleton skeleton, ModelVariantIdentifier variant, ModelFile file) : this(engine, skeleton, variant, file.GetModelDefinition(), ModelQuality.High) { }
        public AnimatedModel(Engine engine, Skeleton skeleton, ModelVariantIdentifier variant, ModelFile file, ModelQuality quality) : this(engine, skeleton, variant, file.GetModelDefinition(), quality) { }
        public AnimatedModel(Engine engine, Skeleton skeleton, ModelVariantIdentifier variant, ModelDefinition definition) : this(engine, skeleton, variant, definition, ModelQuality.High) { }
        public AnimatedModel(Engine engine, Skeleton skeleton, ModelVariantIdentifier variant, ModelDefinition definition, ModelQuality quality)
            : base(engine, variant, definition, quality) {

            Skeleton = skeleton;
            _AnimationPlayer = new AnimationPlayer();

            var nameMap = new Dictionary<string, int>();
            for (var i = 0; i < Skeleton.BoneNames.Length; ++i)
                nameMap.Add(Skeleton.BoneNames[i], i);
            _BoneMap = Definition.BoneNames.Select(n => nameMap[n]).ToArray();
            _InvertedReferencePose = Skeleton.ReferencePose.Select(_ => Matrix.Invert(_)).ToArray();
        }
        #endregion

        public override void Update(EngineTime engineTime) {
            if (AnimationPlayer.Animation != null) {
                AnimationPlayer.Update(engineTime);

                _AnimationPose = _AnimationPlayer.GetPose();

                for(var i = 0; i < JointMatrixArray.Length; ++i) {
                    var skeletonBoneIndex = _BoneMap[i];

                    var invRef = _InvertedReferencePose[skeletonBoneIndex];
                    var pose = _AnimationPose[skeletonBoneIndex];

                    JointMatrixArray[i] = invRef * pose;
                }
            } else {
                for (var i = 0; i < JointMatrixArray.Length; ++i)
                    JointMatrixArray[i] = Matrix.Identity;
            }

            base.Update(engineTime);
        }

        public override void Draw(EngineTime time, ref Matrix world, ref Matrix view, ref Matrix projection) {
            base.Draw(time, ref world, ref view, ref projection);

            if (AnimationPlayer.Animation != null) {
                if (DisplayAnimationJoints) {
                    foreach (var i in _BoneMap) {
                        var m = _AnimationPose[i];
                        var w = Matrix.Scaling(0.025f) * m * world;
                        Engine.Cube.Draw(time, ref w, ref view, ref projection);
                    }
                }
            }
            if (DisplayReferenceJoints) {
                foreach (var i in _BoneMap) {
                    var m = Skeleton.ReferencePose[i];
                    var w = Matrix.Scaling(0.025f) * m * world;
                    Engine.Cube.Draw(time, ref w, ref view, ref projection);
                }
            }
        }
    }
}
