using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.ComponentModel;
using SaintCoinach.Ex.Relational;

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

            if (newValue != null) {
                Columns.Add(new RawDataGridKeyColumn() { CanUserSort = true });

                foreach (var col in newValue.Header.Columns)
                    Columns.Add(ColumnFactory.Create(col));
            }

            this.ItemsSource = new RawDataItemsSource(newValue);
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
    }
}
