using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Quest : XivRow {
        #region Fields
        private QuestRequirements _Requirements;
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
        #endregion

        #region Constructors

        public Quest(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
