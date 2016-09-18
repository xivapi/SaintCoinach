using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.ComponentModel;
using System.Windows.Input;
using SaintCoinach.Ex.Relational;
using Godbert.ViewModels;

namespace Godbert.Controls {
    public class RawDataGrid : DataGrid {
        #region DependencyProperties
        public static readonly DependencyProperty SheetProperty = DependencyProperty.Register("Sheet", typeof(IRelationalSheet), typeof(RawDataGrid), new PropertyMetadata(OnSheetChanged));

        public IRelationalSheet Sheet {
            get { return (IRelationalSheet)GetValue(SheetProperty); }
            set { SetValue(SheetProperty, value); }
        }

        private static void OnSheetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            ((RawDataGrid)o).OnSheetChanged((IRelationalSheet)e.OldValue, (IRelationalSheet)e.NewValue);
        }

        protected virtual void OnSheetChanged(IRelationalSheet oldValue, IRelationalSheet newValue) {
            this.Columns.Clear();

            if (newValue != null)
            {
                Columns.Add(new RawDataGridKeyColumn() { CanUserSort = true });

                foreach (var col in newValue.Header.Columns)
                    Columns.Add(ColumnFactory.Create(col));

                this.ItemsSource = new RawDataItemsSource(newValue);
            }
            else
                this.ItemsSource = null;

        }
        #endregion

        protected override void OnSorting(DataGridSortingEventArgs eventArgs) {
            eventArgs.Handled = true;

            var src = this.ItemsSource as RawDataItemsSource;
            var col = eventArgs.Column as IRawDataColumn;

            if (src == null || col == null)
                return;

            var nextDir = (eventArgs.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            eventArgs.Column.SortDirection = nextDir;

            var cmp = col.GetComparer(nextDir);
            src.Comparer = cmp;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e) {
            var source = e.OriginalSource as FrameworkElement;
            if (source == null)
                return;

            var cell = FindParent<DataGridCell>(source);
            if (cell == null)
                return;

            var navigatable = cell.Column as INavigatable;
            if (navigatable == null)
                return;

            var mainWindow = FindParent<MainWindow>(cell);
            if (mainWindow == null)
                return;

            var mainViewModel = mainWindow.DataContext as MainViewModel;
            if (mainViewModel == null)
                return;

            var row = navigatable.OnNavigate(cell, e);
            if (row == null)
                return;

            mainViewModel.Data.SelectedSheetName = row.Sheet.Name;

            this.SelectedItem = row;
            this.UpdateLayout();
            this.ScrollIntoView(this.SelectedItem);
        }

        private static T FindParent<T>(DependencyObject child) where T : class {
            var parent = System.Windows.Media.VisualTreeHelper.GetParent(child);
            if (parent == null)
                return null;

            var parentAsType = parent as T;
            if (parentAsType != null)
                return parentAsType;

            return FindParent<T>(parent);
        }
    }
}
