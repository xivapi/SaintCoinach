using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace Thaliak.ViewModels {
    [Export(typeof(MainRegionTabViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainRegionTabViewModel : BindableBase {
        private string _Title;
        private Interfaces.ITitledView _ActiveTitledView;
        private IRegionManager _RegionManager;
        private Guid _Id = Guid.NewGuid();

        [Import]
        private IEventAggregator EventAggregator { get; set; }
        public Guid Id { get { return _Id; } }

        public IRegionManager RegionManager {
            get { return _RegionManager; }
            set {
                if(_RegionManager != null)
                    ActiveContentRegion.NavigationService.Navigated -= OnNavigated;
                _RegionManager = value;
                if(_RegionManager != null)
                    ActiveContentRegion.NavigationService.Navigated += OnNavigated;
            }
        }

        void OnNavigated(object sender, RegionNavigationEventArgs e) {
            if(_ActiveTitledView != null)
                _ActiveTitledView.TitleChanged -= OnActiveTitleChanged;

            var activeView = ActiveContentRegion.ActiveViews.FirstOrDefault();
            _ActiveTitledView = activeView as Interfaces.ITitledView;
            if (_ActiveTitledView == null && activeView is FrameworkElement)
                _ActiveTitledView = ((FrameworkElement)activeView).DataContext as Interfaces.ITitledView;

            if (activeView == null)
                Title = "?";
            else {
                if (_ActiveTitledView != null) {
                    _ActiveTitledView.TitleChanged += OnActiveTitleChanged;
                    Title = _ActiveTitledView.Title;
                } else
                    Title = activeView.ToString();
            }

            OnPropertyChanged(() => CanGoBack);
            OnPropertyChanged(() => CanGoForward);
            if (_GoBackCommand != null)
                _GoBackCommand.RaiseCanExecuteChanged();
            if (_GoForwardCommand != null)
                _GoForwardCommand.RaiseCanExecuteChanged();
        }

        void OnActiveTitleChanged(object sender, EventArgs e) {
            Title = _ActiveTitledView.Title;
        }
        public IRegion ActiveContentRegion {
            get {
                if (_RegionManager == null)
                    return null;
                return _RegionManager.Regions.First(_ => _.Name == RegionNames.ActiveContentRegion);
            }
        }
        public string Title {
            get { return _Title; }
            private set {
                _Title = value;
                OnPropertyChanged(() => Title);
            }
        }

        #region Commands
        private DelegateCommand _GoBackCommand;
        private DelegateCommand _GoForwardCommand;
        private DelegateCommand _CloseCommand;

        public ICommand GoBackCommand { get { return _GoBackCommand ?? (_GoBackCommand = new DelegateCommand(GoBack, () => this.CanGoBack)); } }
        public ICommand GoForwardCommand { get { return _GoForwardCommand ?? (_GoForwardCommand = new DelegateCommand(GoForward, () => this.CanGoForward)); } }
        public ICommand CloseCommand { get { return _CloseCommand ?? (_CloseCommand = new DelegateCommand(Close)); } }

        public bool CanGoBack { get { return ActiveContentRegion.NavigationService.Journal.CanGoBack; } }
        public bool CanGoForward { get { return ActiveContentRegion.NavigationService.Journal.CanGoForward; } }

        public void Close() {
            EventAggregator.GetEvent<Events.TabCloseRequestEvent>().Publish(Id);
        }
        public void GoBack() {
            ActiveContentRegion.NavigationService.Journal.GoBack();
        }
        public void GoForward() {
            ActiveContentRegion.NavigationService.Journal.GoForward();
        }
        #endregion
    }
}
