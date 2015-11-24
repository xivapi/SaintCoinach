using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv
{
    public class QuestRewardOther : XivRow
    {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        #region Constructor

        public QuestRewardOther(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
