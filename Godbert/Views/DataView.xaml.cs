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
using Godbert.ViewModels;

namespace Godbert.Views {
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl {
        public DataView() {
            InitializeComponent();

            DataContextChanged += DataView_DataContextChanged;
        }

        private void DataView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var viewModel = (DataViewModel)DataContext;
            viewModel.Parent.DataGridChanged += MainViewModel_DataGridChanged;
        }

        private void MainViewModel_DataGridChanged(object sender, EventArgs e) {
            // Hacky contents refresh.
            var sheet = _DataGrid.Sheet;
            _DataGrid.Sheet = null;
            _DataGrid.Sheet = sheet;
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

        private void _BookmarksList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var bookmark = e.AddedItems.OfType<BookmarkViewModel>().FirstOrDefault();
            if (bookmark == null)
                return;

            var mainWindow = WpfHelper.FindParent<MainWindow>(this);
            if (mainWindow == null)
                return;

            var mainViewModel = mainWindow.DataContext as MainViewModel;
            if (mainViewModel == null)
                return;

            mainViewModel.Data.SelectedSheetName = bookmark.SheetName;
            var row = mainViewModel.Data.SelectedSheet[bookmark.Key];
            _DataGrid.SelectRow(row, bookmark.ColumnIndex);

            // Remove selection.
            _BookmarksList.SelectedIndex = -1;
        }

        private void BookmarkListItem_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.MiddleButton == MouseButtonState.Pressed) {
                var source = e.OriginalSource as FrameworkElement;
                if (source == null)
                    return;

                var listItem = WpfHelper.FindParent<ListBoxItem>(source);
                var bookmark = listItem?.DataContext as BookmarkViewModel;
                if (bookmark == null)
                    return;

                var dataViewModel = (DataViewModel)DataContext;
                dataViewModel.Bookmarks.Remove(bookmark);
            }
        }
    }
}
