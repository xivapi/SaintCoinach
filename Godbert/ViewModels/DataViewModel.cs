using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using SaintCoinach.Ex.Relational;

namespace Godbert.ViewModels {
    using Commands;

    public class DataViewModel : ObservableBase {
        #region Fields
        private string _SelectedSheetName;
        private IRelationalSheet _SelectedSheet;
        private DelegateCommand _ExportCsvCommand;
        private string _FilterSheetTerm;
        private IEnumerable<string> _FilteredSheets;
        private string _FilterDataTerm;
        private ObservableCollection<BookmarkViewModel> _Bookmarks = new ObservableCollection<BookmarkViewModel>();
        #endregion

        #region Properties
        public ICommand ExportCsvCommand { get { return _ExportCsvCommand ?? (_ExportCsvCommand = new DelegateCommand(OnExportCsv)); } }
        public SaintCoinach.ARealmReversed Realm { get; private set; }
        public MainViewModel Parent { get; private set; }

        public IEnumerable<string> FilteredSheetNames { get { return _FilteredSheets; } }

        public string SelectedSheetName {
            get { return _SelectedSheetName; }
            set {
                _SelectedSheetName = value;
                _SelectedSheet = null;
                OnPropertyChanged(() => SelectedSheetName);
                OnPropertyChanged(() => SelectedSheet);

                Settings.Default.SelectedSheetName = value;
            }
        }
        public IRelationalSheet SelectedSheet {
            get {
                if (string.IsNullOrWhiteSpace(SelectedSheetName))
                    return null;
                if (_SelectedSheet == null)
                    _SelectedSheet = Realm.GameData.GetSheet(SelectedSheetName);
                return _SelectedSheet;
            }
        }
        public string FilterSheetTerm {
            get { return _FilterSheetTerm; }
            set {
                _FilterSheetTerm = value;

                if (string.IsNullOrWhiteSpace(value))
                    _FilteredSheets = Realm.GameData.AvailableSheets;
                else
                    _FilteredSheets = Realm.GameData.AvailableSheets.Where(s => s.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();

                OnPropertyChanged(() => FilterSheetTerm);
                OnPropertyChanged(() => FilteredSheetNames);

                Settings.Default.FilterSheetTerm = value;
            }
        }

        public string FilterDataTerm {
            get { return _FilterDataTerm; }
            set {
                _FilterDataTerm = value;

                OnPropertyChanged(() => FilterDataTerm);

                Settings.Default.FilterDataTerm = value;
            }
        }

        public ObservableCollection<BookmarkViewModel> Bookmarks {
            get { return _Bookmarks; }
        }

        public Visibility BookmarksVisibility {
            get { return _Bookmarks.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }
        #endregion

        #region Constructor
        public DataViewModel(SaintCoinach.ARealmReversed realm, MainViewModel parent) {
            this.Realm = realm;
            this.Parent = parent;

            _FilteredSheets = Realm.GameData.AvailableSheets;

            _SelectedSheetName = Settings.Default.SelectedSheetName;
            _FilterDataTerm = Settings.Default.FilterDataTerm;
            FilterSheetTerm = Settings.Default.FilterSheetTerm;

            _Bookmarks.CollectionChanged += _Bookmarks_CollectionChanged;
        }
        #endregion

        #region Export
        private void OnExportCsv() {
            if (SelectedSheet == null)
                return;

            var dlg = new Microsoft.Win32.SaveFileDialog {
                DefaultExt = ".csv",
                Filter = "CSV Files (*.csv)|*.csv",
                AddExtension = true,
                OverwritePrompt = true,
                FileName = FixName(SelectedSheet.Name) + ".csv"
            };

            if (dlg.ShowDialog().GetValueOrDefault(false))
                SaveAsCsv(SelectedSheet, dlg.FileName);
        }
        private static string FixName(string original) {
            var idx = original.LastIndexOf('/');
            if (idx >= 0)
                return original.Substring(idx + 1);
            return original;
        }
        static void SaveAsCsv(IRelationalSheet sheet, string path) {
            using (var s = new StreamWriter(path, false, Encoding.UTF8)) {
                var indexLine = new StringBuilder("key");
                var nameLine = new StringBuilder("#");
                var typeLine = new StringBuilder("int32");

                var colIndices = new List<int>();
                foreach (var col in sheet.Header.Columns) {
                    indexLine.AppendFormat(",{0}", col.Index);
                    nameLine.AppendFormat(",{0}", col.Name);
                    typeLine.AppendFormat(",{0}", col.ValueType);

                    colIndices.Add(col.Index);
                }

                s.WriteLine(indexLine);
                s.WriteLine(nameLine);
                s.WriteLine(typeLine);

                foreach (var row in sheet.Cast<SaintCoinach.Ex.IRow>().OrderBy(_ => _.Key)) {
                    s.Write(row.Key);
                    foreach (var col in colIndices) {
                        var v = row[col];

                        if (v == null)
                            s.Write(",");
                        else if (v is IDictionary<int, object>)
                            WriteDict(s, v as IDictionary<int, object>);
                        else if (IsUnescaped(v))
                            s.Write(",{0}", v);
                        else
                            s.Write(",\"{0}\"", v.ToString().Replace("\"", "\"\""));
                    }
                    s.WriteLine();
                }
            }
        }
        static void WriteDict(StreamWriter s, IDictionary<int, object> v) {
            s.Write(",\"");
            var isFirst = true;
            foreach (var kvp in v) {
                if (isFirst)
                    isFirst = false;
                else
                    s.Write(",");
                s.Write("[{0},", kvp.Key);
                if (kvp.Value != null)
                    s.Write(kvp.Value.ToString().Replace("\"", "\"\""));
                s.Write("]");
            }
            s.Write("\"");
        }
        static bool IsUnescaped(object self) {
            return (self is Boolean
                || self is Byte
                || self is SByte
                || self is Int16
                || self is Int32
                || self is Int64
                || self is UInt16
                || self is UInt32
                || self is UInt64
                || self is Single
                || self is Double);
        }
        #endregion

        private void _Bookmarks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged(() => BookmarksVisibility);
        }
    }
}
