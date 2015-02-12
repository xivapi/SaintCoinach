using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class Enhancement : ItemAction, IParameterObject {
        #region Static

        private const int GroupKey = 0;
        private const int ItemFoodKey = 1;
        private const int DurationKey = 2;

        #endregion

        #region Fields

        private IEnumerable<Parameter> _Parameters;

        #endregion

        #region Properties

        public int EnhancementGroup { get { return GetData(GroupKey); } }
        public int EnhancementGroupHq { get { return GetHqData(GroupKey); } }

        public ItemFood ItemFood {
            get {
                var key = GetData(ItemFoodKey);
                return Sheet.Collection.GetSheet<ItemFood>()[key];
            }
        }

        public ItemFood ItemFoodHq {
            get {
                var key = GetHqData(ItemFoodKey);
                return Sheet.Collection.GetSheet<ItemFood>()[key];
            }
        }

        public TimeSpan Duration { get { return TimeSpan.FromSeconds(GetData(DurationKey)); } }
        public TimeSpan DurationHq { get { return TimeSpan.FromSeconds(GetHqData(DurationKey)); } }

        #endregion

        #region Constructors

        #region Constructor

        public Enhancement(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public IEnumerable<Parameter> Parameters { get { return _Parameters ?? (_Parameters = BuildParameters()); } }

        #region Build

        private ParameterCollection BuildParameters() {
            var parameters = new ParameterCollection();

            var f = ItemFood;
            var fHq = ItemFoodHq;
            if (f == fHq)
                parameters.AddRange(f.Parameters);
            else {
                foreach (var p in f.Parameters) {
                    foreach (var v in p.Where(_ => _.Type != ParameterType.Hq))
                        parameters.AddParameterValue(p.BaseParam, v);
                }
                foreach (var p in fHq.Parameters) {
                    foreach (var v in p.Where(_ => _.Type == ParameterType.Hq))
                        parameters.AddParameterValue(p.BaseParam, v);
                }
            }
            return parameters;
        }

        #endregion
    }
}
