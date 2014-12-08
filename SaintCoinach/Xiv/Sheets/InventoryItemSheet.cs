using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Sheets {
    using Ex.Relational;

    public class InventoryItemSheet : XivSheet<Item> {
        #region Static
        // Types as defined in ItemUICategory
        private static Type DefaultItemType = typeof(InventoryItem);
        private static Dictionary<int, Type> ItemTypes = new Dictionary<int, Type> {
            // 
            {  1, typeof(Items.PhysicalWeapon) },
            {  2, typeof(Items.PhysicalWeapon) },
            {  3, typeof(Items.PhysicalWeapon) },
            {  4, typeof(Items.PhysicalWeapon) },
            {  5, typeof(Items.PhysicalWeapon) },
            { 84, typeof(Items.PhysicalWeapon) },
            // 
            {  6, typeof(Items.MagicWeapon) },
            {  7, typeof(Items.MagicWeapon) },
            {  8, typeof(Items.MagicWeapon) },
            {  9, typeof(Items.MagicWeapon) },
            { 10, typeof(Items.MagicWeapon) },
            // 
            { 11, typeof(Items.Shield) },
            // 
            { 12, typeof(Items.CraftingTool) },
            { 13, typeof(Items.CraftingTool) },
            { 14, typeof(Items.CraftingTool) },
            { 15, typeof(Items.CraftingTool) },
            { 16, typeof(Items.CraftingTool) },
            { 17, typeof(Items.CraftingTool) },
            { 18, typeof(Items.CraftingTool) },
            { 19, typeof(Items.CraftingTool) },
            { 20, typeof(Items.CraftingTool) },
            { 21, typeof(Items.CraftingTool) },
            { 22, typeof(Items.CraftingTool) },
            { 23, typeof(Items.CraftingTool) },
            { 24, typeof(Items.CraftingTool) },
            { 25, typeof(Items.CraftingTool) },
            { 26, typeof(Items.CraftingTool) },
            { 27, typeof(Items.CraftingTool) },
            // 
            { 28, typeof(Items.GatheringTool) },
            { 29, typeof(Items.GatheringTool) },
            { 30, typeof(Items.GatheringTool) },
            { 31, typeof(Items.GatheringTool) },
            { 32, typeof(Items.GatheringTool) },
            // 
            { 34, typeof(Items.Armour) },
            { 35, typeof(Items.Armour) },
            { 36, typeof(Items.Armour) },
            { 37, typeof(Items.Armour) },
            { 38, typeof(Items.Armour) },
            { 39, typeof(Items.Armour) },
            // 
            { 40, typeof(Items.Accessory) },
            { 41, typeof(Items.Accessory) },
            { 42, typeof(Items.Accessory) },
            { 43, typeof(Items.Accessory) },
            // 
            { 44, typeof(Items.Usable) },
            { 45, typeof(Items.Usable) },
            { 46, typeof(Items.Usable) },
            //
            { 57, typeof(Items.Housing) },
            // 
            { 58, typeof(Items.MateriaItem) },
            // 
            { 62, typeof(Items.SoulCrystal) },
            //
            { 64, typeof(Items.Housing) },
            { 65, typeof(Items.Housing) },
            { 66, typeof(Items.Housing) },
            { 67, typeof(Items.Housing) },
            { 68, typeof(Items.Housing) },
            { 69, typeof(Items.Housing) },
            { 70, typeof(Items.Housing) },
            { 71, typeof(Items.Housing) },
            { 72, typeof(Items.Housing) },
            { 73, typeof(Items.Housing) },
            { 74, typeof(Items.Housing) },
            { 75, typeof(Items.Housing) },
            { 76, typeof(Items.Housing) },
            { 77, typeof(Items.Housing) },
            { 78, typeof(Items.Housing) },
            { 79, typeof(Items.Housing) },
            { 80, typeof(Items.Housing) },
        };
        #endregion

        #region Constructor
        public InventoryItemSheet(XivCollection collection, IRelationalSheet source) : base(collection, source) { }
        #endregion

        #region Factory
        protected override Item CreateRow(Ex.Relational.IRelationalRow sourceRow) {
            var uiCategory = (ItemUICategory)sourceRow["ItemUICategory"];

            Type type;
            if (!ItemTypes.TryGetValue(uiCategory.Key, out type))
                type = DefaultItemType;
            var args = new object[] { this, sourceRow };
            var bindFlags = BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic;
            return (Item)Activator.CreateInstance(type, bindFlags, null, args, null);
        }
        #endregion
    }
}
