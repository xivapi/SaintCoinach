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

    [SearchFunctionExport(Function = "category")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ItemCategoryQuery : ISearchQuery {
        [Import]
        private Xiv.XivCollection Data { get; set; }

        public Xiv.ItemUICategory Category { get; set; }

        #region ISearchQuery Members

        public IEnumerable<Type> MatchedTypes {
            get { yield return typeof(Xiv.InventoryItem); }
        }

        public bool IsMatch(object value) {
            var asInvItem = value as Xiv.InventoryItem;
            if (asInvItem == null)
                return false;
            return asInvItem.ItemUICategory == Category;
        }

        public void Set(string args) {
            int key;
            var catSheet = Data.GetSheet<Xiv.ItemUICategory>();
            if (int.TryParse(args, out key))
                Category = catSheet.FirstOrDefault(_ => _.Key == key);
            else
                Category = catSheet.FirstOrDefault(_ => _.Name.IndexOf(args, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #endregion

        public override string ToString() {
            return string.Format("category:{0}", Category);
        }
    }
}
