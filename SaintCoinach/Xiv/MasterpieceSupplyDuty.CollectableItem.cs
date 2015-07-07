using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public partial class MasterpieceSupplyDuty {
        public class CollectableItem {
            #region Properties
            public Item RequiredItem { get; private set; }
            public int Quantity { get; private set; }
            public int CollectabilityHighBonus { get; private set; }
            public int CollectabilityBonus { get; private set; }
            public int CollectabilityBase { get; private set; }
            public int Stars { get; private set; }
            public int RewardBase { get; private set; }
            #endregion

            #region Constructor
            internal CollectableItem(MasterpieceSupplyDuty duty, int index) {
                RequiredItem = duty.As<Item>("RequiredItem", index);
                Quantity = duty.AsInt32("Quantity", index);
                CollectabilityHighBonus = duty.AsInt32("Collectability{HighBonus}", index);
                CollectabilityBonus = duty.AsInt32("Collectability{Bonus}", index);
                CollectabilityBase = duty.AsInt32("Collectability{Base}", index);
                RewardBase = duty.AsInt32("Reward{Base}", index);
                Stars = duty.AsInt32("Stars", index);
            }
            #endregion
        }
    }
}
