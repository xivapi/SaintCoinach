using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class QuestRewardItemGroup {
        #region Properties
        public IEnumerable<QuestRewardItem> Items { get; private set; }
        public QuestRewardGroupType Type { get; private set; }
        public ClassJobCategory ClassJobCategory { get; private set; }
        #endregion

        #region Constructors
        public QuestRewardItemGroup(IEnumerable<QuestRewardItem> items, QuestRewardGroupType type, ClassJobCategory category) {
            this.Items = items;
            this.Type = type;
            this.ClassJobCategory = category;
        }
        #endregion
    }
}
