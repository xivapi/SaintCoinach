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
    /// Interaction logic for Recipe.xaml
    /// </summary>
    [Behaviors.ViewExport("RecipeListItemView")]
    [System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public partial class Recipe : UserControl {
        public Recipe() {
            InitializeComponent();
        }

        public static DependencyProperty NavigateToRecipeItemProperty = DependencyProperty.Register("NavigateToRecipeItem", typeof(bool), typeof(Recipe), new PropertyMetadata(true));
        public bool NavigateToRecipeItem {
            get { return (bool)GetValue(NavigateToRecipeItemProperty); }
            set { SetValue(NavigateToRecipeItemProperty, value); }
        }
    }
}
