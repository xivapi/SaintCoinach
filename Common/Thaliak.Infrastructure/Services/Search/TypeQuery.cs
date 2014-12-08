using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Services.Search {
    [Behaviors.SearchFunctionExport(Function = "type")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TypeQuery : Interfaces.ISearchQuery {
        public string Type { get; set; }
        #region ISearchQuery Members

        public IEnumerable<Type> MatchedTypes {
            get { yield return typeof(object); }
        }

        public bool IsMatch(object value) {
            if (value == null)
                return false;

            var t = value.GetType();
            while (t != null) {
                if (t.Name.Equals(Type, StringComparison.OrdinalIgnoreCase))
                    return true;
                t = t.BaseType;
            }
            return false;
        }

        public void Set(string args) {
            Type = args.Trim();
        }

        #endregion

        public override string ToString() {
            return string.Format("type:{0}", Type);
        }
    }
}
