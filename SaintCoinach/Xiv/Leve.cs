using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Leve : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public LeveClient LeveClient { get { return As<LeveClient>(); } }
        public LeveAssignmentType LeveAssignmentType { get { return As<LeveAssignmentType>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int CharacterLevel { get { return AsInt32("CLevel"); } }
        public PlaceName PlaceName { get { return As<PlaceName>(); } }
        public PlaceName ZonePlaceName { get { return As<PlaceName>("PlaceName{Zone}"); } }
        public Imaging.ImageFile CityStateIcon { get { return AsImage("Icon{CityState}"); } }
        public int DataId { get { return AsInt32("DataId"); } }
        public LeveRewardItem LeveRewardItem { get { return As<LeveRewardItem>(); } }
        public int ExpReward { get { return AsInt32("ExpReward"); } }
        public Level Level { get { return As<Level>(); } }
        public Imaging.ImageFile IssuerIcon { get { return AsImage("Icon{Issuer}"); } }
        #endregion

        #region Constructor
        public Leve(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}