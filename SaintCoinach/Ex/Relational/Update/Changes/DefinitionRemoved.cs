using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class DefinitionRemoved : IChange {
        #region Fields

        private readonly int _NewIndex = -1;

        #endregion

        #region Properties

        public int PreviousIndex { get; private set; }
        public int NewIndex { get { return _NewIndex; } }
        public double Confidence { get; private set; }

        #endregion

        #region Constructors

        public DefinitionRemoved(string sheetName, int prevIndex) {
            Confidence = 0.0;
            SheetName = sheetName;
            PreviousIndex = prevIndex;
        }

        public DefinitionRemoved(string sheetName, int prevIndex, int newIndex, double c) {
            SheetName = sheetName;
            PreviousIndex = prevIndex;
            _NewIndex = newIndex;
            Confidence = c;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Breaking; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            if (NewIndex >= 0)
                return string.Format("Definition '{0}'@{1} was removed (highest match at {2} with c={3:P}).", SheetName,
                    PreviousIndex, NewIndex, Confidence);
            return string.Format("Definition '{0}'@{1} was removed (no match).", SheetName, PreviousIndex);
        }
    }
}
