using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using SaintCoinach.IO;
using SaintCoinach.Xiv.Collections;
using SaintCoinach.Xiv.Sheets;

namespace SaintCoinach.Xiv {
    public class XivCollection : RelationalExCollection {
        #region Fields

        private ENpcCollection _ENpcs;
        private EquipSlotCollection _EquipSlots;
        private ItemCollection _Items;
        private ShopCollection _Shops;

        #endregion

        #region Constructors

        #region Constructor

        public XivCollection(PackCollection packCollection) : base(packCollection) { }

        #endregion

        #endregion

        #region Constructor

        public ENpcCollection ENpcs { get { return _ENpcs ?? (_ENpcs = new ENpcCollection(this)); } }

        public EquipSlotCollection EquipSlots {
            get { return _EquipSlots ?? (_EquipSlots = new EquipSlotCollection(this)); }
        }

        public ItemCollection Items { get { return _Items ?? (_Items = new ItemCollection(this)); } }
        public ShopCollection Shops { get { return _Shops ?? (_Shops = new ShopCollection(this)); } }

        #endregion

        #region Get

        public IXivSheet<T> GetSheet<T>() where T : IXivRow {
            var t = typeof(T);
            var name = t.FullName.Substring(t.FullName.IndexOf(".Xiv.", StringComparison.OrdinalIgnoreCase) + 5);
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

        protected static readonly Dictionary<string, XivSheetCreator> SpecialSheetTypes =
            new Dictionary<string, XivSheetCreator> {
                {
                    "Item", (c, s) => new InventoryItemSheet(c, s)
                }, {
                    "ItemAction", (c, s) => new ItemActionSheet(c, s)
                }
            };

        protected override ISheet CreateSheet(Header header) {
            var baseSheet = (IRelationalSheet)base.CreateSheet(header);

            var xivSheet = TryCreateXivSheet(baseSheet);
            return xivSheet ?? new XivSheet<XivRow>(this, baseSheet);
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
            var constructor = constructedType.GetConstructor(
                                                             BindingFlags.Instance | BindingFlags.NonPublic
                                                             | BindingFlags.Public,
                null,
                new[] {
                    typeof(XivCollection), typeof(IRelationalSheet)
                },
                null);
            return (IXivSheet)constructor.Invoke(new object[] {
                this, sourceSheet
            });
        }

        #endregion
    }
}
