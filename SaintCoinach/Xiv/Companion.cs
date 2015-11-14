using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;
using System;

namespace SaintCoinach.Xiv {
    public class Companion : XivRow, IQuantifiableXivString {
        #region Fields
        private CompanionTransient _CompanionTransient;
        #endregion

        #region Properties

        public Text.XivString Singular { get { return AsString("Singular"); } }
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }
        public Text.XivString Description { get { return CompanionTransient.Description; } }
        public Text.XivString GuideDescription { get { return CompanionTransient.GuideDescription; } }
        public Text.XivString Tooltip { get { return CompanionTransient.Tooltip; } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public byte Cost {  get { return As<byte>("Cost"); } }
        public UInt16 HP { get { return As<UInt16>("HP"); } }
        public byte Attack {  get { return CompanionTransient.Attack; } }
        public byte Defense { get { return CompanionTransient.Defense; } }
        public byte Speed {  get { return CompanionTransient.Speed; } }
        public bool HasAreaAttack {  get { return CompanionTransient.HasAreaAttack; } }
        public bool StrongVsGate { get { return CompanionTransient.StrongVsGate; } }
        public bool StrongVsEye { get { return CompanionTransient.StrongVsEye; } }
        public bool StrongVsShield { get { return CompanionTransient.StrongVsShield; } }
        public bool StrongVsArcana { get { return CompanionTransient.StrongVsArcana; } }
        public MinionSkillType SkillType { get { return CompanionTransient.SkillType; } }
        public byte SpecialActionCost { get { return As<byte>("Skill{Cost}"); } }
        public UInt16 SpecialActionAngle {  get { return As<UInt16>("Skill{Angle}"); } }
        public MinionRace Race {  get { return As<MinionRace>(); } }

        public CompanionTransient CompanionTransient { get { return _CompanionTransient ?? (_CompanionTransient = BuildTransient()); } }

        #endregion

        #region Constructors

        #region Constructor

        public Companion(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Singular;
        }

        #region IQuantifiableName Members
        string IQuantifiable.Singular {
            get { return Singular; }
        }

        string IQuantifiable.Plural {
            get { return Plural; }
        }
        #endregion

        private CompanionTransient BuildTransient()
        {
            return this.Sheet.Collection.GetSheet<CompanionTransient>()[Key];
        }
    }
}
