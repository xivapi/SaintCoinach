using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Animation {
    using SharpDX;
    using Viewer;

    public class AnimatedModel : Viewer.Content.ContentModel {
        #region Fields
        private AnimationContainer _CurrentAnimationContainer;
        private AnimationPlayer _AnimationPlayer;
        private int[] _BoneMap;
        private Matrix[] _InvertedReferencePose;
        private Matrix[] _AnimationPose;
        #endregion

        #region Properties
        public bool DisplayReferenceJoints { get; set; }
        public bool DisplayAnimationJoints { get; set; }
        public double PlaybackSpeed { get { return _AnimationPlayer.PlaybackSpeed; } set { _AnimationPlayer.PlaybackSpeed = value; } }
        public bool IsLooping { get { return _AnimationPlayer.IsLooping; } set { _AnimationPlayer.IsLooping = value; } }
        public Animation CurrentAnimation {
            get { return _AnimationPlayer.Animation; }
            set {
                _AnimationPlayer.Animation = value;
                SetUpForAnimationContainer(value == null ? null : value.Container);
            }
        }
        #endregion

        #region Constructor
        public AnimatedModel(Engine engine, ModelVariantIdentifier variant, ModelFile file) : this(engine, variant, file.GetModelDefinition(), ModelQuality.High) { }
        public AnimatedModel(Engine engine, ModelVariantIdentifier variant, ModelFile file, ModelQuality quality) : this(engine, variant, file.GetModelDefinition(), quality) { }
        public AnimatedModel(Engine engine, ModelVariantIdentifier variant, ModelDefinition definition) : this(engine, variant, definition, ModelQuality.High) { }
        public AnimatedModel(Engine engine, ModelVariantIdentifier variant, ModelDefinition definition, ModelQuality quality)
            : base(engine, variant, definition, quality) {

            _AnimationPlayer = new AnimationPlayer();
        }
        #endregion

        #region Set up
        private void SetUpForAnimationContainer(AnimationContainer container) {
            _CurrentAnimationContainer = container;
            if (container == null)
                return;

            var nameMap = new Dictionary<string, int>();
            for (var i = 0; i < container.SkeletonBoneNames.Length; ++i)
                nameMap.Add(container.SkeletonBoneNames[i], i);
            _BoneMap = Definition.BoneNames.Select(n => nameMap[n]).ToArray();
            _InvertedReferencePose = container.ReferencePose.Select(_ => Matrix.Invert(_)).ToArray();
        }
        #endregion

        public override void Update(EngineTime engineTime) {
            if (_CurrentAnimationContainer != null) {
                _AnimationPlayer.Update(engineTime);

                _AnimationPose = _AnimationPlayer.GetBoneTransformationMatrices();

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

            if (_CurrentAnimationContainer != null) {
                if (DisplayAnimationJoints) {
                    foreach (var i in _BoneMap) {
                        var m = _AnimationPose[i];
                        var w = Matrix.Scaling(0.025f) * m * world;
                        Engine.Cube.Draw(time, ref w, ref view, ref projection);
                    }
                }
                if (DisplayReferenceJoints) {
                    foreach (var i in _BoneMap) {
                        var m = _CurrentAnimationContainer.ReferencePose[i];
                        var w = Matrix.Scaling(0.025f) * m * world;
                        Engine.Cube.Draw(time, ref w, ref view, ref projection);
                    }
                }
            }
        }
    }
}
