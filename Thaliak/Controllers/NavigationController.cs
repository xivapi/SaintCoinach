using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
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

        private Dictionary<Guid, IContentPresenter> _KnownContent = new Dictionary<Guid, IContentPresenter>();

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

            var navReqEvt = _EventAggregator.GetEvent<Events.NavigationRequestEvent>();
            navReqEvt.Subscribe(OnNavigationRequest, ThreadOption.UIThread);
        }

        #region Nav request
        // TODO: Support other regions (i.e. docks)
        const string DefaultRegion = RegionNames.MainRegion;
        static readonly string[] ValidNavigationRegions = new[] {
            RegionNames.MainRegion
        };

        private void OnNavigationRequest(Events.NavigationRequestArguments e) {
            string targetRegionName = DefaultRegion;
            if (!string.IsNullOrWhiteSpace(e.Region))
                targetRegionName = e.Region;
            if (!ValidNavigationRegions.Contains(targetRegionName)) {
                _EventAggregator.GetEvent<Events.NavigationResultEvent>().Publish(e.Id, new NavigationResult(null, false));
                return;
            }

            var region = _RegionManager.Regions.First(_ => _.Name == targetRegionName);
            var openNewTab = e.ForceNewTab || !region.ActiveViews.Any();

            IContentPresenter targetPresenter;
            if (openNewTab) {
                targetPresenter = _ServiceLocator.GetInstance<Views.MainRegionTabView>();   // TODO: Change this to get proper (i.e. docks)

                var innerRegionManager = region.Add(targetPresenter, targetPresenter.Id.ToString(), true);
                region.Activate(targetPresenter);

                targetPresenter.RegionManager = innerRegionManager;
                _KnownContent.Add(targetPresenter.Id, targetPresenter);
            } else
                targetPresenter = (IContentPresenter)region.ActiveViews.First();

            targetPresenter.RegionManager.RequestNavigate(targetPresenter.TargetRegion, e.Uri, (r) => {
                _EventAggregator.GetEvent<Events.NavigationResultEvent>().Publish(e.Id, r);
            });
        }
        #endregion
    }
}
