using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace Thaliak.Modules.Core.Converters {
    [ValueConversion(typeof(SaintCoinach.Xiv.Item), typeof(IEnumerable<string>))]
    public class ItemToItemAttributesConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var item = value as SaintCoinach.Xiv.Item;
            if (item == null)
                return null;

            var list = new List<string>();

            var invItem = item as SaintCoinach.Xiv.InventoryItem;
            var eqItem = item as SaintCoinach.Xiv.Items.Equipment;
            if (invItem != null)
                list.Add(string.Format("⦁ iLv {0}", invItem.ItemLevel.Key));
            if (eqItem != null)
                list.Add(string.Format("⦁ Lv. {0} {1}", eqItem.EquipmentLevel, eqItem.ClassJobCategory.Name));

            if (invItem != null) {
                if (invItem.IsUnique)
                    list.Add("⦁ Unique");
                if (invItem.IsUntradable)
                    list.Add("⦁ Untradable");
            }

            if (item.StackSize > 1)
                list.Add(string.Format("⦁ Stacks {0}", item.StackSize));

            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
