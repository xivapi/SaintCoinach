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
using SaintCoinach.Xiv;
using System.Collections;

namespace Godbert.Controls {
    public class RawDataGrid : DataGrid {
        #region DependencyProperties
        public static readonly DependencyProperty SheetProperty = DependencyProperty.Register("Sheet", typeof(IRelationalSheet), typeof(RawDataGrid), new PropertyMetadata(OnSheetChanged));
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(RawDataGrid), new PropertyMetadata(OnFilterChanged));

        public IRelationalSheet Sheet {
            get { return (IRelationalSheet)GetValue(SheetProperty); }
            set { SetValue(SheetProperty, value); }
        }

        public string Filter {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
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

                var source = new RawDataItemsSource(newValue);
                if (Filter != null)
                    source.Filter = o => FilterMatch(o, Filter);
                this.ItemsSource = source;
            }
            else
                this.ItemsSource = null;
        }

        private static void OnFilterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            ((RawDataGrid)o).OnFilterChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnFilterChanged(string oldValue, string newValue) {
            var src = this.ItemsSource as RawDataItemsSource;
            if (src == null)
                return;

            if (string.IsNullOrWhiteSpace(newValue))
                src.Filter = null;
            else
                src.Filter = o => FilterMatch(o, newValue);
        }

        private static bool FilterMatch(object rowObj, string value) {
            var row = rowObj as IXivRow;
            if (row == null)
                return false;

            if (row.Key.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            foreach (var col in row.Sheet.Header.Columns) {
                var cellObj = row[col.Index];
                if (cellObj == null)
                    continue;

                var dictObj = cellObj as IDictionary<int, object>;
                if (dictObj != null)
                    return dictObj.Values.Any(v => v.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0);
                if (cellObj.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
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

            if (this.SelectedItem != null)
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
