using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public interface IDataDefinition {
        int Length { get; }

        /// <param name="row">The row to convert a value of.</param>
        /// <param name="index">Index in the definition on which the method is called.</param>
        /// <param name="offset">Offset in the row.</param>
        object Convert(IDataRow row, object value, int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        string GetName(int index);

        /// <param name="index">Index in the definition on which the method is called.</param>
        string GetValueType(int index);

        IDataDefinition Clone();
    }
}
