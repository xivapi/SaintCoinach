using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SaintCoinach.Graphics;
using SaintCoinach.Graphics.Sgb;
using SaintCoinach.Graphics.Viewer;
using SaintCoinach.Xiv;

namespace Godbert.ViewModels {

    using Commands;

    public class FurnitureViewModel : ObservableBase {
        #region Fields
        private HousingItem[] _AllFurniture;
        private HousingItem[] _FilteredFurniture;
        private HousingItem _SelectedFurniture;
        private Stain _SelectedStain;
        private string _FilterTerm;
        private SaintCoinach.Graphics.Viewer.Data.ParametersBase _Parameters = new SaintCoinach.Graphics.Viewer.Data.ParametersBase();
        #endregion

        #region Properties
        public MainViewModel Parent { get; private set; }
        public IEnumerable<HousingItem> AllFurniture { get { return _AllFurniture; } }
        public IEnumerable<HousingItem> FilteredFurniture { get { return _FilteredFurniture; } }
        public HousingItem SelectedFurniture {
            get { return _SelectedFurniture; }
            set {
                _SelectedFurniture = value;
                OnPropertyChanged(() => SelectedFurniture);
            }
        }
        public IEnumerable<Stain> Stains { get; private set; }
        public Stain SelectedStain {
            get { return _SelectedStain; }
            set {
                if (value == null || value.Key == 0)
                    _Parameters.Remove(SaintCoinach.Graphics.Viewer.Content.BgColorChangeMaterial.ColorParameterKey);
                else
                    _Parameters.Set(SaintCoinach.Graphics.Viewer.Content.BgColorChangeMaterial.ColorParameterKey, new SharpDX.Vector4(value.Color.R / 255f, value.Color.G / 255f, value.Color.B / 255f, value.Color.A / 255f));
                _SelectedStain = value;
                OnPropertyChanged(() => SelectedStain);
            }
        }
        public string FilterTerm {
            get { return _FilterTerm; }
            set {
                _FilterTerm = value;

                if (string.IsNullOrWhiteSpace(value))
                    _FilteredFurniture = _AllFurniture;
                else
                    _FilteredFurniture = _AllFurniture.Where(e => e.Item.Name.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();

                OnPropertyChanged(() => FilterTerm);
                OnPropertyChanged(() => FilteredFurniture);
            }
        }
        #endregion

        #region Constructor
        public FurnitureViewModel(MainViewModel parent) {
            this.Parent = parent;

            var indoor = Parent.Realm.GameData.GetSheet<HousingFurniture>();
            var outdoor = Parent.Realm.GameData.GetSheet<HousingYardObject>();
            _AllFurniture = indoor.Cast<HousingItem>().Concat(outdoor.Cast<HousingItem>()).Where(_ => _.Item != null && _.Item.Key != 0 && _.Item.Name.ToString().Length > 0).OrderBy(_ => _.Item.Name).ToArray();
            Stains = Parent.Realm.GameData.GetSheet<Stain>();

            _FilteredFurniture = _AllFurniture;
        }
        #endregion

        #region Command
        private ICommand _AddCommand;
        private ICommand _ReplaceCommand;
        private ICommand _NewCommand;

        public ICommand AddCommand { get { return _AddCommand ?? (_AddCommand = new DelegateCommand(OnAdd)); } }
        public ICommand ReplaceCommand { get { return _ReplaceCommand ?? (_ReplaceCommand = new DelegateCommand(OnReplace)); } }
        public ICommand NewCommand { get { return _NewCommand ?? (_NewCommand = new DelegateCommand(OnNew)); } }

        private void OnAdd() {
            SgbFile scene;
            if (TryGetModel(out scene))
                Parent.EngineHelper.AddToLast(SelectedFurniture.Item.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentSgb(e, scene, (SaintCoinach.Graphics.Viewer.Data.ParametersBase)_Parameters.Clone()));
        }
        private void OnReplace() {
            SgbFile scene;
            if (TryGetModel(out scene))
                Parent.EngineHelper.ReplaceInLast(SelectedFurniture.Item.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentSgb(e, scene, (SaintCoinach.Graphics.Viewer.Data.ParametersBase)_Parameters.Clone()));
        }
        private void OnNew() {
            SgbFile scene;
            if (TryGetModel(out scene))
                Parent.EngineHelper.OpenInNew(SelectedFurniture.Item.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentSgb(e, scene, (SaintCoinach.Graphics.Viewer.Data.ParametersBase)_Parameters.Clone()));
        }
        
        private bool TryGetModel(out SgbFile model) {
            model = null;
            if (SelectedFurniture == null)
                return false;

            try {
                model = SelectedFurniture.GetScene();

                var result = (model != null);
                if (!result)
                    System.Windows.MessageBox.Show(string.Format("Unable to find model for {0}.", SelectedFurniture.Item.Name), "Model not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return result;
            } catch (Exception e) {
                System.Windows.MessageBox.Show(string.Format("Failed to load model for {0}:{2}{3}", SelectedFurniture.Item.Name, Environment.NewLine, e), "Read failure", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }
        #endregion

        #region Refresh
        public void Refresh() {
            _FilteredFurniture = _FilteredFurniture.ToArray();
            OnPropertyChanged(() => FilteredFurniture);
        }
        #endregion
    }
}
