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
        #endregion

        #region Constructor
        public MainViewModel() {
            Realm = new ARealmReversed(Properties.Settings.Default.GamePath, SaintCoinach.Ex.Language.English);
            EngineHelper = new EngineHelper();
            Equipment = new EquipmentViewModel(this);
            Monsters = new MonstersViewModel(this);
            Maps = new MapsViewModel(this);
        }
        #endregion

        #region Commands
        private ICommand _TestCommand;
        public ICommand TestCommand { get { return _TestCommand ?? (_TestCommand = new DelegateCommand(OnTest)); } }


        static string[] TestItems = new string[] {
            "Lionliege Breeches",
            "Lionliege Cuirass",
            "Lionliege Sabatons",
            "Lionliege Armet",
            "Lionliege Gauntlets",
        };
        int TestItemIndex = 0;
        private void OnTest() {
            if (TestItemIndex >= TestItems.Length)
                return;

            var eq = (SaintCoinach.Xiv.Items.Equipment)Realm.GameData.GetSheet<SaintCoinach.Xiv.Item>().First(i => i.Name == TestItems[TestItemIndex]);
            TestItemIndex++;
            SaintCoinach.Graphics.ImcVariant imcVar;
            var mdlDef = eq.GetModel(101, out imcVar);
            var mdlVar = new SaintCoinach.Graphics.ModelVariantIdentifier {
                ImcVariant = imcVar,
                StainKey = 14
            };
            EngineHelper.AddToLast("Lionsmane", (e) => new SaintCoinach.Graphics.Viewer.Content.ContentModel(e, mdlVar, mdlDef, SaintCoinach.Graphics.ModelQuality.High));
        }
        #endregion
    }
}
