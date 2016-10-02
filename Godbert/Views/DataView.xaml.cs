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
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl {
        public DataView() {
            InitializeComponent();
        }

        public void RowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row == null)
                return;

            // Ensure row heights do not fluctuate as virtualized columns are
            // loaded/unloaded.
            if (row.MinHeight < e.NewSize.Height)
                row.MinHeight = e.NewSize.Height;
        }
    }
}
