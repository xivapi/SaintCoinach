using System.Linq;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Quest : XivRow, IItemSource {
        #region Fields
        private QuestRequirements _Requirements;
        private QuestRewards _Rewards;
        #endregion

        #region Properties

        public string Name { get { return AsString("Name"); } }

        public string Id { get { return AsString("Id"); } }

        public QuestRequirements Requirements { get { return _Requirements ?? (_Requirements = new QuestRequirements(this)); } }

        public BeastTribe BeastTribe { get { return As<BeastTribe>(); } }

        public ENpc IssuingENpc { get { return Sheet.Collection.ENpcs[As<ENpcResident>("ENpcResident{Start}").Key]; } }
        public ENpc TargetENpc { get { return Sheet.Collection.ENpcs[As<ENpcResident>("ENpcResident{End}").Key]; } }

        public bool IsRepeatable { get { return AsBoolean("IsRepeatable"); } }
        public QuestRepeatInterval RepeatInterval { get { return (QuestRepeatInterval)AsInt32("RepeatInterval"); } }

        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        public JournalGenre JournalGenre { get { return As<JournalGenre>(); } }

        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public Imaging.ImageFile SpecialIcon { get { return AsImage("Icon{Special}"); } }

        public int SortKey { get { return AsInt32("SortKey"); } }

        public QuestRewards Rewards { get { return _Rewards ?? (_Rewards = new QuestRewards(this)); } }
        #endregion

        #region Constructors

        public Quest(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }

        #region IItemSource Members

        System.Collections.Generic.IEnumerable<Item> IItemSource.Items {
            get {
                return Rewards.Items.SelectMany(g => g.Items.Select(i => i.Item));
            }
        }

        #endregion
    }
}
