using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    public class RelationUpdater {
        const Language UsedLanguage = Language.Japanese;

        #region Fields
        private RelationalExCollection _Previous;
        private Definition.RelationDefinition _PreviousDefinition;
        private RelationalExCollection _Updated;
        private Definition.RelationDefinition _UpdatedDefinition;
        #endregion

        #region Properties
        public Definition.RelationDefinition Previous { get { return _PreviousDefinition; } }
        public Definition.RelationDefinition Updated { get { return _UpdatedDefinition; } }
        #endregion

        #region Constructor
        public RelationUpdater(IO.PackCollection previousPacks, Definition.RelationDefinition previousDefinition, IO.PackCollection updatedPacks) {
            _Previous = new RelationalExCollection(previousPacks);
            _PreviousDefinition = previousDefinition;

            _Updated = new RelationalExCollection(updatedPacks);
            _UpdatedDefinition = new Definition.RelationDefinition();

            _Previous.ActiveLanguage = UsedLanguage;
            _Updated.ActiveLanguage = UsedLanguage;
        }
        #endregion

        #region Update
        public IEnumerable<IChange> Update() {
            var changes = new List<IChange>();

            foreach (var prevSheetDef in Previous.SheetDefinitions) {
                System.Diagnostics.Trace.WriteLine(string.Format("Update sheet {0}.", prevSheetDef.Name));

                if (!_Updated.SheetExists(prevSheetDef.Name)) {
                    changes.Add(new Changes.SheetRemoved(prevSheetDef.Name));
                    continue;
                }

                var prevSheet = _Previous.GetSheet(prevSheetDef.Name);

                var updatedSheet = _Updated.GetSheet(prevSheetDef.Name);
                var updatedSheetDef = Updated.GetOrCreateSheet(prevSheetDef.Name);

                var sheetUpdater = new SheetUpdater(prevSheet, prevSheetDef, updatedSheet, updatedSheetDef);
                changes.AddRange(sheetUpdater.Update());

                GC.Collect();
            }

            Updated.Compile();

            return changes;
        }
        public IEnumerable<IChange> DetectDataChanges() {
            var changes = new List<IChange>();

            foreach (var prevSheetDef in Previous.SheetDefinitions) {
                System.Diagnostics.Trace.WriteLine(string.Format("Detect data changes in {0}.", prevSheetDef.Name));

                if (!_Updated.SheetExists(prevSheetDef.Name))
                    continue;

                var prevSheet = _Previous.GetSheet(prevSheetDef.Name);

                var updatedSheet = _Updated.GetSheet(prevSheetDef.Name);
                var updatedSheetDef = Updated.GetOrCreateSheet(prevSheetDef.Name);

                var sheetComparer = new SheetComparer(prevSheet, prevSheetDef, updatedSheet, updatedSheetDef);
                changes.AddRange(sheetComparer.Compare());

                GC.Collect();
            }

            return changes;
        }
        #endregion
    }
}
