using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Services.Search {
    public abstract class QueryCollection : Interfaces.ISearchQuery {
        #region Fields
        private ICollection<Interfaces.ISearchQuery> _SubQueries = new ObservableCollection<Interfaces.ISearchQuery>();
        #endregion

        #region Properties
        public IEnumerable<Interfaces.ISearchQuery> SubQueries { get { return _SubQueries; } }
        #endregion

        #region Things
        public virtual void Add(Interfaces.ISearchQuery query) {
            _SubQueries.Add(query);
        }
        public virtual bool Remove(Interfaces.ISearchQuery query) {
            return _SubQueries.Remove(query);
        }
        #endregion

        #region ISearchQuery Members
        public abstract IEnumerable<Type> MatchedTypes { get; }

        public abstract bool IsMatch(object value);
        #endregion
    }
}
