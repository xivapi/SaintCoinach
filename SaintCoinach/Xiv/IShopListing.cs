using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IShopListing {
        IEnumerable<IShopListingItem> Rewards { get; }
        IEnumerable<IShopListingItem> Costs { get; }
        IEnumerable<IShop> Shops { get; }
    }
}
