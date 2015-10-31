using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Graphics;

namespace SaintCoinach.Xiv.Items {
    /// <summary>
    ///     Base class for equipment items.
    /// </summary>
    public abstract class Equipment : Item, IParameterObject {
        #region Fields

        /// <summary>
        ///     <see cref="ParameterCollection" /> containing all parameters, primary and secondary, of the current item.
        /// </summary>
        private ParameterCollection _AllParameters;

        /// <summary>
        ///     <see cref="ParameterCollection" /> containg all secondary parameters of the current item.
        /// </summary>
        private ParameterCollection _SecondaryParameters;

        private Race[] _RacesEquippableBy;

        #endregion

        #region Properties

        public IEnumerable<Race> RacesEquippableBy {
            get { return _RacesEquippableBy ?? (_RacesEquippableBy = BuildRacesEquippableBy()); }
        }

        public bool EquippableByMale {
            get { return AsBoolean("EquippableByGender{M}"); }
        }
        public bool EquippableByFemale {
            get { return AsBoolean("EquippableByGender{F}"); }
        }

        public bool IsCrestWorthy {
            get { return AsBoolean("IsCrestWorthy"); }
        }

        /// <summary>
        ///     Gets the primary <see cref="Parameter" />s of the current item.
        /// </summary>
        /// <value>The primary <see cref="Parameter" />s of the current item.</value>
        public abstract IEnumerable<Parameter> PrimaryParameters { get; }

        /// <summary>
        ///     Gets the secondary <see cref="Parameter" />s of the current item.
        /// </summary>
        /// <value>The secondary <see cref="Parameter" />s of the current item.</value>
        public IEnumerable<Parameter> SecondaryParameters {
            get { return _SecondaryParameters ?? (_SecondaryParameters = BuildSecondaryParameters()); }
        }

        /// <summary>
        ///     Gets all <see cref="Parameter" />s of the current item.
        /// </summary>
        /// <value>The all <see cref="Parameter" />s of the current item.</value>
        public IEnumerable<Parameter> AllParameters {
            get {
                if (_AllParameters != null) return _AllParameters;

                _AllParameters = new ParameterCollection();
                _AllParameters.AddRange(PrimaryParameters);
                _AllParameters.AddRange(SecondaryParameters);
                return _AllParameters;
            }
        }

        /// <summary>
        ///     Gets the level required to equip the current item.
        /// </summary>
        /// <value>The level required to equip the current item.</value>
        public int EquipmentLevel { get { return AsInt32("Level{Equip}"); } }

        /// <summary>
        ///     Gets the modifier used for <see cref="BaseParam" />s on the current item.
        /// </summary>
        /// <value>The modifier used for <see cref="BaseParam" />s on the current item.</value>
        public int BaseParamModifier { get { return AsInt32("BaseParamModifier"); } }

        /// <summary>
        ///     Gets the <see cref="EquipSlotCategory" /> of the current item.
        /// </summary>
        /// <value>The <see cref="EquipSlotCategory" /> of the current item.</value>
        public EquipSlotCategory EquipSlotCategory { get { return As<EquipSlotCategory>(); } }

        /// <summary>
        ///     Gets the number of materia than can be fitted into the current item without overmelding.
        /// </summary>
        /// <value>The number of materia than can be fitted into the current item without overmelding.</value>
        public int FreeMateriaSlots { get { return AsInt32("MateriaSlotCount"); } }

        /// <summary>
        ///     Gets the <see cref="ClassJob" /> required to repair the current item.
        /// </summary>
        /// <value>The <see cref="ClassJob" /> required to repair the current item.</value>
        public ClassJob RepairClassJob { get { return As<ClassJob>("ClassJob{Repair}"); } }

        /// <summary>
        ///     Gets the <see cref="Item" /> required to repair the current item.
        /// </summary>
        /// <value>The <see cref="Item" /> required to repair the current item.</value>
        public Item RepairItem { get { return As<Item>("Item{Repair}"); } }

        /// <summary>
        ///     Gets the type of <see cref="ItemSpecialBonus" /> required to grant additional bonuses of the current item.
        /// </summary>
        /// <value>The type of <see cref="ItemSpecialBonus" /> required to grant additional bonuses of the current item.</value>
        public ItemSpecialBonus ItemSpecialBonus { get { return As<ItemSpecialBonus>(); } }

        /// <summary>
        ///     Gets the <see cref="ItemSeries" /> of the current item.
        /// </summary>
        /// <value>The <see cref="ItemSeries" /> of the current item.</value>
        public ItemSeries ItemSeries { get { return As<ItemSeries>(); } }

