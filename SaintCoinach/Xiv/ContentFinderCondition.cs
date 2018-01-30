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

        public ContentMemberType ContentMemberType { get { return As<ContentMemberType>(); } }

        public int ContentIndicator { get { return AsInt32("ContentIndicator"); } }

        /// <summary>
        /// Gets the description of the current content.
        /// </summary>
        /// <value>The description of the current content.</value>
        public Text.XivString Description { get { return (Text.XivString)Sheet.Collection.GetSheet("ContentFinderConditionTransient")[this.Key]["Description"]; } }

        /// <summary>
        /// Gets the minimum level required for the current content.
        /// </summary>
        /// <value>The minimum level required for the current content.</value>
        public int RequiredClassJobLevel { get { return AsInt32("ClassJobLevel{Required}"); } }

        /// <summary>
        /// Gets the maximum level for the current content.
        /// </summary>
        /// <value>The maximum level for the current content.</value>
        public int ClassJobLevelSync { get { return AsInt32("ClassJobLevel{Sync}"); } }

        /// <summary>
        /// Gets the item level required to enter the current content.
        /// </summary>
        /// <value>The item level required to enter the current content.</value>
        public virtual int RequiredItemLevel { get { return AsInt32("ItemLevel{Required}"); } }

        /// <summary>
        /// Gets the item level the current content gets synced to if it is higher.
        /// </summary>
        /// <value>The item level the current content gets synced to if it is higher.</value>
        public int ItemLevelSync { get { return AsInt32("ItemLevel{Sync}"); } }

        /// <summary>
        /// Gets the icon for the current finder content.
        /// </summary>
        /// <value>The icon for the current finder content.</value>
        public virtual Imaging.ImageFile Icon { get { return AsImage("Icon"); } }

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
