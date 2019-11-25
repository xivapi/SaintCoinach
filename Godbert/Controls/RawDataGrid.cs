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
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using SaintCoinach.Ex;

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

        public static bool[] ColumnSetToRaw;

        protected virtual void OnSheetChanged(IRelationalSheet oldValue, IRelationalSheet newValue) {
            this.Columns.Clear();

            if (newValue != null)
            {
                var keyPath = newValue.Header.Variant == 1 ? "Key" : "FullKey";

                //prevent multiple enumeration
                var columns = newValue.Header.Columns.ToList();

                if (Settings.Default.SortByOffsets)
                    columns.Sort((x, y) => x.Offset.CompareTo(y.Offset));

                ColumnSetToRaw = new bool[columns.Count];

                Columns.Add(new RawDataGridKeyColumn(keyPath) { CanUserSort = true });

                foreach (var col in columns)
                    Columns.Add(ColumnFactory.Create(col));

                var source = new RawDataItemsSource(newValue);
                if (!string.IsNullOrWhiteSpace(Filter))
                    source.Filter = Filter;
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
                src.Filter = newValue;
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

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            if (e.RightButton == MouseButtonState.Pressed)
            {
                var cell = GetClickedHeader(e);
                if (cell == null)
                    return;

                if (cell.Column is RawDataGridImageColumn ||
                    cell.Column is RawDataGridTextColumn ||
                    cell.Column is RawDataGridColorColumn)
                {
                    ColumnFactory.ForceRaw = !ColumnFactory.ForceRaw;
                    Items.Refresh();
                    e.Handled = true;
                }
            }

            if (e.MiddleButton == MouseButtonState.Pressed) 
            {

                var cellHeader = GetClickedHeader(e);
                if (cellHeader != null)
                {

                    if (cellHeader.Column is RawDataGridImageColumn ||
                        cellHeader.Column is RawDataGridTextColumn ||
                        cellHeader.Column is RawDataGridColorColumn)
                    {

                        var columnIndex = ((IRawDataColumn) cellHeader.Column).Column.Index;
                        ColumnSetToRaw[columnIndex] = !ColumnSetToRaw[columnIndex];
                        Items.Refresh();
                        e.Handled = true;
                    }


                    return;
                }

                var cell = GetClickedCell(e);
                if (cell == null)
                    return;

                var dataView = WpfHelper.FindParent<Views.DataView>(cell);
                if (dataView == null)
                    return;

                var dataViewModel = dataView.DataContext as DataViewModel;
                if (dataViewModel == null)
                    return;

                var row = cell.DataContext as IRow;
                if (row == null)
                    return;

                var dataColumn = cell.Column as IRawDataColumn;
                if (dataColumn == null)
                    return;

                var bookmark = new BookmarkViewModel() {
                    SheetName = dataViewModel.SelectedSheetName,
                    Key = row.Key,
                    RowDefault = row.ToString(),
                    ColumnName = dataColumn.Column?.Name,
                    ColumnIndex = dataColumn.Column?.Index
                };

                if (dataViewModel.Bookmarks.Contains(bookmark)) {
                    dataViewModel.Bookmarks.Remove(bookmark);
                    return;
                }

                dataViewModel.Bookmarks.Add(bookmark);
            }
        }

        private DataGridColumnHeader GetClickedHeader(MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) &&
            !(dep is DataGridCell) &&
            !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            return dep as DataGridColumnHeader;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e) {
            var cell = GetClickedCell(e);
            if (cell == null)
                return;

            var navigatable = cell.Column as INavigatable;
            if (navigatable == null)
                return;

            var mainWindow = WpfHelper.FindParent<MainWindow>(cell);
            if (mainWindow == null)
                return;

            var mainViewModel = mainWindow.DataContext as MainViewModel;
            if (mainViewModel == null)
                return;

            var row = navigatable.OnNavigate(cell, e);
            if (row == null)
                return;

            mainViewModel.Data.SelectedSheetName = row.Sheet.Name;

            SelectRow(row, null);
        }

        private static DataGridCell GetClickedCell(MouseButtonEventArgs e) {
            if (e.OriginalSource is FrameworkElement source)
                return WpfHelper.FindParent<DataGridCell>(source);

            return null;
        }

        public void SelectRow(IRow row, int? columnIndex) {
            this.SelectedItem = row;
            this.UpdateLayout();

            if (this.SelectedItem == null)
                return;

            DataGridColumn selectedColumn = null;
            if (columnIndex != null)
                selectedColumn = (DataGridColumn)this.Columns.OfType<IRawDataColumn>().FirstOrDefault(c => c.Column?.Index == columnIndex);

            this.ScrollIntoView(this.SelectedItem, selectedColumn);
        }
    }
}
