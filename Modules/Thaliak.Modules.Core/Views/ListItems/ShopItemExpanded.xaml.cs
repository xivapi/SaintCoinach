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
    /// Interaction logic for ShopItemExpanded.xaml
    /// </summary>
    [Behaviors.ViewExport("ExpandedShopItemListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class ShopItemExpanded : UserControl {
        public ShopItemExpanded() {
            InitializeComponent();

            NavigateToShops = true;
            NavigateToENpcs = true;
        }

        private IServiceLocator CurrentServiceLocator { get { return ServiceLocator.Current; } }
        private IEventAggregator EventAggregator { get { return CurrentServiceLocator.GetInstance<IEventAggregator>(); } }
        public Events.NavigationRequestEvent NavigationEvent { get { return EventAggregator.GetEvent<Events.NavigationRequestEvent>(); } }

        public bool NavigateToShopItems {
            get { return _BaseShopItem.NavigateToShopItems; }
            set { _BaseShopItem.NavigateToShopItems = value; }
        }
        public bool NavigateToCostItems {
            get { return _BaseShopItem.NavigateToCostItems; }
            set { _BaseShopItem.NavigateToCostItems = value; }
        }
        public bool NavigateToShops { get; set; }
        public bool NavigateToENpcs { get; set; }

        #region ENpc
        private void ENpc_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavigateToENpc(sender);
        }

        private void ENpc_TouchUp(object sender, TouchEventArgs e) {
            NavigateToENpc(sender);
        }

        private void NavigateToENpc(object sender) {
            if (!NavigateToENpcs)
                return;

            var enpc = ((FrameworkElement)sender).DataContext as SaintCoinach.Xiv.ENpc;
            if (enpc != null)
                NavigationEvent.Publish(enpc, false);
        }
        #endregion

        #region ENpc
        private void Shop_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavigateToShop(sender);
        }

        private void Shop_TouchUp(object sender, TouchEventArgs e) {
            NavigateToShop(sender);
        }

        private void NavigateToShop(object sender) {
            if (!NavigateToShops)
                return;

            var shop = ((FrameworkElement)sender).DataContext as SaintCoinach.Xiv.IShop;
            if (shop != null)
                NavigationEvent.Publish(shop, false);
        }
        #endregion
    }
}
