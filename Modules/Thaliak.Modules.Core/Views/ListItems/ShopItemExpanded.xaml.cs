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
    /// Interaction logic for ShopItemExpanded.xaml
    /// </summary>
    [Behaviors.ViewExport("ExpandedShopItemListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class ShopItemExpanded : UserControl {
        public ShopItemExpanded() {
            InitializeComponent();
        }

        public bool NavigateToShopItem {
            get { return _BaseShopItem.NavigateToShopItem; }
            set { _BaseShopItem.NavigateToShopItem = value; }
        }
        public bool NavigateToCostItems {
            get { return _BaseShopItem.NavigateToCostItems; }
            set { _BaseShopItem.NavigateToCostItems = value; }
        }
        public static DependencyProperty NavigateToShopsProperty = DependencyProperty.Register("NavigateToShops", typeof(bool), typeof(Recipe), new PropertyMetadata(true));
        public bool NavigateToShops {
            get { return (bool)GetValue(NavigateToShopsProperty); }
            set { SetValue(NavigateToShopsProperty, value); }
        }

        public static DependencyProperty NavigateToENpcsProperty = DependencyProperty.Register("NavigateToENpcs", typeof(bool), typeof(Recipe), new PropertyMetadata(true));
        public bool NavigateToENpcs {
            get { return (bool)GetValue(NavigateToENpcsProperty); }
            set { SetValue(NavigateToENpcsProperty, value); }
        }
    }
}
