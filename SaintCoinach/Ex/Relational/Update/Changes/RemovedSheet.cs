using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    public class RemovedSheet : IChange {
        private string _SheetName;

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Breaking; } }
        public string SheetName { get { return _SheetName; } }

        public RemovedSheet(string name) {
            _SheetName = name;
        }

        public override string ToString() {
            return string.Format("Sheet '{0}' has been removed.", SheetName);
        }
    }
}
