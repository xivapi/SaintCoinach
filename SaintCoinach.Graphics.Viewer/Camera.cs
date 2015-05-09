using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Keys = System.Windows.Forms.Keys;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;

    public class Camera : IUpdateableComponent {
        #region Fields
        private Vector3 _CameraPosition = Vector3.UnitX;
        private Vector3 _Up = Vector3.Up;

        private float _Yaw = 0;
        private float _Pitch = 0;
        private Matrix _Projection;
        private Matrix _View;

        const float RotationSpeed = (float)(Math.PI / 2f);
        const float MoveSpeed = 20.0f;

        private Engine _Engine;
        #endregion

        #region Properties
        public Matrix Projection { get { return _Projection; } }
        public Matrix View { get { return _View; } }
        public Vector3 CameraPosition { get { return _CameraPosition; } }
        #endregion

        #region Constructor
        public Camera(Engine engine) {
            _Engine = engine;

            Reset();
        }
        #endregion

        #region Control
        public void Reset() {
            _CameraPosition = Vector3.Zero;// +1.8f * Vector3.BackwardLH + 1.2f * Vector3.Up;
            _Yaw = 0;
            _Pitch = 0;

            UpdateViewMatrix();
        }
        public Matrix GetRotation() {
            return Matrix.RotationYawPitchRoll(_Yaw, _Pitch, 0);
        }
        private void UpdateViewMatrix() {
            var rotation = GetRotation();

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = (Vector3)Vector3.Transform(cameraOriginalTarget, rotation);
            Vector3 cameraFinalTarget = _CameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = (Vector3)Vector3.Transform(cameraOriginalUpVector, rotation);

            _View = Matrix.LookAtRH(_CameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }
        #endregion

        #region Visibility test
        public bool Contains(BoundingBox bbox) {
            var frustum = new BoundingFrustum(_View * _Projection);
            return frustum.Contains(bbox) != ContainmentType.Disjoint;
        }
        public bool IsVisible(BoundingBox bounds) {
            return IsVisible(bounds.Minimum) || IsVisible(bounds.Maximum);
        }
        public bool IsVisible(Vector3 point) {
            return false;
        }
        public bool IsVisible(Vector4 point) {
            return IsVisible(new Vector3(point.X, point.Y, point.Z));
        }
        private void AddToCameraPosition(Vector3 vectorToAdd) {
            var rotation = GetRotation();

            Vector3 rotatedVector = (Vector3)Vector3.Transform(vectorToAdd, rotation);
            _CameraPosition += MoveSpeed * rotatedVector;
        }
        #endregion

        private bool _IsEnabled = true;
        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime time) {
            var amount = (float)(time.ElapsedTime.TotalMilliseconds / 2000f);
            Vector3 moveVector = new Vector3(0, 0, 0);
            var aspectRatio = (float)(_Engine.Form.ClientSize.Width / (float)_Engine.Form.ClientSize.Height);

            if (_Engine.Form.Focused) {
                var modFactor = 2f;
                if (_Engine.Keyboard.IsKeyDown(Keys.Space))
                    modFactor *= 2f;
                if (_Engine.Keyboard.IsKeyDown(Keys.ShiftKey))
                    amount *= modFactor;
                if (_Engine.Keyboard.IsKeyDown(Keys.ControlKey))
                    amount /= modFactor;

                if (_Engine.Keyboard.IsKeyDown(Keys.W))
                    moveVector += new Vector3(0, 0, -1);
                if (_Engine.Keyboard.IsKeyDown(Keys.S))
                    moveVector += new Vector3(0, 0, 1);
                if (_Engine.Keyboard.IsKeyDown(Keys.D))
                    moveVector += new Vector3(1, 0, 0);
                if (_Engine.Keyboard.IsKeyDown(Keys.A))
                    moveVector += new Vector3(-1, 0, 0);
                if (_Engine.Keyboard.IsKeyDown(Keys.Q))
                    moveVector += new Vector3(0, 1, 0);
                if (_Engine.Keyboard.IsKeyDown(Keys.Z))
                    moveVector += new Vector3(0, -1, 0);
                if (_Engine.Keyboard.IsKeyDown(Keys.R))
                    Reset();

                if (_Engine.Keyboard.IsKeyDown(Keys.Left))
                    _Yaw += RotationSpeed * amount * 2;
                if (_Engine.Keyboard.IsKeyDown(Keys.Right))
                    _Yaw -= RotationSpeed * amount * 2;

                if (_Engine.Keyboard.IsKeyDown(Keys.Up))
                    _Pitch += RotationSpeed * amount * 2;
                if (_Engine.Keyboard.IsKeyDown(Keys.Down))
                    _Pitch -= RotationSpeed * amount * 2;

                AddToCameraPosition(moveVector * amount);
            }

            UpdateViewMatrix();

            _Projection = Matrix.PerspectiveFovRH(0.9f, aspectRatio, 0.1f, 10000.0f);
        }
    }
}
