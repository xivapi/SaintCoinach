using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ContentMemberType : XivRow {
        #region Properties

        /// <summary>
        /// Gets the number of tanks per party.
        /// </summary>
        /// <value>The number of tanks per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// </remarks>
        public int TanksPerParty { get { return AsInt32("TanksPerParty"); } }

        /// <summary>
        /// Gets the number of healers per party.
        /// </summary>
        /// <value>The number of healers per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// </remarks>
        public int HealersPerParty { get { return AsInt32("HealersPerParty"); } }

        /// <summary>
        /// Gets the number of melee DDs per party.
        /// </summary>
        /// <value>The number of melee DDs per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// For most content this and the value of <see cref="RangedPerParty"/> are counted as one total for DDs of any type.
        /// </remarks>
        public int MeleesPerParty { get { return AsInt32("MeleesPerParty"); } }

        /// <summary>
        /// Gets the number of ranged DDs per party.
        /// </summary>
        /// <value>The number of ranged DDs per party.</value>
        /// <remarks>
        /// This value is only relevant when registering without a complete preformed party, or for specific content (i.e. Wolves' Den).
        /// For most content this and the value of <see cref="MeleesPerParty"/> are counted as one total for DDs of any type.
        /// </remarks>
        public int RangedPerParty { get { return AsInt32("RangedPerParty"); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceContent"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public ContentMemberType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
