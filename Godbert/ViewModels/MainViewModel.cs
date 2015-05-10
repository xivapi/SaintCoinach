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
        public MonstersViewModel Monsters { get; private set; }
        public MapsViewModel Maps { get; private set; }
        public DemihumanViewModel Demihuman { get; private set; }
        public DataViewModel Data { get; private set; }

        public bool IsEnglish { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.English; } }
        public bool IsJapanese { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.Japanese; } }
        public bool IsFrench { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.French; } }
        public bool IsGerman { get { return Realm.GameData.ActiveLanguage == SaintCoinach.Ex.Language.German; } }
        #endregion

        #region Constructor
        public MainViewModel() {
            Realm = new ARealmReversed(Properties.Settings.Default.GamePath, SaintCoinach.Ex.Language.English);
            EngineHelper = new EngineHelper();
            Equipment = new EquipmentViewModel(this);
            Monsters = new MonstersViewModel(this);
            Maps = new MapsViewModel(this);
            Demihuman = new DemihumanViewModel(this);
            Data = new DataViewModel(Realm);
        }
        #endregion

        #region Commands
        private ICommand _LanguageCommand;
        public ICommand LanguageCommand { get { return _LanguageCommand ?? (_LanguageCommand = new Commands.DelegateCommand<SaintCoinach.Ex.Language>(OnLanguage)); } }

        private void OnLanguage(SaintCoinach.Ex.Language newLanguage) {
            Realm.GameData.ActiveLanguage = newLanguage;

            OnPropertyChanged(() => IsEnglish);
            OnPropertyChanged(() => IsJapanese);
            OnPropertyChanged(() => IsGerman);
            OnPropertyChanged(() => IsFrench);

            Equipment.Refresh();
            Maps.Refresh();
        }
        #endregion
    }
}
