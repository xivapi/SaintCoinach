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


        #region Fields
        private Models.ModelCharaHierarchy _Entries;
        private object _SelectedEntry;
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
        public Models.ModelCharaHierarchy Entries {
            get { return _Entries; }
            private set {
                _Entries = value;
                OnPropertyChanged(() => Entries);
            }
        }
        public object SelectedEntry {
            get { return _SelectedEntry; }
            set {
                _SelectedEntry = value;
                OnPropertyChanged(() => SelectedEntry);
                OnPropertyChanged(() => IsValidSelection);
            }
        }
        public bool IsValidSelection { get { return SelectedEntry is Models.ModelCharaVariant; } }
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

            Entries = new Models.ModelCharaHierarchy("d{0:D4}", "e{0:D4}", "v{0:D4}");
            foreach (var mc in parent.Realm.GameData.GetSheet<ModelChara>().Where(mc => mc.Type == 2)) {
                var imcPath = string.Format(ImcPathFormat, mc.ModelKey, mc.BaseKey);
                if (parent.Realm.Packs.FileExists(imcPath))
                    Entries.Add(mc);
            }
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

            var asVariant = SelectedEntry as Models.ModelCharaVariant;
            if (asVariant == null)
                return false;

            title = asVariant.ToString();

            int v = asVariant.Value;
            int e = asVariant.Parent.Value;
            var d = asVariant.Parent.Parent.Value;

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

        #region Brute-force
        private bool _IsBruteForceAvailable = true;
        private ICommand _BruteForceCommand;

        public bool IsBruteForceAvailable {
            get { return _IsBruteForceAvailable; }
            private set {
                _IsBruteForceAvailable = value;
                OnPropertyChanged(() => IsBruteForceAvailable);
            }
        }
        public ICommand BruteForceCommand { get { return _BruteForceCommand ?? (_BruteForceCommand = new DelegateCommand(OnBruteForce)); } }

        private void OnBruteForce() {
            IsBruteForceAvailable = false;

            var progDlg = new Ookii.Dialogs.Wpf.ProgressDialog();
            progDlg.WindowTitle = "Brute-forcing";
            progDlg.Text = "This is going to take a while...";
            progDlg.DoWork += DoBruteForceWork;
            progDlg.RunWorkerCompleted += OnBruteForceComplete;
            progDlg.ShowDialog(System.Windows.Application.Current.MainWindow);
            progDlg.ProgressBarStyle = Ookii.Dialogs.Wpf.ProgressBarStyle.ProgressBar;
            progDlg.ShowTimeRemaining = true;
        }

        void OnBruteForceComplete(object sender, System.ComponentModel.RunWorkerCompletedEventArgs eventArgs) {
            if (eventArgs.Cancelled)
                IsBruteForceAvailable = true;
        }

        void DoBruteForceWork(object sender, System.ComponentModel.DoWorkEventArgs eventArgs) {
            var dlg = (Ookii.Dialogs.Wpf.ProgressDialog)sender;

            var newEntries = new Models.ModelCharaHierarchy(Entries.MainFormat, Entries.SubFormat, Entries.VariantFormat);
            for (var d = 0; d < 10000; ++d) {
                if (dlg.CancellationPending)
                    return;
                dlg.ReportProgress(d / 100, null, string.Format("Current progress: {0:P}", d / 10000.0));
                for (var e = 0; e < 10000; ++e) {
                    //dlg.ReportProgress(((d * 10000) + e) / (10000 * 100));

                    var imcPath = string.Format(ImcPathFormat, d, e);
                    SaintCoinach.IO.File imcBase;
                    if (!Parent.Realm.Packs.TryGetFile(imcPath, out imcBase))
                        continue;
                    try {
                        var imc = new SaintCoinach.Graphics.ImcFile(imcBase);
                        for (var v = 1; v < imc.Count; ++v) {
                            /*if (Entries.Contains(d, e, v))
                                continue;*/

                            var any = false;
                            foreach (var p in imc.Parts) {
                                if (p.Variants[v].Variant != 0) {
                                    any = true;
                                    break;
                                }
                            }
                            if (any)
                                newEntries.Add(d, e, v);
                        }
                    } catch (Exception ex) {
                        Console.Error.WriteLine("Failed parsing imc file {0}:{1}{2}", imcPath, Environment.NewLine, ex);
                    }
                }
            }
            Entries = newEntries;
        }
        #endregion
    }
}
