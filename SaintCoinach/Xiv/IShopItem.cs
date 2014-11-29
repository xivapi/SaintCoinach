using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IShopItem {
        Item Item { get; }
        int Count { get; }

        IEnumerable<IShopItemCost> Costs { get; }
        IEnumerable<IShop> Shops { get; }
    }
}
