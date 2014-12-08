using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex;
    using Ex.Relational;

    public class XivCollection : RelationalExCollection {
        #region Fields
        private Collections.ENpcCollection _ENpcs;
        private Collections.EquipSlotCollection _EquipSlots;
        private Collections.ItemCollection _Items;
        private Collections.ShopCollection _Shops;
        #endregion

        #region Constructor
        public Collections.ENpcCollection ENpcs { get { return _ENpcs ?? (_ENpcs = new Collections.ENpcCollection(this)); } }
        public Collections.EquipSlotCollection EquipSlots { get { return _EquipSlots ?? (_EquipSlots = new Collections.EquipSlotCollection(this)); } }
        public Collections.ItemCollection Items { get { return _Items ?? (_Items = new Collections.ItemCollection(this)); } }
        public Collections.ShopCollection Shops { get { return _Shops ?? (_Shops = new Collections.ShopCollection(this)); } }
        #endregion

        #region Constructor
        public XivCollection(IO.PackCollection packCollection) : base(packCollection) { }
        #endregion

        #region Get
        public IXivSheet<T> GetSheet<T>() where T : IXivRow {
            var t = typeof(T);
            var name = t.FullName.Substring(t.FullName.IndexOf(".Xiv.") + 5);
            return GetSheet<T>(name);
        }
        public new IXivSheet<T> GetSheet<T>(int id) where T : IXivRow {
            return (IXivSheet<T>)GetSheet(id);
        }
        public new IXivSheet GetSheet(int id) {
            return (IXivSheet)base.GetSheet(id);
        }
        public new IXivSheet<T> GetSheet<T>(string name) where T : IXivRow {
            return (IXivSheet<T>)GetSheet(name);
        }
        public new IXivSheet GetSheet(string name) {
            return (IXivSheet)base.GetSheet(name);
        }
        #endregion

        #region Factory
        protected delegate IXivSheet XivSheetCreator(XivCollection collection, IRelationalSheet sourceSheet);
        protected static Dictionary<string, XivSheetCreator> SpecialSheetTypes = new Dictionary<string, XivSheetCreator> {
            { "Item", (c, s) => new Sheets.InventoryItemSheet(c, s) },
            { "ItemAction", (c, s) => new Sheets.ItemActionSheet(c, s) },
        };
        protected override ISheet CreateSheet(Header header) {
            var baseSheet = (IRelationalSheet)base.CreateSheet(header);

            var xivSheet = TryCreateXivSheet(baseSheet);
            if (xivSheet == null)
                return new XivSheet<XivRow>(this, baseSheet);

            return xivSheet;
        }
        protected virtual IXivSheet TryCreateXivSheet(IRelationalSheet sourceSheet) {
            XivSheetCreator specialCreator;
            if (SpecialSheetTypes.TryGetValue(sourceSheet.Name, out specialCreator))
                return specialCreator(this, sourceSheet);

            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var search = "Xiv." + sourceSheet.Name.Replace('/', '.');
            var targetType = typeof(IXivRow);
            var match = allTypes.FirstOrDefault(_ => _.FullName.EndsWith(search) && targetType.IsAssignableFrom(_));
            if (match == null)
                return null;

            var genericType = typeof(XivSheet<>);

            var constructedType = genericType.MakeGenericType(match);
            var constructors = constructedType.GetConstructors();
            var constructor = constructedType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new Type[] { typeof(XivCollection), typeof(IRelationalSheet) },
                null);
            return (IXivSheet)constructor.Invoke(new object[] { this, sourceSheet });
        }
        #endregion
    }
}
