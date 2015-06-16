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
    /// <summary>
    ///     <see cref="RelationalExCollection" /> for OO representations of FFXIV game data.
    /// </summary>
    /// <remarks>
    /// To access data either use <see cref="GetSheet{T}()"/> with the request data type as <c>T</c> or get a collection using one of the exposed properties.
    /// </remarks>
    public class XivCollection : RelationalExCollection {
        #region Fields

        /// <summary>
        ///     Mapping of sheet names to the object types to use for them.
        /// </summary>
        private Dictionary<string, Type> _SheetNameToTypeMap;

        /// <summary>
        ///     Collection of <see cref="BNpc"/> objects.
        /// </summary>
        private BNpcCollection _BNpcs;

        /// <summary>
        ///     Collection of <see cref="ClassJobActionBase"/> (containing both <see cref="Action" /> and <see cref="CraftAction" />).
        /// </summary>
        private ClassJobActionCollection _ClassJobActions;

        /// <summary>
        ///     Collection of ENpc objects (containg data of both <see cref="ENpcBase" /> and <see cref="ENpcResident" />).
        /// </summary>
        private ENpcCollection _ENpcs;

        /// <summary>
        ///     Collection of equipment slots.
        /// </summary>
        private EquipSlotCollection _EquipSlots;

        /// <summary>
        ///     Collection of items (containing both <see cref="Item" /> and <see cref="EventItem" />).
        /// </summary>
        private ItemCollection _Items;

        /// <summary>
        ///     Collection of all shops.
        /// </summary>
        private ShopCollection _Shops;

        /// <summary>
        ///     Database connection to Libra Eorzea data.
        /// </summary>
        private Libra.Entities _Libra;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the collection of <see cref="BNpc"/> objects.
        /// </summary>
        /// <value>The collection of <see cref="BNpc"/> objects.</value>
        /// <remarks>
        /// This property is only supported when the Libra Eorzea database is present.
        /// </remarks>
        public BNpcCollection BNpcs {
            get {
                if (_BNpcs != null) return _BNpcs;
                if (!IsLibraAvailable)
                    throw new NotSupportedException("BNpcs are only available when Libra Eorzea database is present.");

                _BNpcs = new BNpcCollection(this);
                return _BNpcs;
            }
        }

        /// <summary>
        ///     Gets the collection of <see cref="ClassJobActionBase"/> (containing both <see cref="Action" /> and <see cref="CraftAction" />).
        /// </summary>
        /// <value>The collection of <see cref="ClassJobActionBase"/> (containing both <see cref="Action" /> and <see cref="CraftAction" />).</value>
        public ClassJobActionCollection ClassJobActions { get { return _ClassJobActions ?? (_ClassJobActions = new ClassJobActionCollection(this)); } }

        /// <summary>
        ///     Gets the collection of ENpc objects (containg data of both <see cref="ENpcBase" /> and <see cref="ENpcResident" />
        ///     ).
        /// </summary>
        /// <value>The collection of ENpc objects (containg data of both <see cref="ENpcBase" /> and <see cref="ENpcResident" />).</value>
        public ENpcCollection ENpcs { get { return _ENpcs ?? (_ENpcs = new ENpcCollection(this)); } }

        /// <summary>
        ///     Gets the collection of equipment slots.
        /// </summary>
        /// <value>The collection of equipment slots.</value>
        public EquipSlotCollection EquipSlots {
            get { return _EquipSlots ?? (_EquipSlots = new EquipSlotCollection(this)); }
        }

        /// <summary>
        ///     Gets the collection of items (containing both <see cref="Item" /> and <see cref="EventItem" />).
        /// </summary>
        /// <value>The collection of items (containing both <see cref="Item" /> and <see cref="EventItem" />).</value>
        public ItemCollection Items { get { return _Items ?? (_Items = new ItemCollection(this)); } }

        /// <summary>
        ///     Gets the collection of all shops.
        /// </summary>
        /// <value>The collection of all shops.</value>
        public ShopCollection Shops { get { return _Shops ?? (_Shops = new ShopCollection(this)); } }

        /// <summary>
        /// Gets a value indicating whether the Libra Eorzea database is available.
        /// </summary>
        /// <value>A value indicating whether the Libra Eorzea database is available.</value>
        public bool IsLibraAvailable { get { return _Libra != null; } }

        /// <summary>
        /// Gets the connection to the Libra Eorzea database.
        /// </summary>
        /// <value>The connection to the Libra Eorzea database.</value>
        public Libra.Entities Libra { get { return _Libra; } }

        #endregion

        #region Constructors
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="XivCollection" /> class.
        /// </summary>
        /// <param name="packCollection">The <see cref="PackCollection" /> to use to access game data.</param>
        public XivCollection(PackCollection packCollection) : this(packCollection, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XivCollection" /> class.
        /// </summary>
        /// <param name="packCollection">The <see cref="PackCollection" /> to use to access game data.</param>
        /// <param name="libraDatabase"><see cref="FileInfo"/> of the Libra Eorzea database file, or <c>null</c> if Libra data should be disabled.</param>
        public XivCollection(PackCollection packCollection, System.IO.FileInfo libraDatabase)
            : base(packCollection) {

            if (libraDatabase != null && libraDatabase.Exists) {
                const string LibraConnectionStringFormat = @"metadata=res://*/Libra.LibraModel.csdl|res://*/Libra.LibraModel.ssdl|res://*/Libra.LibraModel.msl;provider=System.Data.SQLite.EF6;provider connection string='data source=""{0}""'";
                var connStr = string.Format(LibraConnectionStringFormat, libraDatabase.FullName);
                _Libra = new Libra.Entities(connStr);
            }
        }

        #endregion

        #region Get

        /// <summary>
        ///     Get the <see cref="IXivSheet{T}" /> for a specific type.
        /// </summary>
        /// <typeparam name="T">Type of the rows to get the <see cref="IXivSheet{T}" /> for.</typeparam>
        /// <returns>Returns the <see cref="IXivSheet{T}" /> for the specified <c>T</c>.</returns>
        public IXivSheet<T> GetSheet<T>() where T : IXivRow {
            var t = typeof(T);

            var attr = t.GetCustomAttribute<XivSheetAttribute>();
            if (attr != null)
                return GetSheet<T>(attr.SheetName);

            var name = t.FullName.Substring(t.FullName.IndexOf(".Xiv.", StringComparison.OrdinalIgnoreCase) + 5);
            return GetSheet<T>(name);
        }

        /// <summary>
        ///     Get the <see cref="IXivSheet{T}" /> for a specific type using a sheet identifier.
        /// </summary>
        /// <typeparam name="T">Type of the rows to get the <see cref="IXivSheet{T}" /> for.</typeparam>
        /// <param name="id">Sheet identifier.</param>
        /// <returns>Returns the <see cref="IXivSheet{T}" /> for a specific type using <c>id</c> as sheet identifier.</returns>
        public new IXivSheet<T> GetSheet<T>(int id) where T : IXivRow {
            return (IXivSheet<T>)GetSheet(id);
        }

        /// <summary>
        ///     Get the <see cref="IXivSheet" /> for a sheet identifier.
        /// </summary>
        /// <param name="id">Sheet identifier.</param>
        /// <returns>Returns the <see cref="IXivSheet" /> with <c>id</c> as sheet identifier.</returns>
        public new IXivSheet GetSheet(int id) {
            return (IXivSheet)base.GetSheet(id);
        }

        /// <summary>
        ///     Get the <see cref="IXivSheet{T}" /> for a specific type using a sheet name.
        /// </summary>
        /// <typeparam name="T">Type of the rows to get the <see cref="IXivSheet{T}" /> for.</typeparam>
        /// <param name="name">Name of the sheet.</param>
        /// <returns>Returns the <see cref="IXivSheet{T}" /> for a specific type using <c>name</c> as sheet name.</returns>
        public new IXivSheet<T> GetSheet<T>(string name) where T : IXivRow {
            return (IXivSheet<T>)GetSheet(name);
        }

        /// <summary>
        ///     Get the <see cref="IXivSheet" /> for a sheet with a specific name.
        /// </summary>
        /// <param name="name">Name of the sheet.</param>
        /// <returns>Returns the <see cref="IXivSheet" /> with <c>name</c> as sheet name.</returns>
        public new IXivSheet GetSheet(string name) {
            return (IXivSheet)base.GetSheet(name);
        }

        #endregion

        #region Factory

        /// <summary>
        ///     Delegate used to create non-generic <see cref="IXivSheet" />s.
        /// </summary>
        /// <param name="collection"><see cref="XivCollection" /> for which to create the <see cref="IXivSheet" />.</param>
        /// <param name="sourceSheet"><see cref="IRelationalSheet" /> on top of which to create the <see cref="IXivSheet" />.</param>
        /// <returns>Returns the <see cref="IXivSheet" /> created on top of <c>sourceSheet</c>.</returns>
        protected delegate IXivSheet XivSheetCreator(XivCollection collection, IRelationalSheet sourceSheet);

        /// <summary>
        ///     Mappings of source sheets to special <see cref="XivSheetCreator" />s.
        /// </summary>
        protected static readonly Dictionary<string, XivSheetCreator> SpecialSheetTypes =
            new Dictionary<string, XivSheetCreator> {
                {
                    "Item", (c, s) => new InventoryItemSheet(c, s)
                }, {
                    "ItemAction", (c, s) => new ItemActionSheet(c, s)
                }
            };

        /// <summary>
        /// Create a <see cref="ISheet"/> for a <see cref="Header"/>.
        /// </summary>
        /// <param name="header"><see cref="Header"/> to create the sheet for.</param>
        /// <returns>Returns the created <see cref="ISheet"/>.</returns>
        protected override ISheet CreateSheet(Header header) {
            var baseSheet = (IRelationalSheet)base.CreateSheet(header);

            var xivSheet = TryCreateXivSheet(baseSheet);
            return xivSheet ?? new XivSheet<XivRow>(this, baseSheet);
        }

        /// <summary>
        /// Attempt to create a <see cref="IXivSheet{T}"/> with a specific <c>T</c> based on the name of the sheet.
        /// </summary>
        /// <remarks>
        /// Uses the mappings in <see cref="SpecialSheetTypes"/> if present; otherwise looks for a type matching the sheet's name and creates a generic <see cref="XivSheet{T}"/>; returns <c>null</c> if neither is found.
        /// </remarks>
        /// <param name="sourceSheet"><see cref="IRelationalSheet"/> to access the source data.</param>
        /// <returns>Returns a <see cref="IXivSheet"/> if a matching one could be created; <c>null</c> otherwise.</returns>
        protected virtual IXivSheet TryCreateXivSheet(IRelationalSheet sourceSheet) {
            XivSheetCreator specialCreator;
            if (SpecialSheetTypes.TryGetValue(sourceSheet.Name, out specialCreator))
                return specialCreator(this, sourceSheet);

            var match = GetXivRowType(sourceSheet.Name);
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

        private Type GetXivRowType(string sheetName) {
            if (_SheetNameToTypeMap == null)
                BuildSheetToTypeMap();

            Type match;
            if (_SheetNameToTypeMap.TryGetValue(sheetName, out match))
                return match;

            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            var search = "Xiv." + sheetName.Replace('/', '.');
            var targetType = typeof(IXivRow);
            return allTypes.FirstOrDefault(_ => _.FullName.EndsWith(search) && targetType.IsAssignableFrom(_));
        }

        private void BuildSheetToTypeMap() {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var attrTypes = allTypes.Select(t => new { Type = t, Attr = t.GetCustomAttribute<XivSheetAttribute>() }).Where(t => t.Attr != null);

            _SheetNameToTypeMap = attrTypes.ToDictionary(i => i.Attr.SheetName, i => i.Type);
        }

        #endregion
    }
}
