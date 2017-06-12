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
using SaintCoinach.Ex;

namespace Godbert.Controls {
    public class RawDataGridImageColumn : DataGridBoundColumn, IRawDataColumn {
        private RelationalColumn _Column;
        private ContextMenu _ContextMenu;

        public RawDataGridImageColumn(RelationalColumn column) {
            _Column = column;

            _ContextMenu = new ContextMenu();
            var mi = new MenuItem {
                Header = "Save image",
            };
            mi.Click += OnSaveImageClick;
            _ContextMenu.Items.Add(mi);
        }

        void OnSaveImageClick(object sender, RoutedEventArgs e) {
            var fe = sender as FrameworkElement;
            if (fe == null)
                return;

            var ctx = fe.DataContext as IRelationalRow;
            if (ctx == null)
                return;

            var img = ctx[_Column.Index] as SaintCoinach.Imaging.ImageFile;
            if (img == null)
                return;

            var dlg = new Microsoft.Win32.SaveFileDialog {
                DefaultExt = ".png",
                Filter = "PNG Files (*.png)|*.png",
                AddExtension = true,
                OverwritePrompt = true,
                FileName = SetExt(img.Path.Split('/').Last(), "png"),
            };

            if (dlg.ShowDialog().GetValueOrDefault(false))
                img.GetImage().Save(dlg.FileName);
        }
        private static string SetExt(string orig, string ext) {
            var sb = new StringBuilder(orig);

            var idx = orig.LastIndexOf('.');
            if (idx > 0)
                sb.Remove(idx, sb.Length - idx);

            sb.Append('.');
            sb.Append(ext);
            return sb.ToString();
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem) {
            throw new NotImplementedException();
        }
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem) {
            Image obj = cell != null ? cell.Content as Image : null;
            if (obj == null) {
                obj = new Image {
                    Stretch = System.Windows.Media.Stretch.Uniform,
                    ContextMenu = _ContextMenu,
                };
                AttachedImage.SetIsSizeRestricted(obj, true);
            }

            var bind = Binding;
            if (bind == null)
                BindingOperations.ClearBinding(obj, AttachedImage.ImageProperty);
            else
                BindingOperations.SetBinding(obj, AttachedImage.ImageProperty, bind);

            return obj;
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

                var vx = rx[Column.Index] as SaintCoinach.Imaging.ImageFile;
                var vy = ry[Column.Index] as SaintCoinach.Imaging.ImageFile;

                if (vx == vy)
                    return 0;
                if (vx == null)
                    return -1;
                if (vy == null)
                    return 1;

                return vx.Path.CompareTo(vy.Path);
            }

            #endregion
        }
        #endregion
    }
}
