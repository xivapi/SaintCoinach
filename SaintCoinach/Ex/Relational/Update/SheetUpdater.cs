using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex.Relational.Update.Changes;

namespace SaintCoinach.Ex.Relational.Update {
    public class SheetUpdater {
        #region Static

        private const double RequiredMatchConfidence = 0.05; // One in twenty has to be an exact match
        private const double ExactMatchConfidence = 0.95;

        #endregion

        #region Fields

        private readonly SheetDefinition _PreviousDefinition;
        private readonly IRelationalSheet _PreviousSheet;
        private readonly SheetDefinition _UpdatedDefinition;
        private readonly IRelationalSheet _UpdatedSheet;

        #endregion

        #region Constructors

        #region Constructor

        public SheetUpdater(IRelationalSheet prevSheet,
                            SheetDefinition prevDefinition,
                            IRelationalSheet upSheet,
                            SheetDefinition upDefinition) {
            _PreviousSheet = prevSheet;
            _PreviousDefinition = prevDefinition;

            _UpdatedSheet = upSheet;
            _UpdatedDefinition = upDefinition;
        }

        #endregion

        #endregion

        #region Update

        private Dictionary<DefinitionUpdater, IDictionary<int, double>> _Updaters;
        private List<IChange> _ChangeLog;
        private List<int> _UsedColumns;
        private Dictionary<int, IDictionary<DefinitionUpdater, double>> _ColumnMatches;

