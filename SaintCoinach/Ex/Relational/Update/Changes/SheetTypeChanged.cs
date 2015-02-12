using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class SheetTypeChanged : IChange {
        #region Constructors

        public SheetTypeChanged(string name) {
            SheetName = name;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Data | ChangeType.Breaking; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("Type of sheet '{0}' has been changed.", SheetName);
        }
    }
}
