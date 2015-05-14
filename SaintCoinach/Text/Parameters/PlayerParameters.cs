using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Parameters {
    using Xiv;

    public class PlayerParameters : ParameterBase {
        public const int ActiveClassJobIndex = 68;
        public const int LevelIndex1 = 69;          // TODO: I have no idea what the difference between these is.
        public const int LevelIndex2 = 72;          // 72 possibly JOB and 69 CLASS ?
        public const int GamePadTypeIndex = 75;     // TODO: 0 for XInput, 1 for PS3, 2 for PS4?
        public const int RegionIndex = 77;          // I think it is, anyway. Only found it for formatting dates. 0-2 = ?; 3 = EU?; 4+ = ?

        #region Constructors
        public PlayerParameters() { }
        public PlayerParameters(ParameterBase copyFrom) : base(copyFrom) { }
        #endregion

        #region Properties
        public int Level {
            get { return Convert.ToInt32(this[LevelIndex1]); }
            set {
                this[LevelIndex1] = value;
                this[LevelIndex2] = value;
            }
        }
        public ClassJob ActiveClassJob {
            get { return this[ActiveClassJobIndex] as ClassJob; }
            set { this[ActiveClassJobIndex] = value; }
        }
        public int GamePadType {
            get { return Convert.ToInt32(this[GamePadTypeIndex]); }
            set { this[GamePadTypeIndex] = value; }
        }
        public int Region {
            get { return Convert.ToInt32(this[RegionIndex]); }
            set { this[RegionIndex] = value; }
        }
        #endregion
    }
}
