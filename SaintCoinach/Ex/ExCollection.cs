using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SaintCoinach.IO;

using File = SaintCoinach.IO.File;

namespace SaintCoinach.Ex {
    public class ExCollection {
        #region Fields

        private readonly Dictionary<int, string> _SheetIdentifiers = new Dictionary<int, string>();

        private readonly Dictionary<string, WeakReference<ISheet>> _Sheets =
            new Dictionary<string, WeakReference<ISheet>>();

        private HashSet<string> _AvailableSheets;

        #endregion

        #region Properties

        public PackCollection PackCollection { get; private set; }
        public Language ActiveLanguage { get; set; }
        public string ActiveLanguageCode { get { return ActiveLanguage.GetCode(); } }
        public IEnumerable<string> AvailableSheets { get { return _AvailableSheets; } }

        #endregion

        #region Constructors

        #region Constructor

        public ExCollection(PackCollection packCollection) {
            PackCollection = packCollection;

            BuildIndex();
        }

        #endregion

        #endregion

        #region Index

        private void BuildIndex() {
            var exRoot = PackCollection.GetFile("exd/root.exl");

            var available = new List<string>();

            using (var ms = new MemoryStream(exRoot.GetData())) {
                using (var s = new StreamReader(ms, Encoding.ASCII)) {
                    s.ReadLine(); // EXLT,2

                    while (!s.EndOfStream) {
                        var line = s.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var split = line.Split(',');
                        if (split.Length != 2)
                            continue;

                        var name = split[0];
                        var id = int.Parse(split[1]);

                        available.Add(name);
                        if (id >= 0)
                            _SheetIdentifiers.Add(id, name);
                    }
                }
            }

            _AvailableSheets = new HashSet<string>(available);
        }

        #endregion

        #region Get

        public bool SheetExists(int id) {
            return _SheetIdentifiers.ContainsKey(id);
        }

        public bool SheetExists(string name) {
            //name = FixName(name);
            return _AvailableSheets.Contains(name);
        }

        public ISheet<T> GetSheet<T>(int id) where T : IRow {
            return (ISheet<T>)GetSheet(id);
        }

        public ISheet GetSheet(int id) {
            var name = _SheetIdentifiers[id];
            return GetSheet(name);
        }

        public ISheet<T> GetSheet<T>(string name) where T : IRow {
            return (ISheet<T>)GetSheet(name);
        }

        public ISheet GetSheet(string name) {
            const string ExHPathFormat = "exd/{0}.exh";

            //name = FixName(name);
            if (!_AvailableSheets.Contains(name))
                throw new KeyNotFoundException();

            ISheet sheet;
            WeakReference<ISheet> sheetRef;
            if (_Sheets.TryGetValue(name, out sheetRef) && sheetRef.TryGetTarget(out sheet)) return sheet;

            var exhPath = string.Format(ExHPathFormat, name);
            var exh = PackCollection.GetFile(exhPath);

            var header = CreateHeader(name, exh);
            sheet = CreateSheet(header);

            if (_Sheets.ContainsKey(name))
                _Sheets[name].SetTarget(sheet);
            else
                _Sheets.Add(name, new WeakReference<ISheet>(sheet));
            return sheet;
        }

        public string FixName(string name) {
            var res = _AvailableSheets.Where(_ => string.Equals(name, _, StringComparison.OrdinalIgnoreCase)).ToArray();

            return res.Any() ? res.First() : name;
        }

        #endregion

        #region Factory

        protected virtual Header CreateHeader(string name, File file) {
            return new Header(this, name, file);
        }

        protected virtual ISheet CreateSheet(Header header) {
            if (header.AvailableLanguages.Count() > 1)
                return new MultiSheet<MultiRow, DataRow>(this, header);
            return new DataSheet<DataRow>(this, header, header.AvailableLanguages.First());
        }

        #endregion
    }
}