        /// <summary>
        ///     Gets the <see cref="ClassJobCategory" /> required to equip the current item.
        /// </summary>
        /// <value>The <see cref="ClassJobCategory" /> required to equip the current item.</value>
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }

        /// <summary>
        ///     Gets the PvP-rank required to equip the current item.
        /// </summary>
        /// <value>The PvP-rank required to equip the current item.</value>
        public int RequiredPvPRank { get { return AsInt32("PvPRank"); } }

        /// <summary>
        ///     Gets the model identifier used for the current item's primary model.
        /// </summary>
        /// <value>The model identifier used for the current item's primary model.</value>
        public Quad PrimaryModelKey { get { return AsQuad("Model{Main}"); } }

        /// <summary>
        ///     Gets the model identifier used for the current item's secondary model.
        /// </summary>
        /// <value>The model identifier used for the current item's secondary model.</value>
        public Quad SecondaryModelKey { get { return AsQuad("Model{Sub}"); } }
        
        /// <summary>
        ///     Gets the number of Grand Company seals rewarded for expert delivery of the item.
        /// </summary>
        /// <value>The number of Grand Company seals.</value>
        public int ExpertDeliverySeals {
            get {
                if (Rarity > 1 && As<byte>("GCTurnIn") > 0)
                    return (int)Math.Round(5.75 * ItemLevel.Key, MidpointRounding.AwayFromZero);
                else
                    return 0;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Equipment" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        protected Equipment(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        /// <summary>
        ///     Gets all <see cref="Parameter" />s of the current item.
        /// </summary>
        /// <value>The all <see cref="Parameter" />s of the current item.</value>
        /// <seealso cref="AllParameters" />
        IEnumerable<Parameter> IParameterObject.Parameters { get { return AllParameters; } }

        /// <summary>
        ///     Get the model for the current item and a specific character type.
        /// </summary>
        /// <param name="characterType">Character type to get the model for.</param>
        /// <param name="materialVersion">When this method returns contains the variant of the model for the current item.</param>
        /// <returns>The model for the current item and <c>characterType</c>.</returns>
        public ModelDefinition GetModel(int characterType, out Graphics.ImcVariant variant) {
            variant = Graphics.ImcVariant.Default;
            var slot = EquipSlotCategory.PossibleSlots.FirstOrDefault();
            return slot == null ? null : GetModel(slot, characterType, out variant);
        }

        /// <summary>
        ///     Get the model for the current item and a specific character type and in a speific <see cref="EquipSlot" />.
        /// </summary>
        /// <param name="equipSlot"><see cref="EquipSlot" /> for which to get the model.</param>
        /// <param name="characterType">Character type to get the model for.</param>
        /// <param name="materialVersion">When this method returns contains the variant of the model for the current item.</param>
        /// <returns>The model for the current item and <c>characterType</c> in <c>equipSlot</c>.</returns>
        public ModelDefinition GetModel(EquipSlot equipSlot, int characterType, out Graphics.ImcVariant variant) {
            return equipSlot.GetModel(PrimaryModelKey, characterType, out variant);
        }

        #region Helpers

        /// <summary>
        ///     Get the maximum amount of <see cref="BaseParam" /> that can be melded to the current item.
        /// </summary>
        /// <param name="baseParam"><see cref="BaseParam" /> for which to get the amount.</param>
        /// <param name="onHq">A value indicating whether bonuses for a hiqh-quality item should be taken into account.</param>
        /// <returns>The maximum amount of <c>baseParam</c> that can be melded to the current item.</returns>
        public int GetMateriaMeldCap(BaseParam baseParam, bool onHq) {
            var max = GetMaximumParamValue(baseParam);
            var present = GetParameterValue(baseParam, onHq);

            return Math.Max(0, max - present);
        }

        /// <summary>
        ///     Get the amount of <see cref="BaseParam" /> present on the current item.
        /// </summary>
        /// <param name="param"><see cref="BaseParam" /> for which to get the amount.</param>
        /// <param name="includeNonBase">
        ///     A value indicating whether to include bonuses that are not primary or the item's base
        ///     parameter.
        /// </param>
        /// <returns>Returns the amount of <see cref="BaseParam" /> present on the current item.</returns>
        public int GetParameterValue(BaseParam baseParam, bool includeNonBase) {
            var present = AllParameters.FirstOrDefault(_ => _.BaseParam == baseParam);
            // ReSharper disable InvertIf
            if (present == null) return 0;

            if (includeNonBase)
                return (int)present.Cast<ParameterValueFixed>().Sum(p => p.Amount);

            return
                (int)
                present.Where(_ => _.Type == ParameterType.Base || _.Type == ParameterType.Primary)
                       .Cast<ParameterValueFixed>()
                       .Sum(p => p.Amount);
        }

        /// <summary>
        ///     Get the maximum amount of a <see cref="BaseParam" /> possible for the current item.
        /// </summary>
        /// <param name="baseParam"><see cref="BaseParam" /> for which to get the amount.</param>
        /// <returns>The maximum amount of <c>baseParam</c> on the current item.</returns>
        public int GetMaximumParamValue(BaseParam baseParam) {
            // Base value for the param based on the item's level
            var maxBase = ItemLevel.GetMaximum(baseParam);
            // Factor, in percent, for the param when applied to the item's equip slot
            var slotFactor = baseParam.GetMaximum(EquipSlotCategory);
            // Factor, in percent, for the param when used for the item's role
            var roleModifier = baseParam.GetModifier(BaseParamModifier);

            // TODO: Not confirmed to use Round, could be Ceiling or Floor; or applied at different points
            // Rounding appears to use AwayFromZero.  Tested with:
            // Velveteen Work Gloves (#3601) for gathering (34.5 -> 35)
            // Gryphonskin Ring (#4526) for wind resistance (4.5 -> 5)
            // Fingerless Goatskin Gloves of Gathering (#3578) for GP (2.5 -> 3)
            return (int)Math.Round(maxBase * slotFactor * roleModifier / 10000.0, MidpointRounding.AwayFromZero);
        }

        #endregion

        #region Build

        /// <summary>
        ///     Build a <see cref="ParameterCollection" /> for secondary parameters.
        /// </summary>
        /// <returns>A <see cref="ParameterCollection" /> for secondary parameters.</returns>
        protected virtual ParameterCollection BuildSecondaryParameters() {
            var parameters = new ParameterCollection();

            AddDefaultParameters(parameters);
            AddSpecialParameters(parameters);

            return parameters;
        }

        private Race[] BuildRacesEquippableBy() {
            var allRaces = Sheet.Collection.GetSheet<Race>();
            var races = new List<Race>();
            foreach (var race in allRaces.Where(r => r.Key != 0)) {
                if (AsBoolean("EquippableByRace", race.Key))
                    races.Add(race);
            }

            return races.ToArray();
        }

        /// <summary>
        ///     Add the default (base) parameters to a <see cref="ParameterCollection" />.
        /// </summary>
        /// <param name="parameters"><see cref="ParameterCollection" /> to which to add the parameters.</param>
        private void AddDefaultParameters(ParameterCollection parameters) {
            const int Count = 6;

            for (var i = 0; i < Count; ++i) {
                var baseParam = As<BaseParam>("BaseParam", i);
                var value = AsInt32("BaseParamValue", i);

                AddParameter(parameters, ParameterType.Base, baseParam, value);
            }
        }

        /// <summary>
        ///     Add the special parameters to a <see cref="ParameterCollection" />.
        /// </summary>
        /// <param name="parameters"><see cref="ParameterCollection" /> to which to add the parameters.</param>
        private void AddSpecialParameters(ParameterCollection parameters) {
            const int Count = 6;

            ParameterType type;
            switch (ItemSpecialBonus.Key) {
                case 2:
                    type = ParameterType.SetBonus;
                    break;
                case 4:
                    type = ParameterType.Sanction;
                    break;
                default:
                    type = ParameterType.Hq;
                    break;
            }

            for (var i = 0; i < Count; ++i) {
                var baseParam = As<BaseParam>("BaseParam{Special}", i);
                var value = AsInt32("BaseParamValue{Special}", i);

                AddParameter(parameters, type, baseParam, value);
            }
        }

        /// <summary>
        ///     Attempt to add a parameter to a <see cref="ParameterCollection" />.
        /// </summary>
        /// <param name="parameters"><see cref="ParameterCollection" /> to which to add the parameters.</param>
        /// <param name="type"><see cref="ParameterType" /> of the parameter to be added.</param>
        /// <param name="baseParam"><see cref="BaseParam" /> for which a parameter should be added.</param>
        /// <param name="value">Value of the parameter to be added.</param>
        private static void AddParameter(ParameterCollection parameters,
                                         ParameterType type,
                                         BaseParam baseParam,
                                         int value) {
            if (baseParam.Key == 0)
                return;

            parameters.AddParameterValue(baseParam, new ParameterValueFixed(type, value));
        }

        #endregion
    }
}
