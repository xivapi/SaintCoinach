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

using System.ComponentModel.Composition;

namespace Thaliak.Views {
    /// <summary>
    /// Interaction logic for NoSearchResultsView.xaml
    /// </summary>
    [Export("NoSearchResultsView")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class NoSearchResultsView : UserControl {
        public NoSearchResultsView() {
            InitializeComponent();
        }

        [Import]
        public ViewModels.SearchResultsViewModel ViewModel {
            set { DataContext = value; }
        }
    }
}
