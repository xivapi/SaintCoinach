using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class ExCollection {
        #region Fields
        private IO.PackCollection _PackCollection;
        private Language _ActiveLanguage;
        private string[] _AvailableSheets;
        private Dictionary<int, string> _SheetIdentifiers = new Dictionary<int, string>();
        private Dictionary<string, WeakReference<ISheet>> _Sheets = new Dictionary<string, WeakReference<ISheet>>(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region Properties
        public IO.PackCollection PackCollection { get { return _PackCollection; } }
        public Language ActiveLanguage {
            get { return _ActiveLanguage; }
            set { _ActiveLanguage = value; }
        }
        public string ActiveLanguageCode {
            get { return _ActiveLanguage.GetCode(); }
        }
        public IEnumerable<string> AvailableSheets { get { return _AvailableSheets; } }
        #endregion

        #region Constructor
        public ExCollection(IO.PackCollection packCollection) {
            _PackCollection = packCollection;

            BuildIndex();
        }
        #endregion

        #region Index
        private void BuildIndex() {
            var exRoot = PackCollection.GetFile("exd/root.exl");

            var available = new List<string>();

            using (var ms = new MemoryStream(exRoot.GetData())) {
                using (var s = new StreamReader(ms, Encoding.ASCII)) {
                    s.ReadLine();   // EXLT,2

                    while (!s.EndOfStream) {
                        var line = s.ReadLine();
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

            _AvailableSheets = available.ToArray();
        }
        #endregion

        #region Get
        public bool SheetExists(string name) {
            //name = FixName(name);
            return AvailableSheets.Contains(name);
        }
        public ISheet GetSheet(string name) {
            const string ExHPathFormat = "exd/{0}.exh";

            //name = FixName(name);
            if (!AvailableSheets.Contains(name))
                throw new KeyNotFoundException();

            ISheet sheet;
            WeakReference<ISheet> sheetRef;
            if (!_Sheets.TryGetValue(name, out sheetRef) || !sheetRef.TryGetTarget(out sheet)) {
                var exhPath = string.Format(ExHPathFormat, name);
                var exh = PackCollection.GetFile(exhPath);

                var header = CreateHeader(name, exh);
                sheet = CreateSheet(header);

                if (_Sheets.ContainsKey(name))
                    _Sheets[name].SetTarget(sheet);
                else
                    _Sheets.Add(name, new WeakReference<ISheet>(sheet));
            }
            return sheet;
        }
        public string FixName(string name) {
            var res = _AvailableSheets.Where(_ => string.Equals(name, _, StringComparison.OrdinalIgnoreCase));
            if (res.Any())
                return res.First();
            return name;
        }
        #endregion

        #region Factory
        protected virtual Header CreateHeader(string name, IO.File file) {
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
