using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Modules.Core {
    [Export(typeof(TitledNavigationTarget))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TitledNavigationTarget : NavigationTarget, Interfaces.ITitledView {
        public TitledNavigationTarget() {
            this.PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if ("NavigationObject".Equals(e.PropertyName)) {
                if (NavigationObject == null)
                    Title = "?";
                else
                    Title = NavigationObject.ToString();
            }
        }

        #region ITitledView Members
        private string _Title;
        public string Title {
            get { return _Title; }
            private set {
                _Title = value;
                OnPropertyChanged(() => Title);
                var h = TitleChanged;
                if (h != null)
                    h(this, EventArgs.Empty);
            }
        }

        public event EventHandler TitleChanged;

        #endregion
    }
}