        void AddMoveChange(DefinitionUpdater updater, int newIndex, double confidence) {
            if (newIndex != updater.DataDefinition.Index)
                _ChangeLog.Add(new DefinitionMoved(_PreviousDefinition.Name, updater.DataDefinition.Index, newIndex, confidence));

            _UpdatedDefinition.DataDefinitions.Add(updater.GetDefinition(newIndex));

            _Updaters.Remove(updater);
            foreach (var u in _Updaters)
                u.Value.Remove(newIndex);

            for (var i = 0; i < updater.DataDefinition.Length; ++i) {
                _UsedColumns.Add(newIndex + i);
                _ColumnMatches.Remove(newIndex + 1);
            }
        }
        public IEnumerable<IChange> Update() {
            _ChangeLog = new List<IChange>();

            _UpdatedDefinition.DefaultColumn = _PreviousDefinition.DefaultColumn;

            _Updaters = MatchRows().ToDictionary(_ => _, _ => _.GetMatches());
            _UsedColumns = new List<int>();

            ProcessDefinitionMatches();
            ProcessColumnMatches();
            ProcessLeftovers();
            
            /*
            foreach (var def in defs) {
                double c;
                var updatedDef = def.GetBestMatch(out c);

                if (updatedDef == null || double.IsNaN(c) || c < RequiredMatchConfidence) {
                    if (updatedDef != null)
                        changes.Add(new DefinitionRemoved(_PreviousDefinition.Name, def.DataDefinition.Index,
                            updatedDef.Index, c));
                    else
                        changes.Add(new DefinitionRemoved(_PreviousDefinition.Name, def.DataDefinition.Index));
                } else {
                    if (updatedDef.Index != def.DataDefinition.Index)
                        changes.Add(new DefinitionMoved(_PreviousDefinition.Name, def.DataDefinition.Index,
                            updatedDef.Index, c));

                    _UpdatedDefinition.DataDefinitions.Add(updatedDef);
                }
            }*/

            return _ChangeLog;
        }
        bool IsUnusedColumnRange(int index, int length) {
            return !_UsedColumns.Any(c => c >= index && c < (index + length));
        }
        void ProcessDefinitionMatches() {
            _ColumnMatches = new Dictionary<int, IDictionary<DefinitionUpdater, double>>();

            foreach (var updater in _Updaters.ToArray()) {
                int bestMatchIndex = -1;
                double bestMatchConfidence = double.NegativeInfinity;

                // Purge unsatisfactory matches
                foreach(var match in updater.Value.ToArray()) {
                    if(match.Value > bestMatchConfidence || (match.Value == bestMatchConfidence && match.Key == updater.Key.DataDefinition.Index)) {
                        bestMatchIndex = match.Key;
                        bestMatchConfidence = match.Value;
                    }

                    if (match.Value < RequiredMatchConfidence)
                        updater.Value.Remove(match.Key);
                }

                // No satisfactory match
                if (updater.Value.Count == 0) {
                    if(bestMatchIndex >= 0)
                        _ChangeLog.Add(new DefinitionRemoved(_PreviousDefinition.Name, updater.Key.DataDefinition.Index, bestMatchIndex, bestMatchConfidence));
                    else
                        _ChangeLog.Add(new DefinitionRemoved(_PreviousDefinition.Name, updater.Key.DataDefinition.Index));

                    continue;
                }

                // At least one match is considered to be exact
                if(bestMatchConfidence >= ExactMatchConfidence) {
                    AddMoveChange(updater.Key, bestMatchIndex, bestMatchConfidence);
                    continue;
                }

                // Populate column matches
                foreach(var match in updater.Value) {
                    IDictionary<DefinitionUpdater, double> d;
                    if (!_ColumnMatches.TryGetValue(match.Key, out d))
                        _ColumnMatches.Add(match.Key, d = new Dictionary<DefinitionUpdater, double>());
                    d.Add(updater.Key, match.Value);
                }
            }
        }
        void ProcessColumnMatches() {
            foreach(var col in _ColumnMatches.ToArray()) {
                if (_UsedColumns.Contains(col.Key))
                    continue;
                var groups = col.Value
                    .Where(_ => 
                        IsUnusedColumnRange(col.Key, _.Key.DataDefinition.Length)
                        && _Updaters.ContainsKey(_.Key)
                        && !_Updaters[_.Key].Values.Any(mc => mc > _.Value))
                    .GroupBy(_ => _.Value)
                    .OrderByDescending(_ => _.Key)
                    .Select(_ => new {
                        Confidence = _.Key,
                        Matches = _.ToArray()
                    }).ToArray();

                if (groups.Length == 0)
                    continue;

                var bestGroup = groups[0];
                KeyValuePair<DefinitionUpdater, double> bestMatch;
                if(bestGroup.Matches.Length == 1) {
                    bestMatch = bestGroup.Matches[0];
                } else {
                    Console.WriteLine("Multiple possible matches on '{0}' @ '{1}' with c={2:P} from {3}", 
                        _PreviousDefinition.Name, col.Key, bestGroup.Confidence,
                        string.Join(", ", bestGroup.Matches.Select(m => string.Format("{0} @ {1}", m.Key.DataDefinition.GetName(m.Key.DataDefinition.Index), m.Key.DataDefinition.Index))));

                    bestMatch = bestGroup.Matches.OrderBy(_ => Math.Abs(_.Key.DataDefinition.Index - col.Key)).First();
                }

                AddMoveChange(bestMatch.Key, col.Key, bestMatch.Value);
            }
        }
        void ProcessLeftovers() {
            foreach(var updater in _Updaters.ToArray()) {
                var remainingMatches = updater.Value.Where(r => IsUnusedColumnRange(r.Key, updater.Key.DataDefinition.Length)).OrderByDescending(m => m.Value).ToArray();
                if (remainingMatches.Length == 0)
                    _ChangeLog.Add(new DefinitionRemoved(_PreviousDefinition.Name, updater.Key.DataDefinition.Index));
                else
                    AddMoveChange(updater.Key, remainingMatches[0].Key, remainingMatches[0].Value);
            }
        }


        private IEnumerable<DefinitionUpdater> MatchRows() {
            var defUpdaters =
                _PreviousDefinition.DataDefinitions.Select(_ => new DefinitionUpdater(_PreviousDefinition, _)).ToArray();

            foreach (IRow prevRow in _PreviousSheet) {
                var prevRowFields =
                    _PreviousSheet.Header.Columns.OrderBy(_ => _.Index).Select(_ => prevRow[_.Index]).ToArray();
                if (!_UpdatedSheet.ContainsRow(prevRow.Key)) continue;

                var updatedRow = _UpdatedSheet[prevRow.Key];
                var updatedRowFields =
                    _UpdatedSheet.Header.Columns.OrderBy(_ => _.Index).Select(_ => updatedRow[_.Index]).ToArray();

                foreach (var def in defUpdaters)
                    def.MatchRow(prevRowFields, updatedRowFields);
            }

            return defUpdaters;
        }

        #endregion
    }
}
