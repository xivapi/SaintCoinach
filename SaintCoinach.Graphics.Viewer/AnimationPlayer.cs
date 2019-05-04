using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using Viewer;

    public class AnimationPlayer : IUpdateableComponent {
        #region Fields
        private double _PlaybackSpeed = 1f;

        private bool _IsAnimating;
        private bool _IsEnabled = true;
        private bool _IsLooping = true;

        private Animation _Animation;

        private double _CurrentPlaybackPosition = 0f;

        private bool _IsDirty = true;
        private Matrix[] _BoneTransformationMatrices;
        #endregion

        #region Properties
        public double PlaybackSpeed { get { return _PlaybackSpeed; } set { _PlaybackSpeed = value; } }
        public bool IsLooping { get { return _IsLooping; } set { _IsLooping = value; _IsAnimating = true; } }
        public Animation Animation {
            get { return _Animation; }
            set {
                _Animation = value;
                Reset();
            }
        }
        public bool IsAnimating { get { return _IsAnimating; } }
        public double CurrentPlaybackPosition { get { return _CurrentPlaybackPosition; } set { _CurrentPlaybackPosition = value; _IsAnimating = true; _IsDirty = true; } }
        #endregion

        #region Constructor
        #endregion

        #region Update
        public Matrix[] GetPose() {
            if (Animation == null)
                return new Matrix[0];

            if (_IsDirty) {
                _BoneTransformationMatrices = Animation.GetPose((float)CurrentPlaybackPosition);
                _IsDirty = false;
            }

            return _BoneTransformationMatrices;
        }
        public void Reset() {
            _IsAnimating = true;
            CurrentPlaybackPosition = 0f;
        }

        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime engineTime) {
            if (!IsEnabled || !IsAnimating || Animation == null || Math.Abs(PlaybackSpeed) <= float.Epsilon)
                return;

            _IsDirty = true;
            _CurrentPlaybackPosition += engineTime.ElapsedTime.TotalSeconds * PlaybackSpeed;
            if (IsLooping) {
                while (_CurrentPlaybackPosition < 0)
                    _CurrentPlaybackPosition += Animation.Duration;
                while (_CurrentPlaybackPosition > Animation.Duration)
                    _CurrentPlaybackPosition -= Animation.Duration;
            } else if (_CurrentPlaybackPosition <= 0 || _CurrentPlaybackPosition > Animation.Duration)
                _IsAnimating = false;
        }
        #endregion
    }
}
