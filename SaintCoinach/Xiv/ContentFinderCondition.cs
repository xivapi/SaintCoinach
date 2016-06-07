using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ContentFinderCondition : XivRow {
        #region Properties

        public InstanceContent InstanceContent { get { return As<InstanceContent>(); } }

        /// <summary>
        /// Gets the <see cref="ContentRoulette"/> the current content is in.
        /// </summary>
        /// <value>The <see cref="ContentRoulette"/> the current content is in.</value>
        public ContentRoulette ContentRoulette { get { return As<ContentRoulette>(); } }

        public ContentMemberType ContentMemberType { get { return As<ContentMemberType>(); } }

        public int LegacyIndicator { get { return AsInt32("LegacyIndicator"); } }

        /// <summary>
        /// Gets the minimum level required for the current content.
        /// </summary>
        /// <value>The minimum level required for the current content.</value>
        public int Level { get { return AsInt32("Level"); } }

        /// <summary>
        /// Gets the item level required to enter the current content.
        /// </summary>
        /// <value>The item level required to enter the current content.</value>
        public virtual int RequiredItemLevel { get { return AsInt32("ItemLevel{Required}"); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceContent"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public ContentFinderCondition(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
