using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class OrnamentUnlock : ItemAction {
        // Used to unlock fashion accessory, such as parasol.

        private const int OrnamentKey = 0;
        
        public OrnamentUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        public Ornament Ornament { 
            get {
                var key = GetData(OrnamentKey);
                return Sheet.Collection.GetSheet<Ornament>()[key];
            }
        }
    }
}
