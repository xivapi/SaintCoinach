using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class QuestRewardItem {
        #region Properties
        public Item Item { get; private set; }
        public int[] Counts { get; private set; }
        public Stain Stain { get; private set; }
        public bool IsHq { get; private set; }
        #endregion

        #region Constructors
        public QuestRewardItem(Item item, int count, Stain stain, bool isHq) :
            this(item, new[] { count }, stain, isHq) { }

        public QuestRewardItem(Item item, IEnumerable<int> counts, Stain stain, bool isHq) {
            this.Item = item;
            this.Counts = counts.ToArray();
            this.Stain = stain;
            this.IsHq = isHq;
        }
        #endregion
    }
}
