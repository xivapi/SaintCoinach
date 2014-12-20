using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class SheetLanguageAdded : IChange {
        private string _SheetName;
        private Language _Language;

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Data; } }
        public string SheetName { get { return _SheetName; } }
        public Language Language { get { return _Language; } }

        public SheetLanguageAdded(string name, Language language) {
            _SheetName = name;
            _Language = language;
        }

        public override string ToString() {
            return string.Format("{0} has been added to sheet {1}.", Language, SheetName);
        }
    }
}
