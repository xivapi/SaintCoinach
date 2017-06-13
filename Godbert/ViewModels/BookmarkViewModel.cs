using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert.ViewModels {
    public class BookmarkViewModel {
        public string SheetName { get; set; }
        public int Key { get; set; }
        public string RowDefault { get; set; }
        public int? ColumnIndex { get; set; }
        public string ColumnName { get; set; }

        public override string ToString() {
            var rowIdentifier = $"{SheetName}#{Key}";

            var sb = new StringBuilder();
            sb.Append(rowIdentifier);

            if (ColumnName != null)
                sb.Append($" [{ColumnIndex} {ColumnName}]");
            else if (ColumnIndex != null)
                sb.Append($" [{ColumnIndex}]");

            if (!string.IsNullOrWhiteSpace(RowDefault) && RowDefault != rowIdentifier) {
                sb.AppendLine();
                sb.Append(RowDefault);
            }

            return sb.ToString();
        }

        public override bool Equals(object obj) {
            var b = obj as BookmarkViewModel;
            if (b == null)
                return false;

            return b.SheetName == SheetName && b.Key == Key && b.ColumnIndex == ColumnIndex;
        }

        public override int GetHashCode() {
            return SheetName.GetHashCode() ^ Key.GetHashCode() ^ (ColumnIndex ?? 0).GetHashCode();
        }
    }
}
