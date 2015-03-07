using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class QuestRequirements {
        public enum PreqrequisiteType : byte {
            None = 0,
            All = 1,
            Any = 2,
        }
    }
}
