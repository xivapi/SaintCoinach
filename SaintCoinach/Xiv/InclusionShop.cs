using System;
using System.Collections.Generic;
using System.Linq;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv;

public class InclusionShop: XivRow
{
    private List<InclusionShopCategory> _Categories;
    
    public IEnumerable<InclusionShopCategory> Categories => _Categories ??= BuildCategories();

    public bool IsValid => Categories.Any();
    
    public byte UknownByte => As<byte>(1);
    
    private List<InclusionShopCategory> BuildCategories()
    {
        var list = new List<InclusionShopCategory>();
        for (var i = 0; i < 30; i++)
        {
            var cat = As<InclusionShopCategory>($"Category[{i}]");
            if (cat.Key != 0)
                list.Add(cat);
        }
        return list;
    }

    public InclusionShop(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow)
    {
    }
}