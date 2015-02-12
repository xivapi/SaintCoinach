using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class SheetLanguageRemoved : IChange {
        #region Properties

        public Language Language { get; private set; }

        #endregion

        #region Constructors

        public SheetLanguageRemoved(string name, Language language) {
            SheetName = name;
            Language = language;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Data | ChangeType.Breaking; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("{0} has been removed from sheet {1}.", Language, SheetName);
        }
    }
}
