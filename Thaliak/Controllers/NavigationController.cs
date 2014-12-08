using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Controllers {
    [Export(typeof(NavigationController))]
    public class NavigationController {
        private IServiceLocator _ServiceLocator;
        private IRegionManager _RegionManager;
        private IEventAggregator _EventAggregator;

        private Dictionary<Guid, Tuple<IContentPresenter, string>> _KnownContent = new Dictionary<Guid, Tuple<IContentPresenter, string>>();

        [ImportingConstructor]
        public NavigationController(IRegionManager regionManager, IEventAggregator eventAggregator, IServiceLocator serviceLocator) {
            if (regionManager == null)
                throw new ArgumentNullException("regionManager");
            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");
            if (serviceLocator == null)
                throw new ArgumentNullException("serviceLocator");

            _RegionManager = regionManager;
            _EventAggregator = eventAggregator;
            _ServiceLocator = serviceLocator;

            _EventAggregator.GetEvent<Events.NavigationRequestEvent>().Subscribe(OnNavigationRequest, ThreadOption.UIThread);
            _EventAggregator.GetEvent<Events.TabCloseRequestEvent>().Subscribe(OnTabCloseRequest, ThreadOption.UIThread);
        }

        #region TabClose
        private void OnTabCloseRequest(Guid id) {
            Tuple<IContentPresenter, string> presenter;
            if (_KnownContent.TryGetValue(id, out presenter)) {
                var region = _RegionManager.Regions.First(_ => _.Name == presenter.Item2);
                region.Remove(presenter.Item1);
            }
        }
        #endregion

        #region Nav request
        const string DefaultRegion = RegionNames.MainRegion;
        static readonly string[] TabbedRegions = new[] {
            RegionNames.MainRegion
        };
        static readonly string[] DirectRegions = new[]{
            RegionNames.LeftDockRegion,
            RegionNames.RightDockRegion
        };

        private void OnNavigationRequest(Events.NavigationRequestArguments e) {
            string targetRegionName = DefaultRegion;
            if (!string.IsNullOrWhiteSpace(e.Region))
                e.Region = targetRegionName = e.Region;

            var region = _RegionManager.Regions.First(_ => _.Name == targetRegionName);
            if (TabbedRegions.Contains(targetRegionName))
                NavigateTabbedRegion(e, region);
            else if (DirectRegions.Contains(targetRegionName))
                NavigateDirectRegion(e, region);
            else
                _EventAggregator.GetEvent<Events.NavigationResultEvent>().Publish(e, new NavigationResult(null, false));
        }
        private void NavigateDirectRegion(Events.NavigationRequestArguments e, IRegion region) {
            region.RequestNavigate(e.Uri, (r) => {
                _EventAggregator.GetEvent<Events.NavigationResultEvent>().Publish(e, r);
            });
        }
        private void NavigateTabbedRegion(Events.NavigationRequestArguments e, IRegion region) {
            var openNewTab = e.ForceNewTab || !region.ActiveViews.Any() || Keyboard.Modifiers.HasFlag(ModifierKeys.Control);

            IContentPresenter targetPresenter;
            if (openNewTab) {
                targetPresenter = _ServiceLocator.GetInstance<Views.MainRegionTabView>();

                var innerRegionManager = region.Add(targetPresenter, targetPresenter.Id.ToString(), true);
                region.Activate(targetPresenter);

                targetPresenter.RegionManager = innerRegionManager;
                _KnownContent.Add(targetPresenter.Id, Tuple.Create(targetPresenter, region.Name));
            } else
                targetPresenter = (IContentPresenter)region.ActiveViews.First();

            targetPresenter.RegionManager.RequestNavigate(targetPresenter.TargetRegion, e.Uri, (r) => {
                _EventAggregator.GetEvent<Events.NavigationResultEvent>().Publish(e, r);
            });
        }
        #endregion
    }
}
