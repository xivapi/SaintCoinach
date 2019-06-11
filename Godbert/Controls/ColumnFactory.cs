using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex;
using SaintCoinach.Ex.DataReaders;

namespace Godbert.Controls {
    static class ColumnFactory {
        public static DataGridColumn Create(RelationalColumn column) {
            var sheetDef = column.Header.SheetDefinition;
            Type defType = null;
            if (sheetDef != null)
                defType = sheetDef.GetValueType(column.Index);
            var targetType = defType ?? column.Reader.Type;

            var header = BuildHeader(column);
            var binding = CreateCellBinding(column);

            DataGridColumn target = null;
            if (typeof(SaintCoinach.Imaging.ImageFile).IsAssignableFrom(targetType))
                target = new RawDataGridImageColumn(column) {
                    Binding = binding,
                };
            else if (typeof(System.Drawing.Color).IsAssignableFrom(targetType))
                target = new RawDataGridColorColumn(column) {
                    Binding = binding
                };

            target = target ?? new RawDataGridTextColumn(column) {
                Binding = binding
            };

            target.Header = header;
            target.IsReadOnly = true;
            target.CanUserSort = true;
            return target;
        }

        private static string BuildHeader(RelationalColumn column) {
            var sb = new StringBuilder();

            sb.Append(column.Index);
            if (!string.IsNullOrWhiteSpace(column.Name))
                sb.AppendFormat(": {0}", column.Name);
            sb.Append(Environment.NewLine);
            sb.Append(column.Reader.Type.Name);

            if (column.ValueType != column.Reader.Name)
                sb.AppendFormat(" > {0}", column.ValueType);

            if (Settings.Default.ShowOffsets) {
                if (column.Reader is PackedBooleanDataReader)
                    sb.AppendFormat(" [{0:X}&{1:X2}]", column.Offset, ((PackedBooleanDataReader)column.Reader).Mask);
                else
                    sb.AppendFormat(" [{0:X}]", column.Offset);
            }

            return sb.ToString();
        }
        private static Binding CreateCellBinding(RelationalColumn column) {
            return new Binding {
                Converter = CellConverterInstance,
                ConverterParameter = column.Index,
                Mode = BindingMode.OneWay
            };
        }

        public static bool ForceRaw;
        public static readonly System.Windows.Data.IValueConverter CellConverterInstance = new CellConverter();

        private class CellConverter : System.Windows.Data.IValueConverter {



            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
                var row = value as IRow;
                if (row == null)
                    return null;

                var i = System.Convert.ToInt32(parameter);

                if (ForceRaw || RawDataGrid.ColumnSetToRaw[i])
                    return row.GetRaw(i);

                return row[i] ?? row.GetRaw(i);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
