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

using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Modules.Core.Views.ListItems {
    /// <summary>
    /// Interaction logic for ShopItem.xaml
    /// </summary>
    [Behaviors.ViewExport("ShopItemListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class ShopItem : UserControl {
        public ShopItem() {
            InitializeComponent();

            NavigateToCostItems = true;
            NavigateToShopItems = true;
        }

        private IServiceLocator CurrentServiceLocator { get { return ServiceLocator.Current; } }
        private IEventAggregator EventAggregator { get { return CurrentServiceLocator.GetInstance<IEventAggregator>(); } }
        public Events.NavigationRequestEvent NavigationEvent { get { return EventAggregator.GetEvent<Events.NavigationRequestEvent>(); } }

        public bool NavigateToShopItems { get; set; }
        public bool NavigateToCostItems { get; set; }

        #region ShopItem
        private void ShopItem_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavigateToShopItem(sender);
        }

        private void ShopItem_TouchUp(object sender, TouchEventArgs e) {
            NavigateToShopItem(sender);
        }

        private void NavigateToShopItem(object sender) {
            if (!NavigateToShopItems)
                return;

            var shopItem = ((FrameworkElement)sender).DataContext as SaintCoinach.Xiv.IShopItem;
            if (shopItem != null)
                NavigationEvent.Publish(shopItem.Item, false);
        }
        #endregion

        #region ShopItemCost
        private void ShopItemCost_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavigateToShopItemCost(sender);
        }

        private void ShopItemCost_TouchUp(object sender, TouchEventArgs e) {
            NavigateToShopItemCost(sender);
        }

        private void NavigateToShopItemCost(object sender) {
            if (!NavigateToCostItems)
                return;

            var shopItemCost = ((FrameworkElement)sender).DataContext as SaintCoinach.Xiv.IShopItemCost;
            if (shopItemCost != null)
                NavigationEvent.Publish(shopItemCost.Item, false);
        }
        #endregion
    }
}
