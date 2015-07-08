using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class CompanyCraftProcess {
        public class Request {
            public CompanyCraftSupplyItem SupplyItem { get; private set; }
            public int QuantityPerSet { get; private set; }
            public int RequiredSets { get; private set; }
            public int TotalQuantity { get { return QuantityPerSet * RequiredSets; } }

            internal Request(CompanyCraftSupplyItem supplyItem, int quantityPerSet, int requiredSets) {
                SupplyItem = supplyItem;
                QuantityPerSet = quantityPerSet;
                RequiredSets = requiredSets;
            }
        }
    }
}
