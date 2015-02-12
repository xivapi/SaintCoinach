using System;

namespace SaintCoinach.Graphics.Assets {
    public class Bone {
        #region Fields

        internal byte[] _Buffer;

        #endregion

        #region Properties

        public string Name { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public Bone(string name, byte[] buffer) {
            Name = name;
            _Buffer = buffer;
        }

        #endregion

        #endregion

        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }

        public override string ToString() {
            return Name;
        }
    }
}
