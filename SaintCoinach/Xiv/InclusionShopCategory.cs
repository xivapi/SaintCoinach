using System;
using System.Collections.Generic;
using System.Linq;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv;

public class InclusionShopCategory : XivRow
{
    public string Name => AsString("Name");
    
    public ClassJobCategory ClassJobCategory => As<ClassJobCategory>();
    
    public IEnumerable<InclusionShopSeries> Series => AsSubRows<InclusionShopSeries>();

    private IEnumerable<T> AsSubRows<T>() where T : XivSubRow
    {
        return AsSubRows<T>(typeof(T).Name);
    }
    private IEnumerable<T> AsSubRows<T>(string rowName) where T :XivSubRow
    {
        var sheetName = typeof(T).Name;
        var key = (ushort)GetRaw(rowName);
        
        var sheet = Sheet.Collection.GetSheet2<T>(sheetName)
            .Cast<T>()
            .Where(r => r.ParentKey == key)
            .ToArray();
        
        return sheet;
    }

    public InclusionShopCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow)
    {
    }
}