using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
#if DEBUG
    [DebuggerDisplay("( {X}, {Y} )")]
#endif
    [Serializable]
    public struct Vector2 {
        private Single _X;
        private Single _Y;

        public Single X { get { return _X; } set { _X = value; } }
        public Single Y { get { return _Y; } set { _Y = value; } }

        public Vector2(Single x, Single y) {
            _X = x;
            _Y = y;
        }

        public static bool operator ==(Vector2 v1, Vector2 v2) {
            return (v1.X == v2.X && v1.Y == v2.Y);
        }
        public static bool operator !=(Vector2 v1, Vector2 v2) {
            return (v1.X != v2.X || v1.Y != v2.Y);
        }
        public override bool Equals(object obj) {
            if (obj is Vector2)
                return (Vector2)obj == this;
            return false;
        }
        public override int GetHashCode() {
            return X.GetHashCode() * Y.GetHashCode();
        }
        public override string ToString() {
            return string.Format("({0}, {1})", X, Y);
        }
    }
#if DEBUG
    [DebuggerDisplay("( {X}, {Y}, {Z} )")]
#endif
    [Serializable]
    public struct Vector3 {
        private Single _X;
        private Single _Y;
        private Single _Z;

        public Single X { get { return _X; } set { _X = value; } }
        public Single Y { get { return _Y; } set { _Y = value; } }
        public Single Z { get { return _Z; } set { _Z = value; } }

        public Vector3(Single x, Single y, Single z) {
            _X = x;
            _Y = y;
            _Z = z;
        }

        public static bool operator ==(Vector3 v1, Vector3 v2) {
            return (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z);
        }
        public static bool operator !=(Vector3 v1, Vector3 v2) {
            return (v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z);
        }
        public override bool Equals(object obj) {
            if (obj is Vector3)
                return (Vector3)obj == this;
            return false;
        }
        public override int GetHashCode() {
            return X.GetHashCode() * Y.GetHashCode() * Z.GetHashCode();
        }
        public override string ToString() {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
    }
#if DEBUG
    [DebuggerDisplay("( {X}, {Y}, {Z}, {Y} )")]
#endif
    [Serializable]
    public struct Vector4 {
        private Single _X;
        private Single _Y;
        private Single _Z;
        private Single _W;

        public Single X { get { return _X; } set { _X = value; } }
        public Single Y { get { return _Y; } set { _Y = value; } }
        public Single Z { get { return _Z; } set { _Z = value; } }
        public Single W { get { return _W; } set { _W = value; } }

        public Vector4(Single x, Single y, Single z, Single w) {
            _X = x;
            _Y = y;
            _Z = z;
            _W = w;
        }

        public static bool operator ==(Vector4 v1, Vector4 v2) {
            return (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z && v1.W == v2.W);
        }
        public static bool operator !=(Vector4 v1, Vector4 v2) {
            return (v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z || v1.W != v2.W);
        }
        public override bool Equals(object obj) {
            if (obj is Vector4)
                return (Vector4)obj == this;
            return false;
        }
        public override int GetHashCode() {
            return X.GetHashCode() * Y.GetHashCode() * Z.GetHashCode() * W.GetHashCode();
        }
        public override string ToString() {
            return string.Format("({0}, {1}, {2}, {3})", X, Y, Z, W);
        }
    }
}
