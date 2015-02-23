using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class InstanceContentType : XivRow {
        #region Properties

        /// <summary>
        /// Gets the numeric order of the current content type.
        /// </summary>
        /// <value>The numeric order of the current content type.</value>
        public int SortKey { get { return AsInt32("SortKey"); } }

        /// <summary>
        /// Gets the <see cref="ContentType"/> for the current instance type.
        /// </summary>
        /// <value>The <see cref="ContentType"/> for the current instance type.</value>
        public ContentType ContentType { get { return As<ContentType>(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceContentType"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public InstanceContentType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
