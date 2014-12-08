using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Mvvm;

namespace Thaliak.ViewModels {
    [Export(typeof(SearchResultsViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SearchResultsViewModel : NavigationTarget, Interfaces.ITitledView {
        private Interfaces.ISearchQuery _SearchQuery;
        public Interfaces.ISearchQuery SearchQuery {
            get { return _SearchQuery; }
            private set {
                _SearchQuery = value;
                OnPropertyChanged(() => SearchQuery);
                if (value == null)
                    Title = "Search";
                else
                    Title = string.Format("Search ({0})", value);
            }
        }

        public override void OnNavigatedTo(Microsoft.Practices.Prism.Regions.NavigationContext navigationContext) {
            base.OnNavigatedTo(navigationContext);

            var qParam = navigationContext.Parameters.Where(_ => _.Key == "Q");
            if (qParam.Any()) {
                var qIdStr = qParam.Select(_ => _.Value).First().ToString();
                var qId = Guid.Parse(qIdStr);
                SearchQuery = (Interfaces.ISearchQuery)ObjectStorage.Retrieve(qId);
            }
        }

        #region ITitledView Members
        private string _Title = "Search";
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
