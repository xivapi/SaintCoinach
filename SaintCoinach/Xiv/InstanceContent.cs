﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing a duty.
    /// </summary>
    public class InstanceContent : ContentBase, IItemSource {
        #region Fields
        private InstanceContentData _Data;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the time limit to complete the current content.
        /// </summary>
        /// <value>The time limit to complete the current content.</value>
        public TimeSpan TimeLimit { get { return TimeSpan.FromMinutes(AsInt32("TimeLimitmin")); } }

        /// <summary>
        /// Gets the number of parties for the current content.
        /// </summary>
        /// <value>The number of parties for the current content.</value>
        public int PartyCount { get { return AsInt32("PartyCount"); } }

        /// <summary>
        /// Gets a value indicating whether a specific party composition is required, even in preformed parties.
        /// </summary>
        /// <value>A value indicating whether a specific party composition is required, even in preformed parties.</value>
        /// <seealso cref="PlayersPerParty"/>
        /// <seealso cref="TanksPerParty"/>
        /// <seealso cref="HealersPerParty"/>
        /// <seealso cref="MeleesPerParty"/>
        /// <seealso cref="RangedPerParty"/>
        public bool IsForcedPartyComposition { get { return AsBoolean("ForcePartySetup"); } }

        /// <summary>
        /// Gets the description of the current content.
        /// </summary>
        /// <value>The description of the current content.</value>
        public override Text.XivString Description { get { throw new InvalidOperationException("Use ContentFinderCondition.Description"); } }

        /// <summary>
        /// Gets the numeric order of the current content.
        /// </summary>
        /// <value>The numeric order of the current content.</value>
        public int SortKey { get { return AsInt32("SortKey"); } }

        /// <summary>
        /// Gets a value indicating whether a preformed alliance of parties can register for the current content.
        /// </summary>
        /// <value>A value indicating whether a preformed alliance of parties can register for the current content.</value>
        public bool AllowAllianceRegistration { get { return AsBoolean("AllowAllianceRegistration"); } }

        /// <summary>
        /// Gets the bonus to Allagan Tomestones of Soldiery when completing the current content with a character new to it in the party.
        /// </summary>
        /// <value>The bonus to Allagan Tomestones of Soldiery when completing the current content with a character new to it in the party.</value>
        public int NewPlayerBonus { get { return AsInt32("NewPlayerBonus"); } }

        /// <summary>
        /// Gets the rewards received over the course of the duty.
        /// </summary>
        /// <value>The rewards received over the course of the duty.</value>
        public override IEnumerable<IContentReward> FixedRewards {
            get {
                // XXX: Magic numbers here
                const int TomestoneAKey = 3;    // Poetics
                const int TomestoneBKey = 8;    // Verity
                const int CurrencyCount = 5;

                var tomestones = Sheet.Collection.GetSheet<TomestonesItem>();

                var tomeA = tomestones[TomestoneAKey].Item;
                var tomeB = tomestones[TomestoneBKey].Item;

                var sumA = 0;
                var sumB = 0;
                for (var i = 0; i < CurrencyCount; ++i) {
                    sumA += AsInt32("BossCurrencyA", i);
                    sumB += AsInt32("BossCurrencyB", i);
                }

                if (sumA != 0)
                    yield return new ContentReward(tomeA, sumA);
                if (sumB != 0)
                    yield return new ContentReward(tomeB, sumB);
            }
        }

        public InstanceContentData Data { get { return _Data ?? (_Data = new InstanceContentData(this)); } }

        public BNpcBase Boss { get { return As<BNpcBase>("BNpcBaseBoss"); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceContent"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public InstanceContent(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region IItemSource Members

        private Item[] _ItemSourceItems;

        IEnumerable<Item> IItemSource.Items {
            get {
                if (_ItemSourceItems != null)
                    return _ItemSourceItems;

                var items = new List<Item>();

                if (FixedRewards != null) {
                    foreach (var item in FixedRewards) items.Add(item.Item);
                }

                if (Data.MidBosses != null) {
                    foreach (var boss in Data.MidBosses) {
                        if (boss.RewardItems != null)
                            foreach (var item in boss.RewardItems) items.Add(item.Item);
                    }
                }
                if (Data.Boss != null && Data.Boss.RewardItems != null) {
                    foreach (var item in Data.Boss.RewardItems) items.Add(item.Item);
                }
                if (Data.MapTreasures != null) {
                    foreach (var coffer in Data.MapTreasures)
                        foreach (var item in coffer.Items) items.Add(item);
                }

                return _ItemSourceItems = items.ToArray();
            }
        }

        #endregion
    }
}
