using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class QuestRequirements {
        public class ClassJobRequirement {
            #region Properties
            public ClassJobCategory ClassJobCategory { get; private set; }
            public int Level { get; private set; }
            #endregion

            #region Constructors
            internal ClassJobRequirement(ClassJobCategory category, int level) {
                this.ClassJobCategory = category;
                this.Level = level;
            }
            #endregion
        }
    }
}
