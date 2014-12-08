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
    /// Interaction logic for Item.xaml
    /// </summary>
    [Behaviors.ViewExport("ItemListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class Item : UserControl {
        public Item() {
            InitializeComponent();

            NavigateToItem = true;
        }

        private IServiceLocator CurrentServiceLocator { get { return ServiceLocator.Current; } }
        private IEventAggregator EventAggregator { get { return CurrentServiceLocator.GetInstance<IEventAggregator>(); } }
        public Events.NavigationRequestEvent NavigationEvent { get { return EventAggregator.GetEvent<Events.NavigationRequestEvent>(); } }

        public bool NavigateToItem { get; set; }

        #region Nav
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavToItem();
        }

        private void UserControl_TouchUp(object sender, TouchEventArgs e) {
            NavToItem();
        }

        private void NavToItem() {
            if (!NavigateToItem)
                return;

            var target = DataContext;
            if (target != null)
                NavigationEvent.Publish(target, false);
        }
        #endregion
    }
}
