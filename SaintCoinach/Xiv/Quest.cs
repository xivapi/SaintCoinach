using System;
using System.Collections.Generic;
using System.Linq;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Quest : XivRow, IItemSource {
        #region Fields
        private QuestRequirements _Requirements;
        private QuestRewards _Rewards;
        private ENpc[] _InvolvedENpcs;
        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public Text.XivString Id { get { return AsString("Id"); } }

        public QuestRequirements Requirements { get { return _Requirements ?? (_Requirements = new QuestRequirements(this)); } }

        public BeastTribe BeastTribe { get { return As<BeastTribe>(); } }

        public ENpc IssuingENpc {
            get {
                var resident = this["Issuer{Start}"] as ENpcResident;
                return resident == null ? null : Sheet.Collection.ENpcs[resident.Key];
            }
        }
        public ENpc TargetENpc {
            get {
                var resident = this["Target{End}"] as ENpcResident;
                return resident == null ? null : Sheet.Collection.ENpcs[resident.Key];
            }
        }

        public bool IsRepeatable { get { return AsBoolean("IsRepeatable"); } }
        public QuestRepeatInterval RepeatInterval { get { return (QuestRepeatInterval)AsInt32("RepeatIntervalType"); } }

        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        public JournalGenre JournalGenre { get { return As<JournalGenre>(); } }

        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public Imaging.ImageFile SpecialIcon { get { return AsImage("Icon{Special}"); } }

        public int SortKey { get { return AsInt32("SortKey"); } }

        public QuestRewards Rewards { get { return _Rewards ?? (_Rewards = new QuestRewards(this)); } }

        public IEnumerable<ENpc> InvolvedENpcs { get { return _InvolvedENpcs ?? (_InvolvedENpcs = BuildInvolvedENpcs()); } }
        #endregion

        #region Constructors

        public Quest(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }

        #region IItemSource Members

        private Item[] _ItemSourceItems;
        System.Collections.Generic.IEnumerable<Item> IItemSource.Items {
            get {
                return _ItemSourceItems ?? (_ItemSourceItems =  Rewards.Items.SelectMany(g => g.Items.Select(i => i.Item)).ToArray());
            }
        }

        #endregion

        #region Build
        private ENpc[] BuildInvolvedENpcs() {
            var enpcs = new List<ENpc> { IssuingENpc };
            if (Sheet.Collection.IsLibraAvailable) {
                var enpcColl = Sheet.Collection.ENpcs;
                var libraRows = Sheet.Collection.Libra.ENpcResident_Quest.Where(i => i.Quest_Key == this.Key);
                foreach (var r in libraRows)
                    enpcs.Add(enpcColl[(int)r.ENpcResident_Key]);
            }

            enpcs.Add(TargetENpc);

            return enpcs.Distinct().ToArray();
        }
        #endregion
    }
}
