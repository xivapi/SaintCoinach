using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IShop {
        string Name { get; }
        IEnumerable<ENpc> ENpcs { get; }
        IEnumerable<IShopItem> ShopItems { get; }
    }
}
