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
