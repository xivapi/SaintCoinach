using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godbert {
    public class ObservableBase : INotifyPropertyChanged {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) {
            var h = PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property) {
            MemberExpression memberExpression;
            if (property.Body is UnaryExpression) {
                var unaryExpression = (UnaryExpression)property.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            } else {
                memberExpression = (MemberExpression)property.Body;
            }

            OnPropertyChanged(memberExpression.Member.Name);
        }

        #endregion
    }
}
