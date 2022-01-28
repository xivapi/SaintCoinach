using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Sheets {
    using IA = ItemActions;

    public class ItemActionSheet : XivSheet<ItemAction> {
        #region Static

        private static readonly Type DefaultItemActionType = typeof(ItemAction);

        private static readonly Dictionary<int, Type> ItemActionTypes = new Dictionary<int, Type> {
            {
                // Just so it stops giving messages.
                0, typeof(ItemAction)
            }, {
                816, typeof(IA.CompanyEffect)
            }, {
                841, typeof(IA.HostileEffect)
            }, {
                842, typeof(IA.StatusRemoval)
            }, {
                843, typeof(IA.FriendlyEffect)
            }, {
                844, typeof(IA.Food)
            }, {
                845, typeof(IA.Food)
            }, {
                846, typeof(IA.Enhancement)
            }, {
                847, typeof(IA.HpRecovery)
            }, {
                848, typeof(IA.MpRecovery)
            }, {
                849, typeof(IA.HpMpRecovery)
            }, {
                850, typeof(IA.Sanction)
            }, {
                851, typeof(IA.BuddySummon)
            }, {
                852, typeof(IA.Fireworks)
            }, {
                853, typeof(IA.CompanionUnlock)
            }, {
                944, typeof(IA.MgpCard)
            }, {
                1013, typeof(IA.BuddyEquipUnlock)
            }, {
                1053, typeof(IA.Raise)
            }, {
                1054, typeof(IA.AttributeReset)
            }, {
                1055, typeof(IA.GpRecovery)
            }, {
                1058, typeof(IA.SphereScroll)
            }, {
                1322, typeof(IA.MountUnlock)
            }, {
                1326, typeof(IA.Fantasia)
            }, {
                1767, typeof(IA.TpRecovery)
            }, {
                2120, typeof(IA.ItemRoulette)
            }, {
                2136, typeof(IA.RecipeBookUnlock)
            }, {
                2483, typeof(IA.ChocoboFeed)
            }, {
                2633, typeof(IA.CustomizeUnlock)
            }, {
                2634, typeof(IA.ChocoboActionReset)
            }, {
                2645, typeof(IA.RealmRebornRed)
            }, {
                2894, typeof(IA.EternityRing)
            }, {
                3237, typeof(IA.DesynthSkillReset)
            }, {
                3240, typeof(IA.EternalBondInvitation)
            }, {
                3292, typeof(IA.ChocoboLevelCapIncrease)
            }, {
                3357, typeof(IA.TripleTriadCardUnlock)
            }, {
                3800, typeof(IA.MgpCard)
            }, {
                4107, typeof(IA.FolkloreBook)
            }, {
                4647, typeof(IA.EquipmentCoffer)
            }, {
                5136, typeof(IA.Heavenscracker)
            }, {
                5564, typeof(IA.AdventureBook)
            }, {
                5845, typeof(IA.OrchestrionRollUnlock)
            }, {
                8667, typeof(IA.SustainPotion)
            }, {
                9343, typeof(IA.FateContentAction)
            }, { 
                9994, typeof(IA.TeleportationTicket)
            }, { 
                18083, typeof(IA.AchievementScroll)
            }, {
                19743, typeof(IA.FieldNoteUnlock)
            }, {
                20086, typeof(IA.OrnamentUnlock)
            }, {
                25174, typeof(IA.AdventureBook)
            }, {
                25183, typeof(IA.OrchestrionRollUnlock)
            }
        };

        #endregion

        #region Constructors

        public ItemActionSheet(XivCollection collection, IRelationalSheet source) : base(collection, source) { }

        #endregion

        #region Factory

        protected override ItemAction CreateRow(IRelationalRow sourceRow) {
            var typeKey = Convert.ToInt32(sourceRow["Type"]);

            if (!ItemActionTypes.TryGetValue(typeKey, out var type)) {
                type = DefaultItemActionType;
                Trace.WriteLine(string.Format("Unhandled item action type '{0}'.", typeKey));
            }
            var args = new object[] {
                this, sourceRow
            };
            const BindingFlags ActivatorBindFlags =
                BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public
                | BindingFlags.NonPublic;
            return (ItemAction)Activator.CreateInstance(type, ActivatorBindFlags, null, args, null);
        }

        #endregion
    }
}
