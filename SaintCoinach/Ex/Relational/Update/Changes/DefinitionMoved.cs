using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class DefinitionMoved : IChange {
        private string _SheetName;
        private int _PreviousIndex;
        private int _NewIndex;
        private double _Confidence;

        public ChangeType ChangeType { get { return ChangeType.Structure; } }
        public string SheetName { get { return _SheetName; } }
        public int PreviousIndex { get { return _PreviousIndex; } }
        public int NewIndex { get { return _NewIndex; } }
        public double Confidence { get { return _Confidence; } }

        public DefinitionMoved(string sheetName, int prevIndex, int newIndex, double c) {
            _SheetName = sheetName;
            _PreviousIndex = prevIndex;
            _NewIndex = newIndex;
            _Confidence = c;
        }

        public override string ToString() {
            return string.Format("Definition '{0}'@{1} was moved to {2} (c={3:P}).", SheetName, PreviousIndex, NewIndex, Confidence);
        }
    }
}
