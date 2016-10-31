using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant2
{
    public class SubRow : DataRowBase
    {
        public IDataRow ParentRow { get; private set; }
        public string FullKey
        {
            get { return ParentRow.Key + "." + Key; }
        }

        public SubRow(IDataRow parent, int key, int offset) : base(parent.Sheet, key, offset)
        {
            ParentRow = parent;
        }
    }
}
