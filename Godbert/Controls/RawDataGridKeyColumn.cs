using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SaintCoinach.Ex.Relational;

namespace Godbert.Controls {
    public class RawDataGridKeyColumn : DataGridTextColumn, IRawDataColumn {
        public RawDataGridKeyColumn(string keyPath) {

            this.Header = "Key";
            this.IsReadOnly = true;
            this.Binding = new Binding(keyPath) { Mode = BindingMode.OneWay };
        }

        #region IRawDataColumn Members
        public RelationalColumn Column => null;

        public IComparer<object> GetComparer(System.ComponentModel.ListSortDirection direction) {
            return new Comparer {
                Direction = direction,
            };
        }



        private class Comparer : IComparer<object> {
            public ListSortDirection Direction;

            #region IComparer<object> Members

            public int Compare(object x, object y) {
                if (Direction == ListSortDirection.Descending)
                    return -InnerCompare(x, y);
                return InnerCompare(x, y);
            }

            private int InnerCompare(object x, object y) {
                var rx = x as IRelationalRow;
                var ry = y as IRelationalRow;
                if (rx == ry)
                    return 0;
                if (rx == null)
                    return -1;
                if (ry == null)
                    return 1;

                return rx.Key.CompareTo(ry.Key);
            }

            #endregion
        }
        #endregion
    }
}
