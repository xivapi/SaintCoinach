using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class Bone {
        #region Fields
        internal byte[] _Buffer;
        private string _Name;
        #endregion

        #region Properties
        public string Name { get { return _Name; } }
        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }
        #endregion

        #region Constructor
        public Bone(string name, byte[] buffer) {
            _Name = name;
            _Buffer = buffer;
        }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
