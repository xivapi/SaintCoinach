using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv
{
    public class CompanionTransient : XivRow 
    {
        #region Constructors

        #region Constructor

        public CompanionTransient(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Properties

        public Text.XivString Description { get { return AsString("Description"); } }
        public Text.XivString GuideDescription { get { return AsString("Description{Enhanced}"); } }
        public Text.XivString Tooltip { get { return AsString("Tooltip"); } }
        public byte Attack { get { return As<byte>("Attack"); } }
        public byte Defense { get { return As<byte>("Defense"); } }
        public byte Speed { get { return As<byte>("Speed"); } }
        public bool HasAreaAttack { get { return AsBoolean("HasAreaAttack"); } }
        public bool StrongVsGate {  get { return AsBoolean("Strength{Gate}"); } }
        public bool StrongVsEye { get { return AsBoolean("Strength{Eye}"); } }
        public bool StrongVsShield { get { return AsBoolean("Strength{Shield}"); } }
        public bool StrongVsArcana { get { return AsBoolean("Strength{Arcana}"); } }
        public MinionSkillType SkillType { get { return As<MinionSkillType>(); } }
        #endregion
    }
}
