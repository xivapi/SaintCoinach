using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class SheetRemoved : IChange {
        #region Constructors

        public SheetRemoved(string name) {
            SheetName = name;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Breaking; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("Sheet '{0}' has been removed.", SheetName);
        }
    }
}
