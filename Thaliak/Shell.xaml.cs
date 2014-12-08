using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak {
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : MahApps.Metro.Controls.MetroWindow {
        [ImportingConstructor]
        public Shell(IEventAggregator eventAggregator, Services.IObjectStore objectStore) {
            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");
            if (objectStore == null)
                throw new ArgumentNullException("objectStore");

            _EventAggregator = eventAggregator;
            _ObjectStore = objectStore;

            InitializeComponent();

            _EventAggregator.GetEvent<Events.NavigationResultEvent>().Subscribe(OnDockNavigationResult, ThreadOption.UIThread, false,
                (e) => e.Request.Region == RegionNames.LeftDockRegion || e.Request.Region == RegionNames.RightDockRegion);
        }

        private void OnDockNavigationResult(Events.NavigationResultArguments e) {
            if (e.NavigationResult.Result.GetValueOrDefault(false)) {
                if (e.Request.Region == RegionNames.LeftDockRegion)
                    ExpanderDockLeft.IsExpanded = true;
                else if (e.Request.Region == RegionNames.RightDockRegion)
                    ExpanderDockRight.IsExpanded = true;
            }
        }

        private Services.IObjectStore _ObjectStore;
        private IEventAggregator _EventAggregator;

        [Import]
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
        ShellViewModel ViewModel {
            set {
                this.DataContext = value;
            }
        }

        #region Drag/Drop
        private void Element_DragEnter(object sender, DragEventArgs e) {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.StringFormat)) {
                var str = (string)e.Data.GetData(DataFormats.StringFormat);
                Guid id;
                if (Guid.TryParse(str, out id))
                    e.Effects = DragDropEffects.Link;
            }
        }

        private void Element_DragLeave(object sender, DragEventArgs e) {
            
        }

        private void Element_DragOver(object sender, DragEventArgs e) {

        }

        private void Element_Drop(object sender, DragEventArgs e) {
            var frameworkElement = sender as FrameworkElement;

            if (frameworkElement != null && e.Data.GetDataPresent(DataFormats.StringFormat)) {
                var str = (string)e.Data.GetData(DataFormats.StringFormat);
                Guid id;
                if (Guid.TryParse(str, out id)) {
                    var obj = _ObjectStore.Retrieve(id);
                    var evt = _EventAggregator.GetEvent<Events.NavigationRequestEvent>();

                    string targetRegion;
                    switch (frameworkElement.Name) {
                        case "ExpanderDockLeft":
                            targetRegion = RegionNames.LeftDockRegion;
                            break;
                        case "ExpanderDockRight":
                            targetRegion = RegionNames.RightDockRegion;
                            break;
                        case "MainRegionControl":
                            targetRegion = RegionNames.MainRegion;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    evt.Publish(obj, false, targetRegion);
                }
            }
        }
        #endregion

        #region Expansion
        private GridLength _DockLeftColumnWidth = new GridLength(1, GridUnitType.Star);
        private GridLength _DockRightColumnWidth = new GridLength(1, GridUnitType.Star);

        private void ExpanderDockLeft_Expanded(object sender, RoutedEventArgs e) {
            if (e.Source != ExpanderDockLeft)
                return;
            _DockLeftColumn.Width = _DockLeftColumnWidth;
        }
        private void ExpanderDockLeft_Collapsed(object sender, RoutedEventArgs e) {
            if (e.Source != ExpanderDockLeft)
                return;
            _DockLeftColumnWidth = _DockLeftColumn.Width;
            _DockLeftColumn.Width = GridLength.Auto;
        }

        private void ExpanderDockRight_Expanded(object sender, RoutedEventArgs e) {
            if (e.Source != ExpanderDockRight)
                return;
            _DockRightColumn.Width = _DockRightColumnWidth;
        }
        private void ExpanderDockRight_Collapsed(object sender, RoutedEventArgs e) {
            if (e.Source != ExpanderDockRight)
                return;
            _DockRightColumnWidth = _DockRightColumn.Width;
            _DockRightColumn.Width = GridLength.Auto;
        }
        #endregion
    }
}
