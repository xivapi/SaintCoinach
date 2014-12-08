using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System.Windows.Input;

namespace Thaliak {
    [Export]
    public class ShellViewModel : BindableBase {
        public const int MaximumResults = 250;  // TODO: Maybe add back/forward to results view? Or at least show there are too many!

        [Import]
        private Services.ISearchProvider _SearchProvider;
        [Import]
        private IEventAggregator _EventAggregator;
        [Import]
        private Services.IObjectStore _ObjectStore;

        private ICommand _SearchCommand;
        public ICommand SearchCommand { get { return _SearchCommand ?? (_SearchCommand = new DelegateCommand<string>(PerformSearch)); } }

        private void PerformSearch(string query) {
            if (!string.IsNullOrWhiteSpace(query)) {
                var q = _SearchProvider.Parse(query);
                if (q != null) {
                    var items = _SearchProvider.Search(q).Cast<object>().ToArray();
                    var evt = _EventAggregator.GetEvent<Events.NavigationRequestEvent>();

                    if (items.Length == 1)
                        evt.Publish(items[0], false);
                    else {
                        var navParam = new NavigationParameters();
                        if (items.Length > MaximumResults)
                            items = items.Take(MaximumResults).ToArray();
                        var objId = _ObjectStore.Store(items);
                        var qId = _ObjectStore.Store(q);
                        navParam.Add("ID", objId);
                        navParam.Add("Q", qId);

                        if (items.Length == 0)
                            evt.Publish(new Events.NavigationRequestArguments {
                                Uri = new Uri("NoSearchResultsView" + navParam, UriKind.Relative)
                            });
                        else {

                            evt.Publish(new Events.NavigationRequestArguments {
                                Uri = new Uri("SearchResultsView" + navParam, UriKind.Relative)
                            });
                        }
                    }
                }
            }
        }
    }
}
