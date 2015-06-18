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
                1, typeof(PhysicalWeapon)   // PGL
            }, {
                2, typeof(PhysicalWeapon)   // GLD
            }, {
                3, typeof(PhysicalWeapon)   // MRD
            }, {
                4, typeof(PhysicalWeapon)   // ARC
            }, {
                5, typeof(PhysicalWeapon)   // LNC
            }, {
                84, typeof(PhysicalWeapon)  // ROG
            }, {
                87, typeof(PhysicalWeapon)  // DRK
            }, {
                88, typeof(PhysicalWeapon)  // MCH
            },
            // 
            {
                6, typeof(MagicWeapon)      // THM (one-handed)
            }, {
                7, typeof(MagicWeapon)      // THM (two-handed)
            }, {
                8, typeof(MagicWeapon)      // CNJ (one-handed)
            }, {
                9, typeof(MagicWeapon)      // CNJ (two-handed)
            }, {
                10, typeof(MagicWeapon)     // ACN
            }, {
                89, typeof(PhysicalWeapon)  // AST
            },
            // 
            {
                11, typeof(Shield)
            },
            // 
            {
                12, typeof(CraftingTool)    // CRP (main)
            }, {
                13, typeof(CraftingTool)    // CRP (off)
            }, {
                14, typeof(CraftingTool)    // BSM
            }, {
                15, typeof(CraftingTool)    // BSM
            }, {
                16, typeof(CraftingTool)    // ARM
            }, {
                17, typeof(CraftingTool)    // ARM
            }, {
                18, typeof(CraftingTool)    // GSM
            }, {
                19, typeof(CraftingTool)    // GSM
            }, {
                20, typeof(CraftingTool)    // LTW
            }, {
                21, typeof(CraftingTool)    // LTW
            }, {
                22, typeof(CraftingTool)    // WVR
            }, {
                23, typeof(CraftingTool)    // WVR
            }, {
                24, typeof(CraftingTool)    // ALC
            }, {
                25, typeof(CraftingTool)    // ALC
            }, {
                26, typeof(CraftingTool)    // CUL
            }, {
                27, typeof(CraftingTool)    // CUL
            },
            // 
            {
                28, typeof(GatheringTool)   // MIN (main)
            }, {
                29, typeof(GatheringTool)   // MIN (sub)
            }, {
                30, typeof(GatheringTool)   // BTN
            }, {
                31, typeof(GatheringTool)   // BTN
            }, {
                32, typeof(GatheringTool)   // FSH
            },
            // 
            {
                34, typeof(Armour)          // Head
            }, {
                35, typeof(Armour)          // Body
            }, {
                36, typeof(Armour)          // Legs
            }, {
                37, typeof(Armour)          // Hands
            }, {
                38, typeof(Armour)          // Feet
            }, {
                39, typeof(Armour)          // Waist
            },
            // 
            {
                40, typeof(Accessory)       // Neck
            }, {
                41, typeof(Accessory)       // Ears
            }, {
                42, typeof(Accessory)       // Wrists
            }, {
                43, typeof(Accessory)       // Fingers
            },
            // 
            {
                44, typeof(Usable)          // Medicine
            }, {
                45, typeof(Usable)          // Ingredient
            }, {
                46, typeof(Usable)          // Meal
            },
            //
            {
                57, typeof(Housing)         // Furnishing
            },
            // 
            {
                58, typeof(MateriaItem)     // Materia
            },
            // 
            {
                62, typeof(SoulCrystal)     // Soul Crystal
            },
            //
            {
                64, typeof(Housing)         // Construction Permit
            }, {
                65, typeof(Housing)         // Root
            }, {
                66, typeof(Housing)         // Exterior Wall
            }, {
                67, typeof(Housing)         // Window
            }, {
                68, typeof(Housing)         // Door
            }, {
                69, typeof(Housing)         // Roof Decoration
            }, {
                70, typeof(Housing)         // Exterior Wall Decoration
            }, {
                71, typeof(Housing)         // Placard
            }, {
                72, typeof(Housing)         // Fence
            }, {
                73, typeof(Housing)         // Interior Wall
            }, {
                74, typeof(Housing)         // Flooring
            }, {
                75, typeof(Housing)         // Ceiling Light
            }, {
                76, typeof(Housing)         // Outdoor Furnishing
            }, {
                77, typeof(Housing)         // Table
            }, {
                78, typeof(Housing)         // Tabletop
            }, {
                79, typeof(Housing)         // Wall-mounted
            }, {
                80, typeof(Housing)         // Rug
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
