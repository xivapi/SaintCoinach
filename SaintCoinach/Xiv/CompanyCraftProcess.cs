using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public partial class CompanyCraftProcess : XivRow {
        #region Fields
        private Request[] _Requests;
        #endregion

        #region Properties

        public IEnumerable<Request> Requests { get { return _Requests ?? (_Requests = BuildRequests()); } }

        #endregion

        #region Constructors

        public CompanyCraftProcess(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        private Request[] BuildRequests() {
            const int Count = 12;

            var reqs = new List<Request>();

            for(var i = 0; i < Count; ++i) {
                var supplyItem = As<CompanyCraftSupplyItem>("SupplyItem", i);
                var perSet = AsInt32("SetQuantity", i);
                var setCount = AsInt32("SetsRequired", i);

                if (supplyItem == null || supplyItem.Key == 0 || perSet == 0 || setCount == 0)
                    continue;

                reqs.Add(new Request(supplyItem, perSet, setCount));
            }

            return reqs.ToArray();
        }
    }
}
