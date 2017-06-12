using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Ex;

namespace Godbert.Controls {
    public class RawDataGridColorColumn : DataGridBoundColumn, IRawDataColumn {
        private RelationalColumn _Column;
        private ContextMenu _ContextMenu;

        public RawDataGridColorColumn(RelationalColumn column) {
            _Column = column;

            _ContextMenu = new ContextMenu();
            var mi = new MenuItem {
                Header = "Copy",
            };
            mi.Click += OnCopyClick;
            _ContextMenu.Items.Add(mi);
        }
        void OnCopyClick(object sender, RoutedEventArgs e) {
            var fe = sender as FrameworkElement;
            if (fe == null)
                return;

            var ctx = fe.DataContext;
            if (!(ctx is System.Drawing.Color))
                return;

            var src = (System.Drawing.Color)ctx;
            System.Windows.Clipboard.SetDataObject(string.Format("#{0:X8}", src.ToArgb()));
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem) {
            throw new NotImplementedException();
        }
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem) {
            Border obj = cell != null ? cell.Content as Border : null;
            if (obj == null) {
                obj = new Border {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
                obj.SetBinding(Border.BackgroundProperty, new Binding {
                    Converter = ColorConverterInstance
                });
                obj.ContextMenu = _ContextMenu;
            }

            var bind = Binding;
            if (bind == null)
                BindingOperations.ClearBinding(obj, Border.DataContextProperty);
            else
                BindingOperations.SetBinding(obj, Border.DataContextProperty, bind);

            return obj;
        }


        private static readonly ColorConverter ColorConverterInstance = new ColorConverter();
        private class ColorConverter : System.Windows.Data.IValueConverter {

            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
                if (!(value is System.Drawing.Color))
                    return null;

                var src = (System.Drawing.Color)value;
                return new SolidColorBrush(System.Windows.Media.Color.FromArgb(src.A, src.R, src.G, src.B));
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
                throw new NotImplementedException();
            }

            #endregion
        }

        #region IRawDataColumn Members
        public RelationalColumn Column => _Column;

        public IComparer<object> GetComparer(ListSortDirection direction) {
            return new Comparer {
                Column = _Column,
                Direction = direction,
            };
        }


        private class Comparer : IComparer<object> {
            public ListSortDirection Direction;
            public RelationalColumn Column;

            #region IComparer<object> Members

            public int Compare(object x, object y) {
                if (Direction == ListSortDirection.Descending)
                    return -InnerCompare(x, y);
                return InnerCompare(x, y);
            }

            private int InnerCompare(object x, object y) {
                var rx = x as IRow;
                var ry = y as IRow;
                if (rx == ry)
                    return 0;
                if (rx == null)
                    return -1;
                if (ry == null)
                    return 1;

                var vx = rx[Column.Index];
                var vy = ry[Column.Index];

                if (vx == vy)
                    return 0;
                if (!(vx is System.Drawing.Color))
                    return -1;
                if (!(vy is System.Drawing.Color))
                    return 1;

                var cx = (System.Drawing.Color)vx;
                var cy = (System.Drawing.Color)vy;

                return cx.ToArgb().CompareTo(cy.ToArgb());
            }

            #endregion
        }
        #endregion
    }
}
