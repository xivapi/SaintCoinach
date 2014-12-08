using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class Enhancement : ItemAction {
        const int GroupKey = 0;
        const int ItemFoodKey = 1;
        const int DurationKey = 2;

        #region Fields
        private IEnumerable<Parameter> _Parameters = null;
        #endregion

        #region Properties
        public IEnumerable<Parameter> Parameters { get { return _Parameters ?? (_Parameters = BuildParameters()); } }
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

        #region Constructor
        public Enhancement(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ParameterCollection BuildParameters() {
            var parameters = new ParameterCollection();

            var f = ItemFood;
            var fHq = ItemFoodHq;
            if (f == fHq)
                parameters.AddRange(f.Parameters);
            else {
                foreach (var p in f.Parameters) {
                    foreach (var v in p.Where(_ => _.Type != ParameterType.HQ))
                        parameters.AddParameterValue(p.BaseParam, v);
                }
                foreach (var p in fHq.Parameters) {
                    foreach (var v in p.Where(_ => _.Type == ParameterType.HQ))
                        parameters.AddParameterValue(p.BaseParam, v);
                }
            }
            return parameters;
        }
        #endregion
    }
}