using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    public class SheetComparer {
        #region Fields
        private IRelationalSheet _PreviousSheet;
        private Definition.SheetDefinition _PreviousDefinition;

        private IRelationalSheet _UpdatedSheet;
        private Definition.SheetDefinition _UpdatedDefinition;
        #endregion

        #region Constructor
        public SheetComparer(IRelationalSheet prevSheet, Definition.SheetDefinition prevDefinition, IRelationalSheet upSheet, Definition.SheetDefinition upDefinition) {
            _PreviousSheet = prevSheet;
            _PreviousDefinition = prevDefinition;

            _UpdatedSheet = upSheet;
            _UpdatedDefinition = upDefinition;
        }
        #endregion

        #region Detect
        class ColumnMap {
            public string Name;
            public int PreviousIndex;
            public int NewIndex;
        }

        public IEnumerable<IChange> Compare() {
            var changes = new List<IChange>();


            var prevKeys = _PreviousSheet.GetAllRows().Select(_ => _.Key).ToArray();
            var updatedKeys = _UpdatedSheet.GetAllRows().Select(_ => _.Key).ToArray();

            changes.AddRange(updatedKeys.Except(prevKeys).Select(_ => new Changes.RowAdded(_UpdatedDefinition.Name, _)));
            changes.AddRange(prevKeys.Except(updatedKeys).Select(_ => new Changes.RowRemoved(_PreviousDefinition.Name, _)));

            var columns = _UpdatedDefinition.GetAllColumnNames().Select(_ => new ColumnMap {
                Name = _,
                PreviousIndex = _PreviousDefinition.FindColumn(_).Value,
                NewIndex = _UpdatedDefinition.FindColumn(_).Value
            }).ToArray();

            var prevIsMulti = _PreviousSheet is IMultiSheet;
            var upIsMulti = _UpdatedSheet is IMultiSheet;
            if (prevIsMulti == upIsMulti) {
                if (prevIsMulti) {
                    var prevMulti = _PreviousSheet as IMultiSheet;
                    var prevLang = _PreviousSheet.Header.AvailableLanguages;

                    var upMulti = _UpdatedSheet as IMultiSheet;
                    var upLang = _UpdatedSheet.Header.AvailableLanguages;

                    changes.AddRange(upLang.Except(prevLang).Select(_ => new Changes.SheetLanguageAdded(_UpdatedSheet.Name, _)));
                    changes.AddRange(prevLang.Except(upLang).Select(_ => new Changes.SheetLanguageRemoved(_PreviousDefinition.Name, _)));

                    foreach (var lang in prevLang.Intersect(upLang)) {
                        var prevSheet = prevMulti.GetLocalisedSheet(lang);
                        var upSheet = upMulti.GetLocalisedSheet(lang);

                        changes.AddRange(Compare(prevSheet, upSheet, lang, columns));
                    }
                } else
                    changes.AddRange(Compare(_PreviousSheet, _UpdatedSheet, Language.None, columns));
            } else {
                changes.Add(new Changes.SheetTypeChanged(_UpdatedDefinition.Name));
                System.Diagnostics.Trace.WriteLine(string.Format("Type of sheet {0} has changed, unable to detect changes.", _UpdatedDefinition.Name));
            }

            return changes;
        }
        private static IEnumerable<IChange> Compare(ISheet previousSheet, ISheet updatedSheet, Language language, ColumnMap[] columns) {
            var prevRows = previousSheet.GetAllRows();
            var updatedRows = updatedSheet.GetAllRows().ToDictionary(_ => _.Key, _ => _);


            foreach (var prevRow in prevRows) {
                if (updatedRows.ContainsKey(prevRow.Key)) {
                    var updatedRow = updatedRows[prevRow.Key];

                    foreach (var col in columns) {
                        var prevVal = prevRow[col.PreviousIndex];
                        var upVal = updatedRow[col.NewIndex];

                        if (!Comparer.IsMatch(prevVal, upVal))
                            yield return new Changes.FieldChanged(updatedSheet.Header.Name, language, col.Name, updatedRow.Key, prevVal, upVal);
                    }
                }
            }
        }
        #endregion
    }
}
