using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Base class representing duties.
    /// </summary>
    public abstract class ContentBase : XivRow {
        #region Properties
        
        /// <summary>
        /// Gets the name of the current content.
        /// </summary>
        /// <value>The name of the current content.</value>
        public virtual Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        /// Gets the description of the current content.
        /// </summary>
        /// <value>The description of the current content.</value>
        public virtual Text.XivString Description { get { return AsString("Description"); } }

        /// <summary>
        /// Gets a value indicating whether the current content is shown in the duty finder.
        /// </summary>
        /// <value>A value indicating whether the current content is shown in the duty finder.</value>
        public virtual bool IsInDutyFinder { get { return AsBoolean("IsInDutyFinder"); } }

        /// <summary>
        /// Gets the item level required to enter the current content.
        /// </summary>
        /// <value>The item level required to enter the current content.</value>
        public virtual int RequiredItemLevel { get { return AsInt32("ItemLevel{Required}"); } }

        /// <summary>
        /// Gets the maximum number of players allowed per party to register for the current content.
        /// </summary>
        /// <value>The maximum number of players allowed per party to register for the current content.</value>
        public virtual int PlayersPerParty { get { return AsInt32("PlayersPerParty"); } }

        /// <summary>
        /// Gets the number of tanks per party.
        /// </summary>
        /// <value>The number of tanks per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// </remarks>
        public virtual int TanksPerParty { get { return AsInt32("TanksPerParty"); } }

        /// <summary>
        /// Gets the number of healers per party.
        /// </summary>
        /// <value>The number of healers per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// </remarks>
        public virtual int HealersPerParty { get { return AsInt32("HealersPerParty"); } }

        /// <summary>
        /// Gets the number of melee DDs per party.
        /// </summary>
        /// <value>The number of melee DDs per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// For most content this and the value of <see cref="RangedPerParty"/> are counted as one total for DDs of any type.
        /// </remarks>
        public virtual int MeleesPerParty { get { return AsInt32("MeleesPerParty"); } }

        /// <summary>
        /// Gets the number of ranged DDs per party.
        /// </summary>
        /// <value>The number of ranged DDs per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// For most content this and the value of <see cref="MeleesPerParty"/> are counted as one total for DDs of any type.
        /// </remarks>
        public virtual int RangedPerParty { get { return AsInt32("RangedPerParty"); } }

        /// <summary>
        /// Gets the icon for the current content.
        /// </summary>
        /// <value>The icon for the current content.</value>
        public virtual Imaging.ImageFile Icon { get { return AsImage("Icon"); } }

        /// <summary>
        /// Gets the rewards received upon completing the current content.
        /// </summary>
        /// <value>The rewards received upon completing the current content.</value>
        public abstract IEnumerable<IContentReward> FixedRewards { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        protected ContentBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
