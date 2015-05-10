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
    /// Interaction logic for MonstersView.xaml
    /// </summary>
    public partial class MonstersView : UserControl {
        public MonstersView() {
            InitializeComponent();
        }
        private void MonsterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var vm = (ViewModels.MonstersViewModel)DataContext;
            vm.SelectedEntry = MonsterTreeView.SelectedItem;
        }
    }
}
