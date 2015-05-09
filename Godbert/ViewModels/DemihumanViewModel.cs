using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SaintCoinach.Graphics;
using SaintCoinach.Graphics.Viewer;
using SaintCoinach.Graphics.Viewer.Content;
using SaintCoinach.Xiv;

namespace Godbert.ViewModels {
    using Commands;

    public class DemihumanViewModel : ObservableBase {
        const string ImcPathFormat = "chara/demihuman/d{0:D4}/obj/equipment/e{1:D4}/e{1:D4}.imc";
        static readonly string[] ModelPathFormats = new string[] {
            "chara/demihuman/d{0:D4}/obj/equipment/e{1:D4}/model/d{0:D4}e{1:D4}_met.mdl",
            "chara/demihuman/d{0:D4}/obj/equipment/e{1:D4}/model/d{0:D4}e{1:D4}_top.mdl",
            "chara/demihuman/d{0:D4}/obj/equipment/e{1:D4}/model/d{0:D4}e{1:D4}_glv.mdl",
            "chara/demihuman/d{0:D4}/obj/equipment/e{1:D4}/model/d{0:D4}e{1:D4}_dwn.mdl",
            "chara/demihuman/d{0:D4}/obj/equipment/e{1:D4}/model/d{0:D4}e{1:D4}_sho.mdl",
        };

        public class ModelCharaWrapper {
            public ModelChara ModelChara;

            public override string ToString() {
                return string.Format("d{0:D4} / e{1:D4} / v{2:D4}", ModelChara.ModelKey, ModelChara.BaseKey, ModelChara.Variant); 
            }
        }

        #region Fields
        private ModelCharaWrapper _SelectedItem;
        private bool[] _SelectedParts = new bool[] {
            true,
            true,
            true,
            true,
            true
        };
        #endregion

        #region Properties
        public MainViewModel Parent { get; private set; }
        public IEnumerable<ModelCharaWrapper> Items { get; private set; }
        public ModelCharaWrapper SelectedItem {
            get { return _SelectedItem; }
            set {
                _SelectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        public bool ShowPart0 {
            get { return _SelectedParts[0]; }
            set {
                _SelectedParts[0] = value;
                OnPropertyChanged(() => ShowPart0);
            }
        }
        public bool ShowPart1 {
            get { return _SelectedParts[1]; }
            set {
                _SelectedParts[1] = value;
                OnPropertyChanged(() => ShowPart0);
            }
        }
        public bool ShowPart2 {
            get { return _SelectedParts[2]; }
            set {
                _SelectedParts[2] = value;
                OnPropertyChanged(() => ShowPart0);
            }
        }
        public bool ShowPart3 {
            get { return _SelectedParts[3]; }
            set {
                _SelectedParts[3] = value;
                OnPropertyChanged(() => ShowPart3);
            }
        }
        public bool ShowPart4 {
            get { return _SelectedParts[4]; }
            set {
                _SelectedParts[4] = value;
                OnPropertyChanged(() => ShowPart4);
            }
        }
        #endregion

        #region Constructor
        public DemihumanViewModel(MainViewModel parent) {
            this.Parent = parent;

            List<ModelChara> items = new List<ModelChara>();
            foreach (var mc in parent.Realm.GameData.GetSheet<ModelChara>().Where(mc => mc.Type == 2)) {
                var imcPath = string.Format(ImcPathFormat, mc.ModelKey, mc.BaseKey);
                if (parent.Realm.Packs.FileExists(imcPath))
                    items.Add(mc);
            }
            this.Items = items.OrderBy(mc => mc.Variant).OrderBy(mc => mc.BaseKey).OrderBy(mc => mc.ModelKey).Select(mc => new ModelCharaWrapper { ModelChara = mc }).ToArray();
        }
        #endregion

        #region Commands
        private ICommand _AddCommand;
        private ICommand _ReplaceCommand;
        private ICommand _NewCommand;

        public ICommand AddCommand { get { return _AddCommand ?? (_AddCommand = new DelegateCommand(OnAdd)); } }
        public ICommand ReplaceCommand { get { return _ReplaceCommand ?? (_ReplaceCommand = new DelegateCommand(OnReplace)); } }
        public ICommand NewCommand { get { return _NewCommand ?? (_NewCommand = new DelegateCommand(OnNew)); } }

        private void OnAdd() {
            string title;
            Tuple<ModelDefinition, ImcVariant>[] models;
            if (TryGetModel(out title, out models))
                Parent.EngineHelper.AddToLast(title, (e) => models.Select(m => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, m.Item2, m.Item1, ModelQuality.High)).ToArray());
        }
        private void OnReplace() {
            string title;
            Tuple<ModelDefinition, ImcVariant>[] models;
            if (TryGetModel(out title, out models))
                Parent.EngineHelper.ReplaceInLast(title, (e) => models.Select(m => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, m.Item2, m.Item1, ModelQuality.High)).ToArray());
        }
        private void OnNew() {
            string title;
            Tuple<ModelDefinition, ImcVariant>[] models;
            if (TryGetModel(out title, out models))
                Parent.EngineHelper.OpenInNew(title, (e) => models.Select(m => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, m.Item2, m.Item1, ModelQuality.High)).ToArray());
        }

        private bool TryGetModel(out string title, out Tuple<ModelDefinition, ImcVariant>[] models) {
            title = null;
            models = null;

            if (SelectedItem == null)
                return false;

            var modelChara = SelectedItem.ModelChara;

            title = string.Format("d{0:D4} / e{1:D4} / v{2:D4}", modelChara.ModelKey, modelChara.BaseKey, modelChara.Variant); 

            int v = modelChara.Variant;
            int e = modelChara.BaseKey;
            var d = modelChara.ModelKey;

            var imcPath = string.Format(ImcPathFormat, d, e);

            SaintCoinach.IO.File imcFileBase;
            if (!Parent.Realm.Packs.TryGetFile(imcPath, out imcFileBase)) {
                System.Windows.MessageBox.Show(string.Format("Unable to find files for {0}.", title), "File not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }

            try {
                var imcFile = new ImcFile(imcFileBase);
                var modelsList = new List<Tuple<ModelDefinition, ImcVariant>>();
                foreach(var part in imcFile.Parts) {
                    if (!_SelectedParts[part.Bit])
                        continue;

                    var variant = part.Variants[v];
                    if(variant.Variant == 0)
                        continue;

                    var mdlPath = string.Format(ModelPathFormats[part.Bit], d, e);
                    SaintCoinach.IO.File mdlBase;
                    if(!Parent.Realm.Packs.TryGetFile(mdlPath, out mdlBase))
                        continue;
                    var mdl = ((ModelFile)mdlBase).GetModelDefinition();

                    modelsList.Add(Tuple.Create(mdl, variant));
                }

                if(modelsList.Count == 0) {
                    System.Windows.MessageBox.Show(string.Format("No models found for {0}.", title), "No models", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return false;
                }
                models = modelsList.ToArray();

                return true;
            } catch (Exception ex) {
                System.Windows.MessageBox.Show(string.Format("Unable to load model for {0}:{1}{2}", title, Environment.NewLine, ex), "Failure to load", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }
        #endregion
    }
}
