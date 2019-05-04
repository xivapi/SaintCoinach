using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ContentFinderCondition : XivRow {
        #region Properties

        public XivRow Content => As<XivRow>("Content");

        public ContentMemberType ContentMemberType => As<ContentMemberType>();

        public ContentType ContentType => As<ContentType>();

        public Text.XivString Name => AsString("Name");

        // 1 = InstanceContent, 2 = PartyContent, 3 = PublicContent, 4 = GoldSaucerContent, 5 = special or test event content?
        public byte ContentLinkType => As<byte>("ContentLinkType");

        /// <summary>
        /// Gets the description of the current content.
        /// </summary>
        /// <value>The description of the current content.</value>
        public Text.XivString Description => (Text.XivString)Sheet.Collection.GetSheet("ContentFinderConditionTransient")[this.Key]["Description"];

        /// <summary>
        /// Gets the minimum level required for the current content.
        /// </summary>
        /// <value>The minimum level required for the current content.</value>
        public int RequiredClassJobLevel => AsInt32("ClassJobLevel{Required}");

        /// <summary>
        /// Gets the maximum level for the current content.
        /// </summary>
        /// <value>The maximum level for the current content.</value>
        public int ClassJobLevelSync => AsInt32("ClassJobLevel{Sync}");

        /// <summary>
        /// Gets the item level required to enter the current content.
        /// </summary>
        /// <value>The item level required to enter the current content.</value>
        public virtual int RequiredItemLevel => AsInt32("ItemLevel{Required}");

        /// <summary>
        /// Gets the item level the current content gets synced to if it is higher.
        /// </summary>
        /// <value>The item level the current content gets synced to if it is higher.</value>
        public int ItemLevelSync => AsInt32("ItemLevel{Sync}");

        public virtual Imaging.ImageFile Image => AsImage("Image");

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
