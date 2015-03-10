using System.Collections.Generic;
using System.IO;
using System.Linq;

using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex.Relational.Update.Changes;

// ReSharper disable PossibleMultipleEnumeration

namespace SaintCoinach.Ex.Relational.Update {
    public class SheetComparer {
        #region Fields

        private readonly SheetDefinition _PreviousDefinition;
        private readonly IRelationalSheet _PreviousSheet;
        private readonly SheetDefinition _UpdatedDefinition;
        private readonly IRelationalSheet _UpdatedSheet;

        #endregion

        #region Constructors

        #region Constructor

        public SheetComparer(IRelationalSheet prevSheet,
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

        #region Detect

        private class ColumnMap {
            #region Properties

            public string Name { get; set; }
            public int NewIndex { get; set; }
            public int PreviousIndex { get; set; }

            #endregion
        }

        public IEnumerable<IChange> Compare() {
            var changes = new List<IChange>();


            var prevKeys = _PreviousSheet.Cast<IRow>().Select(_ => _.Key).ToArray();
            var updatedKeys = _UpdatedSheet.Cast<IRow>().Select(_ => _.Key).ToArray();

            changes.AddRange(updatedKeys.Except(prevKeys).Select(_ => new RowAdded(_UpdatedDefinition.Name, _)));
            changes.AddRange(prevKeys.Except(updatedKeys).Select(_ => new RowRemoved(_PreviousDefinition.Name, _)));

            var columns = _UpdatedDefinition.GetAllColumnNames().Select(_ => {
                var previousColumn = _PreviousDefinition.FindColumn(_);
                var newColumn = _UpdatedDefinition.FindColumn(_);

                if (!previousColumn.HasValue || !newColumn.HasValue)
                    throw new InvalidDataException();

                return new ColumnMap {
                    Name = _,
                    PreviousIndex = previousColumn.Value,
                    NewIndex = newColumn.Value
                };
            }).ToArray();

            var prevIsMulti = _PreviousSheet is IMultiSheet;
            var upIsMulti = _UpdatedSheet is IMultiSheet;
            if (prevIsMulti == upIsMulti) {
                if (prevIsMulti) {
                    var prevMulti = (IMultiSheet)_PreviousSheet;
                    var prevLang = _PreviousSheet.Header.AvailableLanguages;

                    var upMulti = (IMultiSheet)_UpdatedSheet;
                    var upLang = _UpdatedSheet.Header.AvailableLanguages;

                    changes.AddRange(upLang.Except(prevLang).Select(_ => new SheetLanguageAdded(_UpdatedSheet.Name, _)));
                    changes.AddRange(
                                     prevLang.Except(upLang)
                                             .Select(_ => new SheetLanguageRemoved(_PreviousDefinition.Name, _)));

                    foreach (var lang in prevLang.Intersect(upLang)) {
                        var prevSheet = prevMulti.GetLocalisedSheet(lang);
                        var upSheet = upMulti.GetLocalisedSheet(lang);

                        changes.AddRange(Compare(prevSheet, upSheet, lang, columns));
                    }
                } else
                    changes.AddRange(Compare(_PreviousSheet, _UpdatedSheet, Language.None, columns));
            } else {
                changes.Add(new SheetTypeChanged(_UpdatedDefinition.Name));
                System.Console.Error.WriteLine("Type of sheet {0} has changed, unable to detect changes.",
                    _UpdatedDefinition.Name);
            }

            return changes;
        }

        private static IEnumerable<IChange> Compare(ISheet previousSheet,
                                                    ISheet updatedSheet,
                                                    Language language,
                                                    ColumnMap[] columns) {
            var prevRows = previousSheet.Cast<IRow>().ToArray();
            var updatedRows = updatedSheet.Cast<IRow>().ToDictionary(_ => _.Key, _ => _);


            foreach (var prevRow in prevRows) {
                if (!updatedRows.ContainsKey(prevRow.Key)) continue;

                var updatedRow = updatedRows[prevRow.Key];

                foreach (var col in columns) {
                    var prevVal = prevRow[col.PreviousIndex];
                    var upVal = updatedRow[col.NewIndex];

                    if (!Comparer.IsMatch(prevVal, upVal))
                        yield return
                            new FieldChanged(updatedSheet.Header.Name, language, col.Name, updatedRow.Key, prevVal,
                                upVal);
                }
            }
        }

        #endregion
    }
}
