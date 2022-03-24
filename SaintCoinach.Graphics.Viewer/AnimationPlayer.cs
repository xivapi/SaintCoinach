using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using Viewer;

    public class AnimationPlayer : IUpdateableComponent {
        #region Fields
        private double _PlaybackSpeed = 1f;

        private bool _IsAnimating;
        private bool _IsEnabled = true;
        private bool _IsLooping = true;

        private List<Animation> _Animations = new List<Animation>();
        private int _AnimationIndex = -1;

        private double _CurrentPlaybackPosition = 0f;

        private bool _IsDirty = true;
        private Matrix[] _BoneTransformationMatrices;

        private Engine _Engine;
        #endregion

        #region Properties
        public double PlaybackSpeed { get { return _PlaybackSpeed; } set { _PlaybackSpeed = value; } }
        public bool IsLooping { get { return _IsLooping; } set { _IsLooping = value; _IsAnimating = true; } }
        public IList<Animation> Animations { get { return _Animations.AsReadOnly(); } }
        public Animation Animation { get { return _AnimationIndex == -1 ? null : _Animations[_AnimationIndex]; } }
        public int AnimationIndex {
            get { return _AnimationIndex; }
            set {
                _AnimationIndex = value;
                Reset();
            }
        }
        public bool IsAnimating { get { return _IsAnimating; } }
        public double CurrentPlaybackPosition { get { return _CurrentPlaybackPosition; } set { _CurrentPlaybackPosition = value; _IsAnimating = true; _IsDirty = true; } }
        #endregion

        #region Constructor
        public AnimationPlayer(Engine engine) {
            _Engine = engine;

            Reset();
        }
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
            PlaybackSpeed = 1;
            UpdateTitle();
        }

        public void UpdateTitle() {
            if (Animation == null) {
                _Engine.Subtitle = "";
                return;
            }

            var sb = new StringBuilder();
            sb.Append(_AnimationIndex + 1).Append("/").Append(_Animations.Count);
            sb.Append(" - ").Append(Animation.Name);
            sb.Append(" - ").AppendFormat("{0:0.00}x", PlaybackSpeed);
            sb.Append(" - ").AppendFormat("{0:0.00}s/{1:0.00}s", CurrentPlaybackPosition, Animation.Duration);
            sb.Append(" - ").AppendFormat("{0}/{1}", (int)Math.Floor(CurrentPlaybackPosition / Animation.FrameDuration) + 1, Animation.FrameCount);
            if (!_IsAnimating)
                sb.Append(" - Paused");
            _Engine.Subtitle = sb.ToString();
        }

        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime engineTime) {
            if (!Animations.Any())
                return;

            var elapsedTime = IsAnimating ? engineTime.ElapsedTime.TotalSeconds : 0;
            if (_Engine.Keyboard.IsKeyDown(Keys.R))
                Reset();
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.Oemcomma)) {
                _AnimationIndex = (_AnimationIndex + _Animations.Count - 1) % _Animations.Count;
                elapsedTime = 0;
                _IsAnimating = true;
                CurrentPlaybackPosition = 0f;
            }
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.OemPeriod)) {
                _AnimationIndex = (_AnimationIndex + 1) % _Animations.Count;
                elapsedTime = 0;
                _IsAnimating = true;
                CurrentPlaybackPosition = 0f;
            }
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.OemQuestion)) {
                _IsAnimating = !_IsAnimating;
                elapsedTime = 0;
            }
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.N)) {
                _IsAnimating = false;
                elapsedTime = 0;
                _CurrentPlaybackPosition -= Animation.FrameDuration;
            }
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.M)) {
                _IsAnimating = false;
                elapsedTime = 0;
                _CurrentPlaybackPosition += Animation.FrameDuration;
            }
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.K))
                PlaybackSpeed = Math.Max(1, Math.Round(PlaybackSpeed * 20) - 1) / 20;
            if (_Engine.Keyboard.WasKeyPressedRepeatable(Keys.L))
                PlaybackSpeed = Math.Min(80, Math.Round(PlaybackSpeed * 20) + 1) / 20;
            UpdateTitle();

            _IsDirty = true;
            _CurrentPlaybackPosition += elapsedTime * PlaybackSpeed;
            if (IsLooping) {
                while (_CurrentPlaybackPosition < 0)
                    _CurrentPlaybackPosition += Animation.Duration;
                while (_CurrentPlaybackPosition > Animation.Duration)
                    _CurrentPlaybackPosition -= Animation.Duration;
            } else if (_CurrentPlaybackPosition <= 0 || _CurrentPlaybackPosition > Animation.Duration)
                _IsAnimating = false;
        }
        #endregion

        #region Misc
        public void AddAnimation(Animation animation) {
            _Animations.Add(animation);
        }

        public void SortAnimationsByName() {
            var prev = Animation;
            _Animations.Sort((x, y) => (x.Name.CompareTo(y.Name)));
            AnimationIndex = Animations.IndexOf(prev);
        }
        #endregion
    }
}
