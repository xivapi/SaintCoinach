using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    public class SheetUpdater {
        const double RequiredMatchConfidence = 0.05;    // One in twenty has to be an exact match

        #region Fields
        private IRelationalSheet _PreviousSheet;
        private Definition.SheetDefinition _PreviousDefinition;

        private IRelationalSheet _UpdatedSheet;
        private Definition.SheetDefinition _UpdatedDefinition;
        #endregion

        #region Constructor
        public SheetUpdater(IRelationalSheet prevSheet, Definition.SheetDefinition prevDefinition, IRelationalSheet upSheet, Definition.SheetDefinition upDefinition) {
            _PreviousSheet = prevSheet;
            _PreviousDefinition = prevDefinition;

            _UpdatedSheet = upSheet;
            _UpdatedDefinition = upDefinition;
        }
        #endregion

        #region Update
        public IEnumerable<IChange> Update() {
            var changes = new List<IChange>();

            _UpdatedDefinition.DefaultColumn = _PreviousDefinition.DefaultColumn;

            var defs = MatchRows();
            foreach (var def in defs) {
                double c;
                var updatedDef = def.GetBestMatch(out c);

                if (updatedDef == null || double.IsNaN(c) || c < RequiredMatchConfidence) {
                    if (updatedDef != null)
                        changes.Add(new Changes.RemovedDefinition(_PreviousDefinition.Name, def.DataDefinition.Index, updatedDef.Index, c));
                    else
                        changes.Add(new Changes.RemovedDefinition(_PreviousDefinition.Name, def.DataDefinition.Index));
                } else {
                    if(updatedDef.Index != def.DataDefinition.Index)
                        changes.Add(new Changes.MovedDefinition(_PreviousDefinition.Name, def.DataDefinition.Index, updatedDef.Index, c));

                    _UpdatedDefinition.DataDefinitions.Add(updatedDef);
                }
            }

            return changes;
        }
        private IEnumerable<DefinitionUpdater> MatchRows() {
            var defUpdaters = _PreviousDefinition.DataDefinitions.Select(_ => new DefinitionUpdater(_PreviousDefinition, _)).ToArray();

            var prevRows = _PreviousSheet.GetAllRows();
            foreach (var prevRow in prevRows) {
                var prevRowFields = _PreviousSheet.Header.Columns.OrderBy(_ => _.Index).Select(_ => prevRow[_.Index]).ToArray();
                if (_UpdatedSheet.ContainsRow(prevRow.Key)) {
                    var updatedRow = _UpdatedSheet[prevRow.Key];
                    var updatedRowFields = _UpdatedSheet.Header.Columns.OrderBy(_ => _.Index).Select(_ => updatedRow[_.Index]).ToArray();

                    foreach (var def in defUpdaters)
                        def.MatchRow(prevRowFields, updatedRowFields);
                }
            }

            return defUpdaters;
        }
        public IEnumerable<IChange> DetectDataChanges() {
            var changes = new List<IChange>();

            throw new NotImplementedException();

            return changes;
        }
        #endregion
    }
}
