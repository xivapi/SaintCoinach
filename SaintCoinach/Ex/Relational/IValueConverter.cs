using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using System;

namespace SaintCoinach.Ex.Relational {
    public interface IValueConverter {
        #region Properties

        string TargetTypeName { get; }
        Type TargetType { get; }

        #endregion

        object Convert(IDataRow row, object rawValue);

        JObject ToJson();

        void ResolveReferences(SheetDefinition sheetDef);
    }
}
