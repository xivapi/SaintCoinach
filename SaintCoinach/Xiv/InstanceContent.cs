using System;
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
    public class InstanceContent : ContentBase {
        #region Fields
        private InstanceContentData _Data;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="InstanceContentType"/> of the current content.
        /// </summary>
        /// <value>The <see cref="InstanceContentType"/> of the current content.</value>
        public InstanceContentType InstanceContentType { get { return As<InstanceContentType>(); } }

        /// <summary>
        /// Gets the <see cref="ContentRoulette"/> the current content is in.
        /// </summary>
        /// <value>The <see cref="ContentRoulette"/> the current content is in.</value>
        public ContentRoulette ContentRoulette { get { return As<ContentRoulette>(); } }

        /// <summary>
        /// Gets the time limit to complete the current content.
        /// </summary>
        /// <value>The time limit to complete the current content.</value>
        public TimeSpan TimeLimit { get { return TimeSpan.FromMinutes(AsInt32("TimeLimit{min}")); } }

        /// <summary>
        /// Gets the minimum level required for the current content.
        /// </summary>
        /// <value>The minimum level required for the current content.</value>
        public int Level { get { return AsInt32("Level"); } }

        /// <summary>
        /// Gets the maximum level for the current content.
        /// </summary>
        /// <value>The maximum level for the current content.</value>
        public int LevelSync { get { return AsInt32("Level{Sync}"); } }

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
        public override string Description { get { return (string)Sheet.Collection.GetSheet("ContentDescription")[this.Key]["Description"]; } }

        /// <summary>
        /// Gets the item level the current content gets synced to if it is higher.
        /// </summary>
        /// <value>The item level the current content gets synced to if it is higher.</value>
        public int ItemLevelSync { get { return AsInt32("ItemLevel{Sync}"); } }

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
        /// Gets the rewards received upon completing the current content.
        /// </summary>
        /// <value>The rewards received upon completing the current content.</value>
        public override IEnumerable<IContentReward> FixedRewards {
            get {
                // XXX: Magic numbers here
                const int SoldieryItemKey = 26;
                const int PoeticsItemKey = 28;

                var sold = AsInt32("Reward{Soldiery}");
                var poe = AsInt32("Reward{Poetics}");
                if (sold == 0 && poe == 0)
                    yield break;

                var items = Sheet.Collection.GetSheet<Item>();
                if (sold != 0)
                    yield return new ContentReward(items[SoldieryItemKey], sold);
                if (poe != 0)
                    yield return new ContentReward(items[PoeticsItemKey], poe);
            }
        }

        /// <summary>
        /// Gets the <see cref="TerritoryType"/> for the current content.
        /// </summary>
        /// <value>The <see cref="TerritoryType"/> for the current content.</value>
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }

        public InstanceContentData Data { get { return _Data ?? (_Data = new InstanceContentData(this)); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceContent"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public InstanceContent(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
