using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class ContentType : XivRow {
        #region Properties

        /// <summary>
        /// Gets the name of the current content type.
        /// </summary>
        /// <value>The name of the current content type.</value>
        public Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        /// Gets the icon for the current content type.
        /// </summary>
        /// <value>The icon for the current content type.</value>
        public ImageFile Icon { get { return AsImage("Icon"); } }

        /// <summary>
        /// Gets the icon for the current content show in the duty finder.
        /// </summary>
        /// <value>The icon for the current content show in the duty finder.</value>
        public ImageFile DutyFinderIcon { get { return AsImage("Icon{DutyFinder}"); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentType"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public ContentType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
