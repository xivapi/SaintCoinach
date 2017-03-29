using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

        #region Update

        public IEnumerable<IChange> Update(bool detectDataChanges) {
            var changes = new ConcurrentBag<IChange>();

            var progress = new UpdateProgress {
                CurrentOperation = "Structure",
                TotalSteps = (detectDataChanges ? 2 : 1) * Previous.SheetDefinitions.Count
            };

            var sheetLock = new object();

            Parallel.ForEach(Previous.SheetDefinitions, prevSheetDef =>
            {
                progress.CurrentFile = prevSheetDef.Name;
                _Progress.Report(progress);

                if (!_Updated.SheetExists(prevSheetDef.Name))
                {
                    changes.Add(new SheetRemoved(prevSheetDef.Name));
                    return;
                }

                IRelationalSheet prevSheet, updatedSheet;
                SheetDefinition updatedSheetDef;
                lock (sheetLock)
                {
                    prevSheet = _Previous.GetSheet(prevSheetDef.Name);
                    updatedSheet = _Updated.GetSheet(prevSheetDef.Name);
                    updatedSheetDef = Updated.GetOrCreateSheet(prevSheetDef.Name);
                }

                var sheetUpdater = new SheetUpdater(prevSheet, prevSheetDef, updatedSheet, updatedSheetDef);
                foreach (var update in sheetUpdater.Update())
                    changes.Add(update);

                //GC.Collect();

                progress.IncrementStep();
            });

            Updated.Compile();

            if (detectDataChanges) {
                progress.CurrentOperation = "Data";

                Parallel.ForEach(Previous.SheetDefinitions, prevSheetDef =>
                {
                    progress.CurrentFile = prevSheetDef.Name;
                    _Progress.Report(progress);

                    if (!_Updated.SheetExists(prevSheetDef.Name))
                        return;

                    IRelationalSheet prevSheet, updatedSheet;
                    SheetDefinition updatedSheetDef;
                    lock (sheetLock)
                    {
                        prevSheet = _Previous.GetSheet(prevSheetDef.Name);
                        updatedSheet = _Updated.GetSheet(prevSheetDef.Name);
                        updatedSheetDef = Updated.GetOrCreateSheet(prevSheetDef.Name);
                    }

                    var sheetComparer = new SheetComparer(prevSheet, prevSheetDef, updatedSheet, updatedSheetDef);
                    foreach (var change in sheetComparer.Compare())
                        changes.Add(change);

                    //GC.Collect();

                    progress.IncrementStep();
                });
            }

            progress.CurrentOperation = "Finished";
            progress.CurrentFile = null;
            _Progress.Report(progress);

            return changes;
        }

        #endregion

        private class NullProgress : IProgress<UpdateProgress> {
            #region IProgress<UpdateProgress> Members

            public void Report(UpdateProgress value) { }

            #endregion
        }
    }
}
