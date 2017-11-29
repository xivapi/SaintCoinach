using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public partial class MasterpieceSupplyDuty {
        public class CollectableItem {
            private double _xpMultiplier, _xpMultiplier2;
            private double _currencyMultiplier, _currencyMultiplier2;

            #region Properties
            public MasterpieceSupplyDuty MasterpieceSupplyDuty { get; private set; }
            public Item RequiredItem { get; private set; }
            public int Quantity { get; private set; }
            public int CollectabilityHighBonus { get; private set; }
            public int CollectabilityBonus { get; private set; }
            public int CollectabilityBase { get; private set; }
            public int Stars { get; private set; }
            public int ScripRewards { get; private set; }
            public int MaxClassJobLevel { get; private set; }
            public int ExpModifier { get; private set; }
            #endregion

            #region Constructor
            internal CollectableItem(MasterpieceSupplyDuty duty, int index) {
                MasterpieceSupplyDuty = duty;
                RequiredItem = duty.As<Item>("RequiredItem", index);
                Quantity = duty.AsInt32("Quantity", index);
                CollectabilityHighBonus = duty.AsInt32("Collectability{HighBonus}", index);
                CollectabilityBonus = duty.AsInt32("Collectability{Bonus}", index);
                CollectabilityBase = duty.AsInt32("Collectability{Base}", index);
                ExpModifier = duty.AsInt32("ExpModifier", index);
                ScripRewards = duty.AsInt32("Reward{Scrips}", index);
                MaxClassJobLevel = duty.AsInt32("ClassJobLevel{Max}", index);
                Stars = duty.AsInt32("Stars", index);

                var bonusMultiplierRow = duty.As<IXivRow>("BonusMultiplier", index);
                _xpMultiplier = ((double)(UInt16)bonusMultiplierRow["XpMultiplier[1]"]) / 1000;
                _xpMultiplier2 = ((double)(UInt16)bonusMultiplierRow["XpMultiplier[0]"]) / 1000;
                _currencyMultiplier = ((double)(UInt16)bonusMultiplierRow["CurrencyMultiplier[1]"]) / 1000;
                _currencyMultiplier2 = ((double)(UInt16)bonusMultiplierRow["CurrencyMultiplier[0]"]) / 1000;
            }
            #endregion

            public int[] CalculateExp(int level) {
                // Constrain level by valid range for this collectable.
                level = Math.Min(MasterpieceSupplyDuty.ClassJobLevel, level);
                level = Math.Max(MaxClassJobLevel, level);

                // Find the base XP.
                var paramGrow = MasterpieceSupplyDuty.Sheet.Collection.GetSheet<ParamGrow>()[level];
                var expPortion = ((double)1000 / ExpModifier);
                var baseExp = (int)(paramGrow.ExpToNext / expPortion);

                // Apply bonus multipliers.
                return new int[] { baseExp, (int)(baseExp * _xpMultiplier), (int)(baseExp * _xpMultiplier2) };
            }

            public int[] CalculateScripRewards() {
                return new int[] { ScripRewards, (int)(ScripRewards * _currencyMultiplier), (int)(ScripRewards * _currencyMultiplier2) };
            }
        }
    }
}
