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
            set { _SelectedStain = value; }
        }
        public string FilterTerm {
            get { return _FilterTerm; }
            set {
                _FilterTerm = value;

                if (string.IsNullOrWhiteSpace(value))
                    _FilteredEquipment = _AllEquipment;
                else
                    _FilteredEquipment = _AllEquipment.Where(e => e.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();

                OnPropertyChanged(() => FilterTerm);
                OnPropertyChanged(() => FilteredEquipment);
            }
        }
        #endregion

        #region Constructor
        public EquipmentViewModel(MainViewModel parent) {
            this.Parent = parent;

            // Ignore waist and soul crystals
            _AllEquipment = Parent.Realm.GameData.GetSheet<Item>().OfType<Equipment>().Where(e => !e.EquipSlotCategory.PossibleSlots.Any(s => s.Key == 5 || s.Key == 13)).ToArray();
            Stains = Parent.Realm.GameData.GetSheet<Stain>();
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
            ModelDefinition model;
            ModelVariantIdentifier variant;
            if (TryGetModel(out model, out variant))
                Parent.EngineHelper.AddToLast(SelectedEquipment.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }
        private void OnReplace() {
            ModelDefinition model;
            ModelVariantIdentifier variant;
            if (TryGetModel(out model, out variant))
                Parent.EngineHelper.ReplaceInLast(SelectedEquipment.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }
        private void OnNew() {
            ModelDefinition model;
            ModelVariantIdentifier variant;
            if (TryGetModel(out model, out variant))
                Parent.EngineHelper.OpenInNew(SelectedEquipment.Name, (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }

        private bool TryGetModel(out ModelDefinition model, out ModelVariantIdentifier variant) {
            model = null;
            variant = default(ModelVariantIdentifier);
            if (SelectedEquipment == null)
                return false;

            model = SelectedEquipment.GetModel(101, out variant.ImcVariant);
            if (SelectedEquipment.IsDyeable && SelectedStain != null)
                variant.StainKey = SelectedStain.Key;

            return (model != null);
        }
        #endregion
    }
}
