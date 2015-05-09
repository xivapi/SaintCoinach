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

    public class MonstersViewModel : ObservableBase {
        const string ImcPathFormat = "chara/monster/m{0:D4}/obj/body/b{1:D4}/b{1:D4}.imc";
        const string ModelPathFormat = "chara/monster/m{0:D4}/obj/body/b{1:D4}/model/m{0:D4}b{1:D4}.mdl";

        public class MonsterVariant {
            public MonsterBody Body;
            public int Variant;

            public string Display { get { return string.Format("v{0:D4}", Variant); } }

            public override string ToString() {
                return string.Format("{0} / v{1:D4}", Body, Variant);
            }
        }
        public class MonsterBody : List<MonsterVariant> {
            public MonsterModel Monster;
            public int Body;

            public string ImcPath;
            public string ModelPath;

            public string Display { get { return string.Format("b{0:D4}", Body); } }

            public override string ToString() {
                return string.Format("{0} / b{1:D4}", Monster, Body);
            }
        }
        public class MonsterModel : List<MonsterBody> {
            public int Monster;

            public string Display { get { return string.Format("m{0:D4}", Monster); } }

            public override string ToString() {
                return string.Format("m{0:D4}", Monster);
            }
        }
        

        #region Fields
        private object _SelectedEntry;
        #endregion

        #region Properties
        public IEnumerable<MonsterModel> Entries { get; private set; }
        public object SelectedEntry {
            get { return _SelectedEntry; }
            set {
                _SelectedEntry = value;
                OnPropertyChanged(() => SelectedEntry);
                OnPropertyChanged(() => IsValidSelection);
            }
        }
        public bool IsValidSelection { get { return SelectedEntry is MonsterVariant; } }
        public MainViewModel Parent { get; private set; }
        #endregion

        #region Constructor
        public MonstersViewModel(MainViewModel parent) {
            this.Parent = parent;

            var modelCharaSheet = Parent.Realm.GameData.GetSheet<ModelChara>();

            var allEntries = new List<MonsterModel>();
            foreach(var mc in modelCharaSheet.Where(mc => mc.Type == 3).Select(mc => new {
                        Model = mc.ModelKey,
                        Body = mc.BaseKey,
                        Variant = mc.Variant
                    }).OrderBy(e => e.Variant).OrderBy(e => e.Body).OrderBy(e => e.Model)) {
                var model = allEntries.FirstOrDefault(m => m.Monster == mc.Model);
                if (model == null)
                    allEntries.Add(model = new MonsterModel { Monster = mc.Model });
                var body = model.FirstOrDefault(b => b.Body == mc.Body);
                if (body == null)
                    model.Add(body = new MonsterBody {
                        Body = mc.Body,
                        Monster = model,
                        ImcPath = string.Format(ImcPathFormat, mc.Model, mc.Body),
                        ModelPath = string.Format(ModelPathFormat, mc.Model, mc.Body)
                    });
                if (!body.Any(b => b.Variant == mc.Variant))
                    body.Add(new MonsterVariant { Variant = mc.Variant, Body = body });
            }

            foreach (var m in allEntries.ToArray()) {
                foreach (var b in m.ToArray()) {
                    if (!Parent.Realm.Packs.FileExists(b.ImcPath) || !Parent.Realm.Packs.FileExists(b.ModelPath)) {
                        System.Diagnostics.Trace.WriteLine(string.Format("File does not exist for monster model '{0}'.", b));
                        m.Remove(b);
                    }
                }
                if (m.Count == 0)
                    allEntries.Remove(m);
            }
            Entries = allEntries;
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
            ImcVariant variant;
            if (TryGetModel(out model, out variant))
                Parent.EngineHelper.AddToLast(SelectedEntry.ToString(), (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }
        private void OnReplace() {
            ModelDefinition model;
            ImcVariant variant;
            if (TryGetModel(out model, out variant))
                Parent.EngineHelper.ReplaceInLast(SelectedEntry.ToString(), (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }
        private void OnNew() {
            ModelDefinition model;
            ImcVariant variant;
            if (TryGetModel(out model, out variant))
                Parent.EngineHelper.OpenInNew(SelectedEntry.ToString(), (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, variant, model, ModelQuality.High));
        }

        private bool TryGetModel(out ModelDefinition model, out ImcVariant variant) {
            model = null;
            variant = ImcVariant.Default;

            var asVariant = SelectedEntry as MonsterVariant;
            if (asVariant == null)
                return false;

            int v = asVariant.Variant;
            int b = asVariant.Body.Body;
            var m = asVariant.Body.Monster.Monster;

            var imcPath = asVariant.Body.ImcPath;
            var mdlPath = asVariant.Body.ModelPath;

            SaintCoinach.IO.File imcFileBase;
            SaintCoinach.IO.File mdlFileBase;
            if (!Parent.Realm.Packs.TryGetFile(imcPath, out imcFileBase) || !Parent.Realm.Packs.TryGetFile(mdlPath, out mdlFileBase) || !(mdlFileBase is ModelFile)) {
                System.Windows.MessageBox.Show(string.Format("Unable to find files for {0}.", asVariant), "File not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }

            try {
                var imcFile = new ImcFile(imcFileBase);
                model = ((ModelFile)mdlFileBase).GetModelDefinition();
                variant = imcFile.GetVariant(v);

                return true;
            } catch (Exception e) {
                System.Windows.MessageBox.Show(string.Format("Unable to load model for {0}:{1}{2}", asVariant, Environment.NewLine, e), "Failure to load", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }
        #endregion
    }
}
