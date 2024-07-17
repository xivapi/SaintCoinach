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

        #endregion

        #region Properties

        public byte EquipRestriction {
            get { return As<byte>("EquipRestriction"); }
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
        ///     Gets the number of materia than can be fitted into the current item without overmelding.
        /// </summary>
        /// <value>The number of materia than can be fitted into the current item without overmelding.</value>
        public int FreeMateriaSlots { get { return AsInt32("MateriaSlotCount"); } }

        /// <summary>
        ///     Gets the <see cref="Item" /> required to repair the current item.
        /// </summary>
        /// <value>The <see cref="Item" /> required to repair the current item.</value>
        public Item RepairItem { get { return As<XivRow>("Item{Repair}").As<Item>("Item"); } }

        /// <summary>
        ///     Gets the type of <see cref="ItemSpecialBonus" /> required to grant additional bonuses of the current item.
        /// </summary>
        /// <value>The type of <see cref="ItemSpecialBonus" /> required to grant additional bonuses of the current item.</value>
        public ItemSpecialBonus ItemSpecialBonus { get { return As<ItemSpecialBonus>(); } }

        /// <summary>
        ///     Gets the parameter used for some <see cref="ItemSpecialBonus"/> required to grant additional bonuses of the current item. 
        /// </summary>
        /// <value>The parameter used for some <see cref="ItemSpecialBonus"/> required to grant additional bonuses of the current item. </value>
        public byte ItemSpecialBonusParam {  get { return As<byte>("ItemSpecialBonus{Param}"); } }

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
        ///     Gets the PvP indicator of the item.
        /// </summary>
        /// <value>True if the item is PvP equipment, false otherwise.</value>
        public bool IsPvP { get { return AsBoolean("IsPvP"); } }

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
                if (Rarity <= 1 || AsInt32("Price{Low}") <= 0)
                    return 0;

                // Formula used for GCSupplyDutyReward seals:
                // For every item level 200 and below, you receive 5.75 seals.
                // For every item level after 200, you receive 2 seals.

                var rewards = Sheet.Collection.GetSheet("GCSupplyDutyReward");
                if (rewards.ContainsRow(ItemLevel.Key))
                    return (int)(uint)rewards[ItemLevel.Key]["Seals{ExpertDelivery}"];
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
            return (int)Math.Round(maxBase * slotFactor * roleModifier / 100000.0, MidpointRounding.AwayFromZero);
        }

        public int GetModelCharacterType() {
            switch (EquipRestriction) {
                case 0: return 0; // Not equippable
                case 1: return 101; // Unrestricted, default to male hyur
                case 2: return 101; // Any male
                case 3: return 201; // Any female
                case 4: return 101; // Hyur male
                case 5: return 201; // Hyur female
                case 6: return 501; // Elezen male
                case 7: return 601; // Elezen female
                case 8: return 1101; // Lalafell male
                case 9: return 1201; // Lalafell female
                case 10: return 701; // Miqo'te male
                case 11: return 801; // Miqo'te female
                case 12: return 901; // Roegadyn male
                case 13: return 1001; // Roegadyn female
                case 14: return 1301; // Au Ra male
                case 15: return 1401; // Au Ra female
                case 16: return 1501; // Hrothgar male
                case 17: return 1801; // Viera female
                case 18: return 1701; // Viera male
                case 19: return 1601; // Hrothgar female
                default:
                    throw new NotImplementedException();
            }
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

        /// <summary>
        ///     Add the default (base) parameters to a <see cref="ParameterCollection" />.
        /// </summary>
        /// <param name="parameters"><see cref="ParameterCollection" /> to which to add the parameters.</param>
        private void AddDefaultParameters(ParameterCollection parameters) {
            const int Count = 6;

            for (var i = 0; i < Count; ++i) {
                var baseParam = As<BaseParam>("BaseParam", i);
                var value = AsInt32("BaseParamValue", i);

                AddParameter(parameters, ParameterType.Base, baseParam, value, i);
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
                case 6:
                    type = ParameterType.SetBonusCapped;
                    break;
                case 7:
                    type = ParameterType.EurekaEffect;
                    break;
                default:
                    type = ParameterType.Hq;
                    break;
            }

            for (var i = 0; i < Count; ++i) {
                var baseParam = As<BaseParam>("BaseParam{Special}", i);
                var value = AsInt32("BaseParamValue{Special}", i);

                AddParameter(parameters, type, baseParam, value, i);
            }
        }

        /// <summary>
        ///     Attempt to add a parameter to a <see cref="ParameterCollection" />.
        /// </summary>
        /// <param name="parameters"><see cref="ParameterCollection" /> to which to add the parameters.</param>
        /// <param name="type"><see cref="ParameterType" /> of the parameter to be added.</param>
        /// <param name="baseParam"><see cref="BaseParam" /> for which a parameter should be added.</param>
        /// <param name="value">Value of the parameter to be added.</param>
        /// <param name="index">Index of the parameter to be added.</param>
        private static void AddParameter(ParameterCollection parameters,
                                         ParameterType type,
                                         BaseParam baseParam,
                                         int value,
                                         int index) {
            if (baseParam.Key == 0)
                return;

            parameters.AddParameterValue(baseParam, new ParameterValueFixed(type, value, index));
        }

        #endregion
    }
}
