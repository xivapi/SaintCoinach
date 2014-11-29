using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public abstract class Equipment : InventoryItem {
        #region Fields
        private ParameterCollection _SecondaryParameters = null;
        private ParameterCollection _AllParameters = null;
        #endregion

        #region Properties
        public abstract IEnumerable<Parameter> PrimaryParameters { get; }
        public IEnumerable<Parameter> SecondaryParameters { get { return _SecondaryParameters ?? (_SecondaryParameters = BuildSecondaryParameters()); } }
        public IEnumerable<Parameter> AllParameters {
            get {
                if (_AllParameters == null) {
                    _AllParameters = new ParameterCollection();
                    _AllParameters.AddRange(PrimaryParameters);
                    _AllParameters.AddRange(SecondaryParameters);
                }
                return _AllParameters;
            }
        }
        public int EquipmentLevel { get { return AsInt32("Level{Equip}"); } }
        public EquipSlotCategory EquipSlotCategory { get { return As<EquipSlotCategory>(); } }
        public int FreeMateriaSlots { get { return AsInt32("MateriaSlotCount"); } }
        public long ModelIdentifierPrimary { get { return AsInt64("Model{Main}"); } }
        public long ModelIdentifierSecondary { get { return AsInt64("Model{Sub}"); } }
        public ClassJob RepairClassJob { get { return As<ClassJob>("ClassJob{Repair}"); } }
        public InventoryItem RepairItem { get { return As<InventoryItem>("Item{Repair}"); } }
        public ItemSpecialBonus ItemSpecialBonus { get { return As<ItemSpecialBonus>(); } }
        public ItemSeries ItemSeries { get { return As<ItemSeries>(); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int RequiredPvPRank { get { return AsInt32("PvPRank"); } }
        #endregion

        #region Constructor
        public Equipment(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        protected virtual ParameterCollection BuildSecondaryParameters() {
            var parameters = new ParameterCollection();

            AddDefaultParameters(parameters);
            AddSpecialParameters(parameters);

            return parameters;
        }
        private void AddDefaultParameters(ParameterCollection parameters) {
            const int Count = 6;

            for (int i = 0; i < Count; ++i) {
                var baseParam = As<BaseParam>("BaseParam", i);
                var value = AsInt32("BaseParamValue", i);

                AddParameter(parameters, ParameterType.Base, baseParam, value);
            }
        }
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
                    type = ParameterType.HQ;
                    break;
            }

            for (int i = 0; i < Count; ++i) {
                var baseParam = As<BaseParam>("BaseParam{Special}", i);
                var value = AsInt32("BaseParamValue{Special}", i);

                AddParameter(parameters, type, baseParam, value);
            }
        }
        private void AddParameter(ParameterCollection parameters, ParameterType type, BaseParam baseParam, int value) {
            if (baseParam.Key == 0)
                return;

            parameters.AddParameterValue(baseParam, new ParameterValueFixed(type, value));
        }
        #endregion
    }
}