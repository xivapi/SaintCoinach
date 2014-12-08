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
    /// Interaction logic for Recipe.xaml
    /// </summary>
    [Behaviors.ViewExport("RecipeListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class Recipe : UserControl {
        public Recipe() {
            InitializeComponent();

            NavigateToRecipeItem = true;
        }

        private IServiceLocator CurrentServiceLocator { get { return ServiceLocator.Current; } }
        private IEventAggregator EventAggregator { get { return CurrentServiceLocator.GetInstance<IEventAggregator>(); } }
        public Events.NavigationRequestEvent NavigationEvent { get { return EventAggregator.GetEvent<Events.NavigationRequestEvent>(); } }

        public bool NavigateToRecipeItem { get; set; }

        #region Nav
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavToRecipe();
        }

        private void UserControl_TouchUp(object sender, TouchEventArgs e) {
            NavToRecipe();
        }

        private void NavToRecipe() {
            if (!NavigateToRecipeItem)
                return;

            var rec = DataContext as SaintCoinach.Xiv.Recipe;
            if (rec != null)
                NavigationEvent.Publish(rec.ResultItem, false);
        }
        #endregion
    }
}
