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

    public class TerritoryViewModel : ObservableBase {
        #region Fields
        private TerritoryView[] _AllTerritories;
        private TerritoryView[] _FilteredTerritories;
        private TerritoryView _SelectedTerritory;
        private string _FilterTerm;
        #endregion

        #region Properties
        public MainViewModel Parent { get; private set; }
        public IEnumerable<TerritoryView> AllTerritories { get { return _AllTerritories; } }
        public IEnumerable<TerritoryView> FilteredTerritories { get { return _FilteredTerritories; } }
        public TerritoryView SelectedTerritory {
            get { return _SelectedTerritory; }
            set {
                _SelectedTerritory = value;
                OnPropertyChanged(() => SelectedTerritory);
            }
        }
        public string FilterTerm {
            get { return _FilterTerm; }
            set {
                _FilterTerm = value;

                if (string.IsNullOrWhiteSpace(value))
                    _FilteredTerritories = _AllTerritories;
                else
                    _FilteredTerritories = _AllTerritories.Where(e => e.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();

                OnPropertyChanged(() => FilterTerm);
                OnPropertyChanged(() => FilteredTerritories);
            }
        }
        #endregion

        #region Constructor
        public TerritoryViewModel(MainViewModel parent) {
            this.Parent = parent;

            var allTerritoryTypes = parent.Realm.GameData.GetSheet<TerritoryType>();

            _AllTerritories = allTerritoryTypes
                .Where(t => !string.IsNullOrEmpty(t.Bg.ToString()))
                .Select(t => new TerritoryView(t))
                .OrderBy(m => m.PlaceNames)
                .ThenBy(m => m.TerritoryType.Key)
                .ToArray();

            _FilteredTerritories = _AllTerritories;
        }
        #endregion

        #region Commands
        private ICommand _OpenCommand;

        public ICommand OpenCommand { get { return _OpenCommand ?? (_OpenCommand = new DelegateCommand(OnOpen)); } }

        private void OnOpen() {
            if (SelectedTerritory == null)
                return;

            try {
                var t = new Territory(SelectedTerritory.TerritoryType);

                if (t == null)
                    System.Windows.MessageBox.Show(string.Format("Could not find territory data for {0}.", SelectedTerritory.Name), "Territory not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                else
                    Parent.EngineHelper.OpenInNew(SelectedTerritory.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentTerritory(e, t));
            } catch (Exception e) {
                System.Windows.MessageBox.Show(string.Format("Error reading territory for {0}:{1}{2}", SelectedTerritory.Name, Environment.NewLine, e), "Failure to read territory", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        #endregion

        #region Refresh
        public void Refresh() {
            _FilteredTerritories = _FilteredTerritories.ToArray();
            OnPropertyChanged(() => FilteredTerritories);
        }
        #endregion
    }

    public class TerritoryView
    {
        public TerritoryView(TerritoryType territory)
        {
            TerritoryType = territory;

            var places = new List<string>();
            places.Add(territory.RegionPlaceName.Name.ToString());
            places.Add(territory.ZonePlaceName.Name.ToString());
            places.Add(territory.PlaceName.Name.ToString());
            PlaceNames = string.Join(" > ", places.Where(p => !string.IsNullOrEmpty(p)).Distinct());
            
            Name = string.Format("({0}) {1}", territory.Name.ToString(), PlaceNames);
        }

        public TerritoryType TerritoryType { get; private set; }
        public string Name { get; private set; }
        public string PlaceNames { get; private set; }
    }
}
