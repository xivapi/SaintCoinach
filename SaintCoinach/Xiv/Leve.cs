using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a Levequest.
    /// </summary>
    public class Leve : XivRow, ILocatable, IItemSource {
        #region Properties

        /// <summary>
        ///     Gets the name of the current leve.
        /// </summary>
        /// <value>The name of the current leve.</value>
        public Text.XivString Name { get { return AsString("Name"); } }

        /// <summary>
        ///     Gets the name of the current leve.
        /// </summary>
        /// <value>The name of the current leve.</value>
        public Text.XivString Description { get { return AsString("Description"); } }

        /// <summary>
        ///     Gets the <see cref="LeveClient" /> of the current leve.
        /// </summary>
        /// <value>The <see cref="LeveClient" /> of the current leve.</value>
        public LeveClient LeveClient { get { return As<LeveClient>(); } }

        /// <summary>
        ///     Gets the <see cref="LeveAssignmentType" /> of the current leve.
        /// </summary>
        /// <value>The <see cref="LeveAssignmentType" /> of the current leve.</value>
        public LeveAssignmentType LeveAssignmentType { get { return As<LeveAssignmentType>(); } }

        /// <summary>
        ///     Gets the <see cref="ClassJobCategory" /> for the current leve.
        /// </summary>
        /// <value>The <see cref="ClassJobCategory" /> for the current leve.</value>
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }

        /// <summary>
        ///     Gets the level of the current leve.
        /// </summary>
        /// <value>The level of the current leve.</value>
        public int CharacterLevel { get { return AsInt32("CLevel"); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> the current leve takes place at.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> the current leve takes place at.</value>
        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        /// <summary>
        ///     Gets the <see cref="PlaceName" /> of the zone the current leve takes place in.
        /// </summary>
        /// <value>The <see cref="PlaceName" /> of the zone the current leve takes place in.</value>
        public PlaceName ZonePlaceName { get { return As<PlaceName>("PlaceName{Zone}"); } }

        /// <summary>
        ///     Gets the icon of the city state that issued the current leve.
        /// </summary>
        /// <value>The icon of the city state that issued the current leve.</value>
        public ImageFile CityStateIcon { get { return AsImage("Icon{CityState}"); } }

        /// <summary>
        ///     Gets the integer key of the object containing additional information.
        /// </summary>
        /// <value>The integer key of the object containing additional information.</value>
        public int DataId { get { return AsInt32("DataId"); } }

        /// <summary>
        ///     Gets the <see cref="LeveRewardItem" /> the current leve offers.
        /// </summary>
        /// <value>The <see cref="LeveRewardItem" /> the current leve offers.</value>
        public LeveRewardItem LeveRewardItem { get { return As<LeveRewardItem>(); } }

        /// <summary>
        ///     Gets the experience reward of the current leve.
        /// </summary>
        /// <value>The experience reward of the current leve.</value>
        public int ExpReward { get { return AsInt32("ExpReward"); } }

        /// <summary>
        ///     Gets the gil reward of the current leve.
        /// </summary>
        /// <value>The gil reward of the current leve.</value>
        public int GilReward { get { return AsInt32("GilReward"); } }

        /// <summary>
        ///     Gets the <see cref="Level" /> object for the location of the current leve.
        /// </summary>
        /// <value>The <see cref="Level" /> object for the location of the current leve.</value>
        public Level Level { get { return As<Level>(); } }

        /// <summary>
        ///     Gets the icon for the location of the current leve.
        /// </summary>
        /// <value>The icon for the location of the current leve.</value>
        public ImageFile IssuerIcon { get { return AsImage("Icon{Issuer}"); } }

        /// <summary>
        /// Gets the locations of the current object.
        /// </summary>
        /// <value>The locations of the current object.</value>
        public IEnumerable<ILocation> Locations { get { yield return this.Level; } }

        /// <summary>
        /// Gets the <see cref="LeveVfx"/> used for the current leve.
        /// </summary>
        /// <value>The <see cref="LeveVfx"/> used for the current leve.</value>
        public LeveVfx LeveVfx { get { return As<LeveVfx>(); } }

        /// <summary>
        /// Gets the plate icon used for the current leve.
        /// </summary>
        /// <value>The plate icon used for the current leve.</value>
        public Imaging.ImageFile PlateIcon { get { return LeveVfx.Icon; } }

        /// <summary>
        /// Gets the <see cref="LeveVfx"/> used for the current leve.
        /// </summary>
        /// <value>The <see cref="LeveVfx"/> used for the current leve.</value>
        public LeveVfx FrameLeveVfx { get { return As<LeveVfx>("LeveVfx{Frame}"); } }

        /// <summary>
        /// Gets the plate icon used for the current leve.
        /// </summary>
        /// <value>The plate icon used for the current leve.</value>
        public Imaging.ImageFile FrameIcon { get { return FrameLeveVfx.Icon; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Leve" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Leve(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Returns a string representation of the current leve.
        /// </summary>
        /// <returns>The value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }

        #region IItemSource Members

        private Item[] _ItemSourceItems;
        public IEnumerable<Item> Items {
            get { return _ItemSourceItems ?? (_ItemSourceItems = LeveRewardItem.ItemGroups.SelectMany(g => g.Value.Items.Select(v => v.Item)).ToArray()); }
        }

        #endregion
    }
}
