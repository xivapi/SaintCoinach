using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class SheetLanguageAdded : IChange {
        #region Properties

        public Language Language { get; private set; }

        #endregion

        #region Constructors

        public SheetLanguageAdded(string name, Language language) {
            SheetName = name;
            Language = language;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Data; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("{0} has been added to sheet {1}.", Language, SheetName);
        }
    }
}
