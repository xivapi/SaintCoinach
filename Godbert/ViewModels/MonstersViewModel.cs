using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using SaintCoinach.Graphics;
using SaintCoinach.Graphics.Viewer;
using SaintCoinach.Graphics.Viewer.Content;
using SaintCoinach.Graphics.Viewer.Interop;
using SaintCoinach.IO;
using SaintCoinach.Xiv;
using SaintCoinach.Xiv.ItemActions;
using Directory = System.IO.Directory;
using File = SaintCoinach.IO.File;
using ProgressBarStyle = Ookii.Dialogs.Wpf.ProgressBarStyle;

namespace Godbert.ViewModels {
    using Commands;

    public class MonstersViewModel : ObservableBase {
        const string ImcPathFormat = "chara/monster/m{0:D4}/obj/body/b{1:D4}/b{1:D4}.imc";
        const string ModelPathFormat = "chara/monster/m{0:D4}/obj/body/b{1:D4}/model/m{0:D4}b{1:D4}.mdl";
        const string SkeletonPathFormat = "chara/monster/m{0:D4}/skeleton/base/b{1:D4}/skl_m{0:D4}b{1:D4}.sklb";
        const string PapPathFormat = "chara/monster/m{0:D4}/animation/a0001/bt_common/resident/monster.pap";

        #region Fields
        private Models.ModelCharaHierarchy _Entries;
        private object _SelectedEntry;

        private bool _IsExporting = false;

        public bool IsExporting {
            get { return _IsExporting; }
            private set {
                _IsExporting = value;
                OnPropertyChanged(() => IsExporting);
            }
        }
        #endregion

        #region Properties
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
        public MainViewModel Parent { get; private set; }
        #endregion

        #region Constructor
        public MonstersViewModel(MainViewModel parent) {
            this.Parent = parent;

            var modelCharaSheet = Parent.Realm.GameData.GetSheet<ModelChara>();

            Entries = new Models.ModelCharaHierarchy("m{0:D4}", "b{0:D4}", "v{0:D4}");
            foreach(var mc in modelCharaSheet.Where(mc => mc.Type == 3)) {
                var imcPath = string.Format(ImcPathFormat, mc.ModelKey, mc.BaseKey);
                var mdlPath = string.Format(ModelPathFormat, mc.ModelKey, mc.BaseKey);
                if(!Parent.Realm.Packs.FileExists(imcPath) ||!Parent.Realm.Packs.FileExists(mdlPath))
                    continue;
                Entries.Add(mc);
            }
        }
        #endregion

        #region Command
        private ICommand _AddCommand;
        private ICommand _ReplaceCommand;
        private ICommand _ExportCommand;
        private ICommand _NewCommand;

        public ICommand AddCommand { get { return _AddCommand ?? (_AddCommand = new DelegateCommand(OnAdd)); } }
        public ICommand ReplaceCommand { get { return _ReplaceCommand ?? (_ReplaceCommand = new DelegateCommand(OnReplace)); } }
        public ICommand ExportCommand { get { return _ExportCommand ?? (_ExportCommand = new DelegateCommand(OnExport)); } }
        public ICommand NewCommand { get { return _NewCommand ?? (_NewCommand = new DelegateCommand(OnNew)); } }

        private void OnAdd() {
            Skeleton skele;
            ModelDefinition model;
            ImcVariant variant;
            int m, b;
            if (TryGetModel(out skele, out model, out variant, out m, out b))
                Parent.EngineHelper.AddToLast(SelectedEntry.ToString(), (e) => CreateModel(e, skele, model, variant, m, b));
        }
        private void OnReplace() {
            Skeleton skele;
            ModelDefinition model;
            ImcVariant variant;
            int m, b;
            if (TryGetModel(out skele, out model, out variant, out m, out b))
                Parent.EngineHelper.ReplaceInLast(SelectedEntry.ToString(), (e) => CreateModel(e, skele, model, variant, m, b));
        }
        private void OnExport()
        {
            Skeleton skele;
            ModelDefinition model;
            ImcVariant variant;
            int m, b;
            if (!TryGetModel(out skele, out model, out variant, out m, out b)) return;

            List<PapFile> paps = SearchPaps(model.File.Path, m);

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                Description = "Select folder to export to",
                UseDescriptionForTitle = true
            };

            bool? result = dialog.ShowDialog();

            if (result.HasValue && result.Value && !string.IsNullOrEmpty(dialog.SelectedPath))
            {
                Task.Run(() =>
                {
                    string identifier = SelectedEntry.ToString().Replace(" / ", "_");
                    string folderName = Path.Combine(dialog.SelectedPath, identifier);
                    Directory.CreateDirectory(folderName);

                    string fileName = Path.Combine(folderName, identifier + ".fbx");

                    // Set IsExporting for feedback
                    IsExporting = true;
                    int exportResult = FbxExport.ExportFbx(fileName, model.GetModel(0).Meshes, skele, paps);
                    FbxExport.ExportMonsterMaterials(Parent.Realm, folderName, model.GetModel(0).Definition.Materials, variant);
                    IsExporting = false;
                    
                    if (exportResult == 0)
                        System.Windows.MessageBox.Show("The export of " + Path.GetFileName(fileName) + " has completed.",
                            "Export Complete",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information,
                            MessageBoxResult.OK,
                            System.Windows.MessageBoxOptions.DefaultDesktopOnly);
                    else
                        System.Windows.MessageBox.Show("The export of " + Path.GetFileName(fileName) + " has failed.",
                            "Export Failed",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error,
                            MessageBoxResult.OK,
                            System.Windows.MessageBoxOptions.DefaultDesktopOnly);
                });
            }
        }
        private void OnNew() {
            Skeleton skele;
            ModelDefinition model;
            ImcVariant variant;
            int m, b;
            if (TryGetModel(out skele, out model, out variant, out m, out b))
                Parent.EngineHelper.OpenInNew(SelectedEntry.ToString(), (e) => CreateModel(e, skele, model, variant, m, b));
        }

