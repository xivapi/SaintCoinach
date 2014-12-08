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
    /// Interaction logic for RecipeExpanded.xaml
    /// </summary>
    [Behaviors.ViewExport("ExpandedRecipeListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class RecipeExpanded : UserControl {
        public RecipeExpanded() {
            InitializeComponent();

            NavigateToRecipeMaterials = true;
        }

        private IServiceLocator CurrentServiceLocator { get { return ServiceLocator.Current; } }
        private IEventAggregator EventAggregator { get { return CurrentServiceLocator.GetInstance<IEventAggregator>(); } }
        public Events.NavigationRequestEvent NavigationEvent { get { return EventAggregator.GetEvent<Events.NavigationRequestEvent>(); } }

        public bool NavigateToRecipeItem {
            get { return _BaseRecipe.NavigateToRecipeItem; }
            set { _BaseRecipe.NavigateToRecipeItem = value; }
        }
        public bool NavigateToRecipeMaterials { get; set; }

        #region Nav
        private void RecipeMaterial_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            NavigateToRecipeMaterial(sender);
        }

        private void RecipeMaterial_TouchUp(object sender, TouchEventArgs e) {
            NavigateToRecipeMaterial(sender);
        }

        private void NavigateToRecipeMaterial(object sender) {
            if (!NavigateToRecipeMaterials)
                return;

            var rec = ((FrameworkElement)sender).DataContext as SaintCoinach.Xiv.RecipeIngredient;
            if (rec != null)
                NavigationEvent.Publish(rec.Item, false);
        }
        #endregion
    }
}
