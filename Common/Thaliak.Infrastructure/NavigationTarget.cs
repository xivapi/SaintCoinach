using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak {
    [Export(typeof(NavigationTarget))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NavigationTarget : BindableBase, INavigationAware {
        #region Fields
        private object _NavigationObject;
        private Services.IObjectStore _ObjectStorage;
        #endregion

        #region Properties
        public object NavigationObject {
            get { return _NavigationObject; }
            set {
                _NavigationObject = value;
                OnPropertyChanged(() => NavigationObject);
            }
        }
        protected Services.IObjectStore ObjectStorage { get { return _ObjectStorage ?? (_ObjectStorage = ServiceLocator.Current.GetInstance<Services.IObjectStore>()); } }
        #endregion

        #region INavigationAware Members
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext) {
            
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext) {
            var idParam = navigationContext.Parameters.Where(_ => _.Key == "ID");
            if (idParam.Any()) {
                var idStr = idParam.Select(_ => _.Value).First().ToString();
                var id = Guid.Parse(idStr);
                NavigationObject = ObjectStorage.Retrieve(id);
            }
        }
        #endregion
    }
}
