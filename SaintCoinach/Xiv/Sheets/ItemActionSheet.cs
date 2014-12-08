using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Sheets {
    using Ex.Relational;
    using IA = ItemActions;

    public class ItemActionSheet : XivSheet<ItemAction> {
        /* TODO:
         * - 843: Friendly status (company manuals / regain feather)
         * - 1058: Sphere scroll
         * - 1326: Fantasia
         * - 1767: TpRecovery
         * - 2120: Decorative eggs
         * - 2136: Master books
         */
        #region Static
        private static Type DefaultItemActionType = typeof(ItemAction);
        private static Dictionary<int, Type> ItemActionTypes = new Dictionary<int, Type> {
            { 0, typeof(ItemAction) },      // Just so it stops giving messages.
            { 841, typeof(IA.HostileEffect) },
            { 842, typeof(IA.StatusRemoval) },
            { 843, typeof(IA.FriendlyEffect) },
            { 844, typeof(IA.Food) },
            { 845, typeof(IA.Food) },
            { 846, typeof(IA.Enhancement) },
            { 847, typeof(IA.HpRecovery) },
            { 848, typeof(IA.MpRecovery) },
            { 849, typeof(IA.HpMpRecovery) },
            { 850, typeof(IA.Sanction) },
            { 851, typeof(IA.BuddySummon) },
            { 852, typeof(IA.Fireworks) },
            { 853, typeof(IA.CompanionUnlock) },
            { 1013, typeof(IA.BuddyEquipUnlock) },
            { 1053, typeof(IA.Raise) },
            { 1054, typeof(IA.AttributeReset) },
            { 1055, typeof(IA.GpRecovery) },
            { 1058, typeof(IA.SphereScroll) },
            { 1322, typeof(IA.MountUnlock) },
            { 1326, typeof(IA.Fantasia) },
            { 1767, typeof(IA.TpRecovery) },
            { 2120, typeof(IA.ItemRoulette) },
            { 2136, typeof(IA.RecipeBookUnlock) },
            { 2483, typeof(IA.ChocoboFeed) },
        };
        #endregion

        #region Constructor
        public ItemActionSheet(XivCollection collection, IRelationalSheet source) : base(collection, source) { }
        #endregion

        #region Factory
        protected override ItemAction CreateRow(Ex.Relational.IRelationalRow sourceRow) {
            var typeKey = Convert.ToInt32(sourceRow["Type"]);

            Type type;
            if (!ItemActionTypes.TryGetValue(typeKey, out type)) {
                type = DefaultItemActionType;
                System.Diagnostics.Trace.WriteLine(string.Format("Unhandled item action type '{0}'.", typeKey));
            }
            var args = new object[] { this, sourceRow };
            var bindFlags = BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic;
            return (ItemAction)Activator.CreateInstance(type, bindFlags, null, args, null);
        }
        #endregion
    }
}
