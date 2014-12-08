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
    /// Interaction logic for RecipeExpanded.xaml
    /// </summary>
    [Behaviors.ViewExport("ExpandedRecipeListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class RecipeExpanded : UserControl {
        public RecipeExpanded() {
            InitializeComponent();
        }

        public bool NavigateToRecipeItem {
            get { return _BaseRecipe.NavigateToRecipeItem; }
            set { _BaseRecipe.NavigateToRecipeItem = value; }
        }
        public static DependencyProperty NavigateToRecipeMaterialsProperty = DependencyProperty.Register("NavigateToRecipeMaterials", typeof(bool), typeof(Recipe), new PropertyMetadata(true));
        public bool NavigateToRecipeMaterials {
            get { return (bool)GetValue(NavigateToRecipeMaterialsProperty); }
            set { SetValue(NavigateToRecipeMaterialsProperty, value); }
        }
    }
}
