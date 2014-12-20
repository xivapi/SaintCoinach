using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class SheetTypeChanged : IChange {
        private string _SheetName;

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Data | ChangeType.Breaking; } }
        public string SheetName { get { return _SheetName; } }

        public SheetTypeChanged(string name) {
            _SheetName = name;
        }

        public override string ToString() {
            return string.Format("Type of sheet '{0}' has been changed.", SheetName);
        }
    }
}
