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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel.Composition;

namespace Thaliak.Views {
    /// <summary>
    /// Interaction logic for MainRegionTab.xaml
    /// </summary>
    [Export(typeof(MainRegionTabView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainRegionTabView : UserControl, IContentPresenter {
        public MainRegionTabView() {
            InitializeComponent();
        }

        [Import]
        public ViewModels.MainRegionTabViewModel ViewModel {
            get { return (ViewModels.MainRegionTabViewModel)DataContext; }
            set { DataContext = value; }
        }

        #region IRegionManagerAware Members

        public Microsoft.Practices.Prism.Regions.IRegionManager RegionManager {
            get { return ViewModel.RegionManager; }
            set { ViewModel.RegionManager = value; }
        }
        public string TargetRegion { get { return RegionNames.ActiveContentRegion; } }
        private Guid _Id = Guid.NewGuid();
        public Guid Id { get { return _Id; } }
        #endregion
    }
}
