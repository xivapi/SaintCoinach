using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search {
    using Behaviors;
    using Interfaces;

    [SearchFunctionExport(Function = "ilv")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ItemLevelQuery : Services.Search.IntegerRangeQuery {
        #region ISearchQuery Members

        public override bool IsMatch(object value) {
            var asInvItem = value as Xiv.InventoryItem;
            if (asInvItem == null)
                return false;
            return IsInRange(asInvItem.ItemLevel.Key);
        }

        public override IEnumerable<Type> MatchedTypes {
            get { yield return typeof(Xiv.InventoryItem); }
        }
        #endregion

        public override string ToString() {
            return "ilv:" + base.ToString();
        }
    }
}
