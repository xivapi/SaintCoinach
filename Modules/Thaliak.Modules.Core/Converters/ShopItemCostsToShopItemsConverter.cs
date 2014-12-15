using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace Thaliak.Modules.Core.Converters {
    [ValueConversion(typeof(IEnumerable<SaintCoinach.Xiv.IShopItemCost>), typeof(IEnumerable<SaintCoinach.Xiv.IShopItem>))]
    public class ShopItemCostsToShopItemsConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var asEnum = value as IEnumerable<SaintCoinach.Xiv.IShopItemCost>;
            if(asEnum == null)
                return null;

            return asEnum.Select(_ => _.ShopItem).Distinct();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
