using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    public class DefinitionUpdater {
        #region Fields
        private Definition.PositionedDataDefintion _DataDefinition;
        private Definition.SheetDefinition _SheetDefinition;
        private Dictionary<int, List<double>> _IndexMatchConfidence = new Dictionary<int, List<double>>();
        private int _RowMatchCount = 0;
        #endregion

        #region Properties
        public Definition.PositionedDataDefintion DataDefinition { get { return _DataDefinition; } }
        #endregion

        #region Constructor
        public DefinitionUpdater(Definition.SheetDefinition sheetDefinition, Definition.PositionedDataDefintion dataDefinition) {
            _SheetDefinition = sheetDefinition;
            _DataDefinition = dataDefinition;
        }
        #endregion

        #region Match
        public void MatchRow(object[] previousRowData, object[] updatedRowData) {
            for (var updatedI = 0; updatedI <= updatedRowData.Length - DataDefinition.Length; ++updatedI) {
                var matches = 0;

                for (var i = 0; i < DataDefinition.Length; ++i) {
                    var prevColumn = i + DataDefinition.Index;
                    var upColumn = updatedI + i;

                    var prevVal = previousRowData[prevColumn];
                    var upVal = updatedRowData[upColumn];

                    if (Comparer.IsMatch(prevVal, upVal))
                        ++matches;
                }

                if (matches > 0) {
                    var c = matches / (double)DataDefinition.Length;
                    if (!_IndexMatchConfidence.ContainsKey(updatedI))
                        _IndexMatchConfidence.Add(updatedI, new List<double>());
                    _IndexMatchConfidence[updatedI].Add(c);
                }
            }

            ++_RowMatchCount;
        }
        public Definition.PositionedDataDefintion GetBestMatch(out double confidence) {
            int index = GetBestMatchIndex(out confidence);
            if (index < 0)
                return null;

            var newDef = DataDefinition.Clone();
            newDef.Index = index;
            return newDef;
        }

        private int GetBestMatchIndex(out double confidence) {
            if (_IndexMatchConfidence.Count == 0) {
                confidence = 1.0;
                return -1;
            }

            var matches = _IndexMatchConfidence.Select(_ => new {
                Index = _.Key,
                Count = _.Value.Count,
                Sum = _.Value.Sum(),
            }).GroupBy(_ => _.Sum).OrderByDescending(_ => _.Key);

            var bestMatch = matches.First();

            confidence = bestMatch.Key / (double)_RowMatchCount;

            if(bestMatch.Count() > 1)
                Console.WriteLine("Multiple possible matches on '{2}' @ '{3}' for {4} ({0} with c={1:P})", bestMatch.Count(), confidence, _SheetDefinition.Name, DataDefinition.GetName(DataDefinition.Index), DataDefinition.Length);

            if (bestMatch.Any(_ => _.Index == DataDefinition.Index))
                return DataDefinition.Index;
            return bestMatch.OrderBy(_ => Math.Abs(DataDefinition.Index - _.Index)).Select(_ => _.Index).First();
        }
        #endregion
    }
}