        static string[] DefaultAnimationNames = { "cbnm_id0", "cbbm_id0" };

        private List<PapFile> SearchPaps(string modelPath, int m)
        {
            /*  The files found in these paths will all be exported
             *  If more paths are found, you can add them here
             *  A good starting point might be attempting to determine how
             *  a0002 animations are decided?   */
            string[] searchPaths =
            {
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/mon_sp/m{0:D4}",
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/mon_sp/m{0:D4}/hide",
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/mon_sp/m{0:D4}/show",
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/event",
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/minion",
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/resident",
                "chara/monster/m{0:D4}/animation/a{1:D4}/bt_common/specialpop"
            };

            SaintCoinach.IO.Directory d;
            List<PapFile> allFiles = new List<PapFile>();

            // Animations are in the same pack as the model itself
            Pack p = Parent.Realm.Packs.GetPack(PackIdentifier.Get(modelPath));

            foreach (string path in searchPaths) {
                string currentFullPath = string.Format(path, m, 1);
                ((IndexSource)p.Source).TryGetDirectory(currentFullPath, out d);

                if (d == null)
                    continue;

                IEnumerator<File> dirEnumerator = d.GetEnumerator();
                while (dirEnumerator.MoveNext())
                    allFiles.Add(new PapFile(dirEnumerator.Current));
                dirEnumerator.Dispose();
            }
            return allFiles;
        }

        private IComponent CreateModel(Engine engine, Skeleton skeleton, ModelDefinition model, ImcVariant variant, int m, int b) {
            
            var component = new AnimatedModel(engine, skeleton, variant, model, ModelQuality.High) {};
            var papPath = string.Format(PapPathFormat, m, b);

            SaintCoinach.IO.File papFileBase;
            if (Parent.Realm.Packs.TryGetFile(papPath, out papFileBase)) {
                var anim = new AnimationContainer(skeleton, new PapFile(papFileBase));

                var hasAnim = false;
                for(var i = 0; i < DefaultAnimationNames.Length && !hasAnim; ++i) {
                    var n = DefaultAnimationNames[i];
                    if (anim.AnimationNames.Contains(n)) {
                        component.AnimationPlayer.Animation = anim.Get(n);
                        hasAnim = true;
                    }
                }
                
                if (!hasAnim)
                    component.AnimationPlayer.Animation = anim.Get(0);
            }
            return component;
        }

        private bool TryGetModel(out Skeleton skeleton, out ModelDefinition model, out ImcVariant variant, out int m, out int b) {
            model = null;
            skeleton = null;
            variant = ImcVariant.Default;
            m = 0;
            b = 0;

            var asVariant = SelectedEntry as Models.ModelCharaVariant;
            if (asVariant == null)
                return false;

            int v = asVariant.Value;
            b = asVariant.Parent.Value;
            m = asVariant.Parent.Parent.Value;

            var imcPath = string.Format(ImcPathFormat, m, b);
            var mdlPath = string.Format(ModelPathFormat, m, b);
            var sklPath = string.Format(SkeletonPathFormat, m, 1);// b);

            SaintCoinach.IO.File imcFileBase;
            SaintCoinach.IO.File mdlFileBase;
            if (!Parent.Realm.Packs.TryGetFile(imcPath, out imcFileBase) || !Parent.Realm.Packs.TryGetFile(mdlPath, out mdlFileBase) || !(mdlFileBase is ModelFile)) {
                System.Windows.MessageBox.Show(string.Format("Unable to find files for {0}.", asVariant), "File not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
            
            SaintCoinach.IO.File sklFileBase;
            if(!Parent.Realm.Packs.TryGetFile(sklPath, out sklFileBase)) {
                System.Windows.MessageBox.Show(string.Format("Unable to find skeleton for {0}.", asVariant), "File not found", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }

            skeleton = new Skeleton(new SklbFile(sklFileBase));

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
            for (var m = 0; m < 10000; ++m) {
                if (dlg.CancellationPending)
                    return;
                dlg.ReportProgress(m / 100, null, string.Format("Current progress: {0:P}", m / 10000.0));
                for (var b = 0; b < 10000; ++b) {

                    var imcPath = string.Format(ImcPathFormat, m, b);
                    SaintCoinach.IO.File imcBase;
                    if (!Parent.Realm.Packs.TryGetFile(imcPath, out imcBase))
                        continue;
                    try {
                        var imc = new SaintCoinach.Graphics.ImcFile(imcBase);
                        for (var v = 1; v < imc.Count; ++v) {
                            if (Entries.Contains(m, b, v))
                                continue;

                            var any = false;
                            foreach (var p in imc.Parts) {
                                if (p.Variants[v].Variant != 0) {
                                    any = true;
                                    break;
                                }
                            }
                            if (any)
                                newEntries.Add(m, b, v);
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
