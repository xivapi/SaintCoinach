using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class DefinitionMoved : IChange {
        #region Properties

        public int PreviousIndex { get; private set; }
        public int NewIndex { get; private set; }
        public double Confidence { get; private set; }

        #endregion

        #region Constructors

        public DefinitionMoved(string sheetName, int prevIndex, int newIndex, double c) {
            SheetName = sheetName;
            PreviousIndex = prevIndex;
            NewIndex = newIndex;
            Confidence = c;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Structure; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("Definition '{0}'@{1} was moved to {2} (c={3:P}).", SheetName, PreviousIndex, NewIndex,
                Confidence);
        }
    }
}
