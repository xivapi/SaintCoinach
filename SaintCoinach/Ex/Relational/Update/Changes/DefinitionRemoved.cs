using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class DefinitionRemoved : IChange {
        private string _SheetName;
        private int _PreviousIndex;
        private int _NewIndex = -1;
        private double _Confidence = 0.0;

        public ChangeType ChangeType { get { return ChangeType.Structure | ChangeType.Breaking; } }
        public string SheetName { get { return _SheetName; } }
        public int PreviousIndex { get { return _PreviousIndex; } }
        public int NewIndex { get { return _NewIndex; } }
        public double Confidence { get { return _Confidence; } }

        public DefinitionRemoved(string sheetName, int prevIndex) {
            _SheetName = sheetName;
            _PreviousIndex = prevIndex;
        }
        public DefinitionRemoved(string sheetName, int prevIndex, int newIndex, double c) {
            _SheetName = sheetName;
            _PreviousIndex = prevIndex;
            _NewIndex = newIndex;
            _Confidence = c;
        }

        public override string ToString() {
            if (NewIndex >= 0)
                return string.Format("Definition '{0}'@{1} was removed (highest match at {2} with c={3:P}).", SheetName, PreviousIndex, NewIndex, Confidence);
            return string.Format("Definition '{0}'@{1} was removed (no match).", SheetName, PreviousIndex);
        }
    }
}
