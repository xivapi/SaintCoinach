using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SaintCoinach.Graphics;
using SaintCoinach.Graphics.Viewer;
using SaintCoinach.Xiv;
using SaintCoinach.Xiv.Items;

namespace Godbert.ViewModels {

    using Commands;

    public class EquipmentViewModel : ObservableBase {
        #region Fields
        private Equipment[] _AllEquipment;
        private Equipment[] _FilteredEquipment;
        private Equipment _SelectedEquipment;
        private Stain _SelectedStain;
        private string _FilterTerm;
        #endregion

        #region Properties
        public MainViewModel Parent { get; private set; }
        public IEnumerable<Equipment> AllEquipment { get { return _AllEquipment; } }
        public IEnumerable<Equipment> FilteredEquipment { get { return _FilteredEquipment; } }
        public Equipment SelectedEquipment {
            get { return _SelectedEquipment; }
            set {
                _SelectedEquipment = value;
                OnPropertyChanged(() => SelectedEquipment);
            }
        }
        public IEnumerable<Stain> Stains { get; private set; }
        public Stain SelectedStain {
            get { return _SelectedStain; }
            set {
                _SelectedStain = value;
                OnPropertyChanged(() => SelectedStain);
            }
        }
        public string FilterTerm {
            get { return _FilterTerm; }
            set {
                _FilterTerm = value;

                if (string.IsNullOrWhiteSpace(value))
                    _FilteredEquipment = _AllEquipment;
                else
                    _FilteredEquipment = _AllEquipment.Where(e => e.Name.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();

                OnPropertyChanged(() => FilterTerm);
                OnPropertyChanged(() => FilteredEquipment);
            }
        }
        #endregion

        #region Constructor
        public EquipmentViewModel(MainViewModel parent) {
            this.Parent = parent;

            // Ignore waist and soul crystals
            _AllEquipment = Parent.Realm.GameData.GetSheet<Item>().OfType<Equipment>().Where(e => !e.EquipSlotCategory.PossibleSlots.Any(s => s.Key == 5 || s.Key == 13)).OrderBy(e => e.Name).ToArray();
            Stains = Parent.Realm.GameData.GetSheet<Stain>();

            _FilteredEquipment = _AllEquipment;
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
            if (TryGetModel(out var model, out var variant))
                Parent.EngineHelper.AddToLast(SelectedEquipment.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }
        private void OnReplace() {
            if (TryGetModel(out var model, out var variant))
                Parent.EngineHelper.ReplaceInLast(SelectedEquipment.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }
        private void OnNew() {
            if (TryGetModel(out var model, out var variant))
                Parent.EngineHelper.OpenInNew(SelectedEquipment.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }

        private bool TryGetModel(out ModelDefinition model, out ModelVariantIdentifier variant) {
            model = null;
            variant = default(ModelVariantIdentifier);
            if (SelectedEquipment == null)
                return false;

            var charType = SelectedEquipment.GetModelCharacterType();
            if (charType == 0)
                return false;
            try {
                model = SelectedEquipment.GetModel(charType, out variant.ImcVariant);
                if (SelectedEquipment.IsDyeable && SelectedStain != null)
                    variant.StainKey = SelectedStain.Key;

                var result = (model != null);
                if (!result)
                    System.Windows.MessageBox.Show(string.Format("Unable to find model for {0} (c{1:D4}).", SelectedEquipment.Name, charType), "Model not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return result;
            } catch (Exception e) {
                System.Windows.MessageBox.Show(string.Format("Failed to load model for {0} (c{1:D4}):{2}{3}", SelectedEquipment.Name, charType, Environment.NewLine, e), "Read failure", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }
        #endregion

        #region Refresh
        public void Refresh() {
            _FilteredEquipment = _FilteredEquipment.ToArray();
            OnPropertyChanged(() => FilteredEquipment);
        }
        #endregion
    }
}
