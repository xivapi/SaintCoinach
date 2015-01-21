using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IShopListingItem {
        Item Item { get; }
        int Count { get; }
        bool IsHq { get; }
        IShopListing ShopItem { get; }
    }
}
