using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class QuestRewardItem {
        #region Properties
        public Item Item { get; private set; }
        public int Count { get; private set; }
        public Stain Stain { get; private set; }
        #endregion

        #region Constructors
        public QuestRewardItem(Item item, int count, Stain stain) {
            this.Item = item;
            this.Count = count;
            this.Stain = stain;
        }
        #endregion
    }
}
