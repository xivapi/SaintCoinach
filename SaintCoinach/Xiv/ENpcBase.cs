using System;
using System.Collections.Generic;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ENpcBase : XivRow {
        #region Static

        public const int DataCount = 32;

        #endregion

        #region Fields

        private IRelationalRow[] _AssignedData;

        #endregion

        #region Properties
        public ModelChara ModelChara { get { return As<ModelChara>("ModelChara"); } }
        public Race Race { get { return As<Race>("Race"); } }
        public int Gender { get { return AsInt32("Gender"); } }
        public int BodyType { get { return AsInt32("BodyType"); } }
        public int Height { get { return AsInt32("Height"); } }
        public Tribe Tribe { get { return As<Tribe>("Tribe"); } }
        public int Face { get { return AsInt32("Face"); } }
        public int HairStyle { get { return AsInt32("HairStyle"); } }
        public int HairHighlight { get { return AsInt32("HairHighlight"); } }
        public int SkinColor { get { return AsInt32("SkinColor"); } }
        public int EyeHeterochromia { get { return AsInt32("EyeHeterochromia"); } }
        public int HairColor { get { return AsInt32("HairColor"); } }
        public int HairHighlightColor { get { return AsInt32("HairHighlightColor"); } }
        public int FacialFeature { get { return AsInt32("FacialFeature"); } }
        public int FacialFeatureColor { get { return AsInt32("FacialFeatureColor"); } }
        public int Eyebrows { get { return AsInt32("Eyebrows"); } }
        public int EyeColor { get { return AsInt32("EyeColor"); } }
        public int EyeShape { get { return AsInt32("EyeShape"); } }
        public int Nose { get { return AsInt32("Nose"); } }
        public int Jaw { get { return AsInt32("Jaw"); } }
        public int Mouth { get { return AsInt32("Mouth"); } }
        public int LipColor { get { return AsInt32("LipColor"); } }
        public int BustOrTone1 { get { return AsInt32("BustOrTone1"); } }

        public int ExtraFeature1 { get { return AsInt32("ExtraFeature1"); } }
        public int ExtraFeature2OrBust { get { return AsInt32("ExtraFeature2OrBust"); } }
        public int FacePaint { get { return AsInt32("FacePaint"); } }
        public int FacePaintColor { get { return AsInt32("FacePaintColor"); } }

        public Quad ModelMain { get { return AsQuad("ModelMainHand"); } }
        public Stain DyeMain { get { return As<Stain>("DyeMainHand"); } }
        public Quad ModelSub { get { return AsQuad("ModelOffHand"); } }
        public Stain DyeOff { get { return As<Stain>("DyeOffHand"); } }
        public int[] ModelHead { get { return AsIntArray("ModelHead"); } }
        public int[] ModelBody { get { return AsIntArray("ModelBody"); } }
        public int[] ModelHands { get { return AsIntArray("ModelHands"); } }
        public int[] ModelLegs { get { return AsIntArray("ModelLegs"); } }
        public int[] ModelFeet { get { return AsIntArray("ModelFeet"); } }
        public int[] ModelEars { get { return AsIntArray("ModelEars"); } }
        public int[] ModelNeck { get { return AsIntArray("ModelNeck"); } }
        public int[] ModelWrists { get { return AsIntArray("ModelWrists"); } }
        public int[] ModelLeftRing { get { return AsIntArray("ModelLeftRing"); } }
        public int[] ModelRightRing { get { return AsIntArray("ModelRightRing"); } }
        public Stain DyeHead { get { return As<Stain>("DyeHead"); } }
        public Stain DyeBody { get { return As<Stain>("DyeBody"); } }
        public Stain DyeHands { get { return As<Stain>("DyeHands"); } }
        public Stain DyeLegs { get { return As<Stain>("DyeLegs"); } }
        public Stain DyeFeet { get { return As<Stain>("DyeFeet"); } }
        public Stain DyeEars { get { return As<Stain>("DyeEars"); } }
        public Stain DyeNeck { get { return As<Stain>("DyeNeck"); } }
        public Stain DyeWrists { get { return As<Stain>("DyeWrists"); } }
        public Stain DyeLeftRing { get { return As<Stain>("DyeLeftRing"); } }
        public Stain DyeRightRing { get { return As<Stain>("DyeRightRing"); } }
        public NpcEquip NpcEquip { get { return As<NpcEquip>(); } }

        public IEnumerable<IRelationalRow> AssignedData { get { return _AssignedData ?? (_AssignedData = BuildAssignedData()); } }

        #endregion

        #region Constructors

        public ENpcBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        
        #endregion

        public IRelationalRow GetData(int index) {
            return As<IRelationalRow>("ENpcData", index);
        }
        public int GetRawData(int index) {
            var fulCol = BuildColumnName("ENpcData", index);
            var raw = ((IRelationalRow)this).GetRaw(fulCol);
            return Convert.ToInt32(raw);
        }

        private IRelationalRow[] BuildAssignedData() {
            var data = new List<IRelationalRow>();

            for (var i = 0; i < ENpcBase.DataCount; ++i) {
                var val = GetData(i);
                if (val != null)
                    data.Add(val);
            }

            return data.ToArray();
        }
    }
}
