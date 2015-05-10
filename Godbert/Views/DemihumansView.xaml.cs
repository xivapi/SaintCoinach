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

namespace Godbert.Views {
    /// <summary>
    /// Interaction logic for DemihumansView.xaml
    /// </summary>
    public partial class DemihumansView : UserControl {
        public DemihumansView() {
            InitializeComponent();
        }

        private void DemihumanTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var vm = (ViewModels.DemihumanViewModel)DataContext;
            vm.SelectedEntry = DemihumanTreeView.SelectedItem;
        }
    }
}
