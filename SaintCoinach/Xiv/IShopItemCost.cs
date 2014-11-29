using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IShopItemCost {
        Item Item { get; }
        int Count { get; }
        bool RequireHq { get; }
        IShopItem ShopItem { get; }
    }
}
