using System;
using System.Collections.Generic;
using System.Reflection;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Xiv.Items;

namespace SaintCoinach.Xiv.Sheets {
    public class InventoryItemSheet : XivSheet<Item> {
        #region Static

        // Types as defined in ItemUICategory
        private static readonly Type DefaultItemType = typeof(Item);

        private static readonly Dictionary<int, Type> ItemTypes = new Dictionary<int, Type> {
            // 
            {
                1, typeof(PhysicalWeapon)
            }, {
                2, typeof(PhysicalWeapon)
            }, {
                3, typeof(PhysicalWeapon)
            }, {
                4, typeof(PhysicalWeapon)
            }, {
                5, typeof(PhysicalWeapon)
            }, {
                84, typeof(PhysicalWeapon)
            },
            // 
            {
                6, typeof(MagicWeapon)
            }, {
                7, typeof(MagicWeapon)
            }, {
                8, typeof(MagicWeapon)
            }, {
                9, typeof(MagicWeapon)
            }, {
                10, typeof(MagicWeapon)
            },
            // 
            {
                11, typeof(Shield)
            },
            // 
            {
                12, typeof(CraftingTool)
            }, {
                13, typeof(CraftingTool)
            }, {
                14, typeof(CraftingTool)
            }, {
                15, typeof(CraftingTool)
            }, {
                16, typeof(CraftingTool)
            }, {
                17, typeof(CraftingTool)
            }, {
                18, typeof(CraftingTool)
            }, {
                19, typeof(CraftingTool)
            }, {
                20, typeof(CraftingTool)
            }, {
                21, typeof(CraftingTool)
            }, {
                22, typeof(CraftingTool)
            }, {
                23, typeof(CraftingTool)
            }, {
                24, typeof(CraftingTool)
            }, {
                25, typeof(CraftingTool)
            }, {
                26, typeof(CraftingTool)
            }, {
                27, typeof(CraftingTool)
            },
            // 
            {
                28, typeof(GatheringTool)
            }, {
                29, typeof(GatheringTool)
            }, {
                30, typeof(GatheringTool)
            }, {
                31, typeof(GatheringTool)
            }, {
                32, typeof(GatheringTool)
            },
            // 
            {
                34, typeof(Armour)
            }, {
                35, typeof(Armour)
            }, {
                36, typeof(Armour)
            }, {
                37, typeof(Armour)
            }, {
                38, typeof(Armour)
            }, {
                39, typeof(Armour)
            },
            // 
            {
                40, typeof(Accessory)
            }, {
                41, typeof(Accessory)
            }, {
                42, typeof(Accessory)
            }, {
                43, typeof(Accessory)
            },
            // 
            {
                44, typeof(Usable)
            }, {
                45, typeof(Usable)
            }, {
                46, typeof(Usable)
            },
            //
            {
                57, typeof(Housing)
            },
            // 
            {
                58, typeof(MateriaItem)
            },
            // 
            {
                62, typeof(SoulCrystal)
            },
            //
            {
                64, typeof(Housing)
            }, {
                65, typeof(Housing)
            }, {
                66, typeof(Housing)
            }, {
                67, typeof(Housing)
            }, {
                68, typeof(Housing)
            }, {
                69, typeof(Housing)
            }, {
                70, typeof(Housing)
            }, {
                71, typeof(Housing)
            }, {
                72, typeof(Housing)
            }, {
                73, typeof(Housing)
            }, {
                74, typeof(Housing)
            }, {
                75, typeof(Housing)
            }, {
                76, typeof(Housing)
            }, {
                77, typeof(Housing)
            }, {
                78, typeof(Housing)
            }, {
                79, typeof(Housing)
            }, {
                80, typeof(Housing)
            }
        };

        #endregion

        #region Constructors

        public InventoryItemSheet(XivCollection collection, IRelationalSheet source) : base(collection, source) { }

        #endregion

        #region Factory

        protected override Item CreateRow(IRelationalRow sourceRow) {
            var uiCategory = (ItemUICategory)sourceRow["ItemUICategory"];

            Type type;
            if (!ItemTypes.TryGetValue(uiCategory.Key, out type))
                type = DefaultItemType;
            var args = new object[] {
                this, sourceRow
            };

            const BindingFlags ActivatorBindFlags =
                BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public
                | BindingFlags.NonPublic;
            return (Item)Activator.CreateInstance(type, ActivatorBindFlags, null, args, null);
        }

        #endregion
    }
}
