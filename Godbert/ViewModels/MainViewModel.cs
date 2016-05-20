using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace Godbert.ViewModels {
    using Commands;

    using SaintCoinach;
    using SaintCoinach.Xiv;

    public class MainViewModel : ObservableBase {
        #region Properties
        public ARealmReversed Realm { get; private set; }
        public EngineHelper EngineHelper { get; private set; }
        public EquipmentViewModel Equipment { get; private set; }
        public FurnitureViewModel Furniture { get; private set; }
        public MonstersViewModel Monsters { get; private set; }
        public TerritoryViewModel Territories { get; private set; }
        public DemihumanViewModel Demihuman { get; private set; }
        public DataViewModel Data { get; private set; }

        public bool IsEnglish { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.English; } }
        public bool IsJapanese { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.Japanese; } }
        public bool IsFrench { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.French; } }
        public bool IsGerman { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.German; } }
        #endregion

        #region Constructor
        public MainViewModel() {
            if (!App.IsValidGamePath(Properties.Settings.Default.GamePath))
                return;
            Realm = new ARealmReversed(Properties.Settings.Default.GamePath, SaintCoinach.Ex.Language.English);
            EngineHelper = new EngineHelper();
            Equipment = new EquipmentViewModel(this);
            Furniture = new FurnitureViewModel(this);
            Monsters = new MonstersViewModel(this);
            Territories = new TerritoryViewModel(this);
            Demihuman = new DemihumanViewModel(this);
            Data = new DataViewModel(Realm);
        }
        #endregion

        #region Commands
        private ICommand _LanguageCommand;
        private ICommand _GameLocationCommand;

        public ICommand LanguageCommand { get { return _LanguageCommand ?? (_LanguageCommand = new Commands.DelegateCommand<SaintCoinach.Ex.Language>(OnLanguage)); } }
        public ICommand GameLocationCommand { get { return _GameLocationCommand ?? (_GameLocationCommand = new Commands.DelegateCommand(OnGameLocation)); } }

        private void OnLanguage(SaintCoinach.Ex.Language newLanguage) {
            Realm.GameData.ActiveLanguage = newLanguage;

            OnPropertyChanged(() => IsEnglish);
            OnPropertyChanged(() => IsJapanese);
            OnPropertyChanged(() => IsGerman);
            OnPropertyChanged(() => IsFrench);

            Equipment.Refresh();
            Territories.Refresh();
        }

        private void OnGameLocation() {
            var res = System.Windows.MessageBox.Show("If you continue the application will restart and ask you for the game's installation directory, do you want to continue?", "Change game location", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Exclamation);
            if (res == System.Windows.MessageBoxResult.Yes) {
                Properties.Settings.Default.GamePath = null;
                Properties.Settings.Default.Save();
               
                var current = System.Diagnostics.Process.GetCurrentProcess();
                var startInfo = new System.Diagnostics.ProcessStartInfo(current.MainModule.FileName);
                startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                System.Diagnostics.Process.Start(startInfo);
                App.Current.Shutdown();
            }
        }
        #endregion
    }
}
