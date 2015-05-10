using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SaintCoinach.Graphics;
using SaintCoinach.Graphics.Viewer;
using SaintCoinach.Xiv;

namespace Godbert.ViewModels {
    using Commands;

    public class MapsViewModel : ObservableBase {
        #region Fields
        private Map[] _Maps;
        private Map _SelectedMap;
        #endregion

        #region Properties
        public MainViewModel Parent { get; private set; }
        public IEnumerable<Map> Maps { get { return _Maps; } }
        public Map SelectedMap {
            get { return _SelectedMap; }
            set {
                _SelectedMap = value;
                OnPropertyChanged(() => SelectedMap);
            }
        }
        #endregion

        #region Constructor
        public MapsViewModel(MainViewModel parent) {
            this.Parent = parent;

            var allMaps = parent.Realm.GameData.GetSheet<Map>();
            _Maps = allMaps.Where(m => m.TerritoryType != null && m.TerritoryType.Key > 1 && m.PlaceName != null && m.PlaceName.Key != 0 && m.RegionPlaceName != null && m.RegionPlaceName.Key != 0).ToArray();
        }
        #endregion

        #region Commands
        private ICommand _OpenCommand;

        public ICommand OpenCommand { get { return _OpenCommand ?? (_OpenCommand = new DelegateCommand(OnOpen)); } }

        private void OnOpen() {
            if (SelectedMap == null)
                return;

            var n = string.Format("({0}) {1}: {2}", SelectedMap.Id, SelectedMap.RegionPlaceName, SelectedMap.PlaceName);
            try {
                var t = SelectedMap.GetTerritory();
                if (t == null)
                    System.Windows.MessageBox.Show(string.Format("Could not find territory data for {0}.", n), "Territory not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                else
                    Parent.EngineHelper.OpenInNew(n, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentTerritory(e, t));
            } catch (Exception e) {
                System.Windows.MessageBox.Show(string.Format("Error reading territory for {0}:{1}{2}", n, Environment.NewLine, e), "Failure to read territory", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        #endregion

        #region Refresh
        public void Refresh() {
            _Maps = _Maps.ToArray();
            OnPropertyChanged(() => Maps);
        }
        #endregion
    }
}
