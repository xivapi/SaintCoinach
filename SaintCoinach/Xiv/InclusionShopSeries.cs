using System.Collections.Generic;
using System.Linq;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv;

public class InclusionShopSeries: XivSubRow
{
    public SpecialShop SpecialShop => As<SpecialShop>();

    public IEnumerable<Item> Items => SpecialShop.Items.SelectMany(i=> i.Rewards.Select(i=> i.Item));
        
    public InclusionShopSeries(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow)
    {
    }
}