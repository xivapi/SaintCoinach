using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search {
    using Behaviors;
    using Interfaces;

    [SearchFunctionExport(Function = "slot")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EquipSlotQuery : ISearchQuery {
        [Import]
        private Xiv.XivCollection Data { get; set; }

        public Xiv.EquipSlot EquipSlot { get; set; }

        #region ISearchQuery Members

        public IEnumerable<Type> MatchedTypes {
            get { yield return typeof(Xiv.Items.Equipment); }
        }

        public bool IsMatch(object value) {
            if (EquipSlot == null)
                return false;
            var asEq = value as Xiv.Items.Equipment;
            if (asEq == null)
                return false;
            return asEq.EquipSlotCategory.PossibleSlots.Contains(EquipSlot);
        }

        public void Set(string args) {
            int key;
            if (int.TryParse(args, out key))
                EquipSlot = Data.EquipSlots.FirstOrDefault(_ => _.Key == key);
            else
                EquipSlot = Data.EquipSlots.FirstOrDefault(_ => string.Equals(_.Name, args, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        public override string ToString() {
            return string.Format("slot:{0}", EquipSlot);
        }
    }
}
