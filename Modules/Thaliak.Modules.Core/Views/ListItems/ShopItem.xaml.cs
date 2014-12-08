using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thaliak.Modules.Core.Views.ListItems {
    /// <summary>
    /// Interaction logic for ShopItem.xaml
    /// </summary>
    [Behaviors.ViewExport("ShopItemListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class ShopItem : UserControl {
        public ShopItem() {
            InitializeComponent();
        }

        public static DependencyProperty NavigateToShopItemProperty = DependencyProperty.Register("NavigateToShopItem", typeof(bool), typeof(Recipe), new PropertyMetadata(true));
        public bool NavigateToShopItem {
            get { return (bool)GetValue(NavigateToShopItemProperty); }
            set { SetValue(NavigateToShopItemProperty, value); }
        }

        public static DependencyProperty NavigateToCostItemsProperty = DependencyProperty.Register("NavigateToCostItems", typeof(bool), typeof(Recipe), new PropertyMetadata(true));
        public bool NavigateToCostItems {
            get { return (bool)GetValue(NavigateToCostItemsProperty); }
            set { SetValue(NavigateToCostItemsProperty, value); }
        }
    }
}
