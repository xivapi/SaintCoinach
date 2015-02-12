using System;
using System.Collections.Generic;
using System.Diagnostics;

using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex.Relational.Update.Changes;
using SaintCoinach.IO;

namespace SaintCoinach.Ex.Relational.Update {
    public class RelationUpdater {
        #region Static

        private const Language UsedLanguage = Language.Japanese;

        #endregion

        #region Fields

        private readonly RelationalExCollection _Previous;
        private readonly IProgress<UpdateProgress> _Progress;
        private readonly RelationalExCollection _Updated;

        #endregion

        #region Properties

        public RelationDefinition Previous { get; private set; }
        public RelationDefinition Updated { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public RelationUpdater(PackCollection previousPacks,
                               RelationDefinition previousDefinition,
                               PackCollection updatedPacks,
                               string updatedVersion,
                               IProgress<UpdateProgress> progress) {
            _Progress = progress ?? new NullProgress();

            _Previous = new RelationalExCollection(previousPacks);
            Previous = previousDefinition;

            _Updated = new RelationalExCollection(updatedPacks);
            Updated = new RelationDefinition {
                Version = updatedVersion
            };

            _Previous.ActiveLanguage = UsedLanguage;
            _Updated.ActiveLanguage = UsedLanguage;
        }

        #endregion

        #endregion

        #region Update

        public IEnumerable<IChange> Update(bool detectDataChanges) {
            var changes = new List<IChange>();

            var progress = new UpdateProgress {
                CurrentOperation = "Structure",
                CurrentStep = 0,
                TotalSteps = (detectDataChanges ? 2 : 1) * Previous.SheetDefinitions.Count
            };
            {
                foreach (var prevSheetDef in Previous.SheetDefinitions) {
                    Trace.WriteLine(string.Format("Update sheet {0}.", prevSheetDef.Name));

                    progress.CurrentFile = prevSheetDef.Name;
                    _Progress.Report(progress);

                    if (!_Updated.SheetExists(prevSheetDef.Name)) {
                        changes.Add(new SheetRemoved(prevSheetDef.Name));
                        continue;
                    }

                    var prevSheet = _Previous.GetSheet(prevSheetDef.Name);

                    var updatedSheet = _Updated.GetSheet(prevSheetDef.Name);
                    var updatedSheetDef = Updated.GetOrCreateSheet(prevSheetDef.Name);

                    var sheetUpdater = new SheetUpdater(prevSheet, prevSheetDef, updatedSheet, updatedSheetDef);
                    changes.AddRange(sheetUpdater.Update());

                    GC.Collect();

                    ++progress.CurrentStep;
                }

                Updated.Compile();
            }

            if (detectDataChanges) {
                progress.CurrentOperation = "Data";

                foreach (var prevSheetDef in Previous.SheetDefinitions) {
                    Trace.WriteLine(string.Format("Detect data changes in {0}.", prevSheetDef.Name));

                    progress.CurrentFile = prevSheetDef.Name;
                    _Progress.Report(progress);

                    if (!_Updated.SheetExists(prevSheetDef.Name))
                        continue;

                    var prevSheet = _Previous.GetSheet(prevSheetDef.Name);

                    var updatedSheet = _Updated.GetSheet(prevSheetDef.Name);
                    var updatedSheetDef = Updated.GetOrCreateSheet(prevSheetDef.Name);

                    var sheetComparer = new SheetComparer(prevSheet, prevSheetDef, updatedSheet, updatedSheetDef);
                    changes.AddRange(sheetComparer.Compare());

                    GC.Collect();

                    ++progress.CurrentStep;
                }
            }

            progress.CurrentStep = progress.TotalSteps;
            progress.CurrentOperation = "Finished";
            progress.CurrentFile = null;
            _Progress.Report(progress);

            return changes;
        } /*
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
        }*/

        #endregion

        private class NullProgress : IProgress<UpdateProgress> {
            #region IProgress<UpdateProgress> Members

            public void Report(UpdateProgress value) { }

            #endregion
        }
    }
}
