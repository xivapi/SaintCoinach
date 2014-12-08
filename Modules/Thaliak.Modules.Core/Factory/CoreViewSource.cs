using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism;

namespace Thaliak.Modules.Core.Factory {
    [Export(typeof(Services.IViewSource))]
    public class CoreViewSource : Services.ViewSourceBase {
        [ImportingConstructor]
        CoreViewSource(Thaliak.Services.IObjectStore temporaryStorage)
            : base(temporaryStorage) {
                Register(typeof(SaintCoinach.Xiv.Item), "ItemView", UriKind.Relative, true, ViewContexts.MainContext);
                Register(typeof(SaintCoinach.Xiv.InventoryItem), "InventoryItemView", UriKind.Relative, true, ViewContexts.MainContext);
                Register(typeof(SaintCoinach.Xiv.Items.Equipment), "EquipmentView", UriKind.Relative, true, ViewContexts.MainContext);

                Register(typeof(SaintCoinach.Xiv.Item), "ItemListItemView", UriKind.Relative, true, ViewContexts.ListItemContext, ViewContexts.ExpandedListItemContext);
                Register(typeof(SaintCoinach.Xiv.Recipe), "RecipeListItemView", UriKind.Relative, true, ViewContexts.ListItemContext);
                Register(typeof(SaintCoinach.Xiv.IShopItem), "ShopItemListItemView", UriKind.Relative, true, ViewContexts.ListItemContext);

                Register(typeof(SaintCoinach.Xiv.Recipe), "ExpandedRecipeListItemView", UriKind.Relative, true, ViewContexts.ExpandedListItemContext);
                Register(typeof(SaintCoinach.Xiv.IShopItem), "ExpandedShopItemListItemView", UriKind.Relative, true, ViewContexts.ExpandedListItemContext);
        }
    }
}
