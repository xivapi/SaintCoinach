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

        public Quad ModelMain { get { return AsQuad("Model{MainHand}"); } }
        public Stain DyeMain { get { return As<Stain>("Dye{MainHand}"); } }
        public Quad ModelSub { get { return AsQuad("Model{OffHand}"); } }
        public Stain DyeOff { get { return As<Stain>("Dye{OffHand}"); } }
        public int[] ModelHead { get { return AsIntArray("Model{Head}"); } }
        public int[] ModelBody { get { return AsIntArray("Model{Body}"); } }
        public int[] ModelHands { get { return AsIntArray("Model{Hands}"); } }
        public int[] ModelLegs { get { return AsIntArray("Model{Legs}"); } }
        public int[] ModelFeet { get { return AsIntArray("Model{Feet}"); } }
        public int[] ModelEars { get { return AsIntArray("Model{Ears}"); } }
        public int[] ModelNeck { get { return AsIntArray("Model{Neck}"); } }
        public int[] ModelWrists { get { return AsIntArray("Model{Wrists}"); } }
        public int[] ModelLeftRing { get { return AsIntArray("Model{LeftRing}"); } }
        public int[] ModelRightRing { get { return AsIntArray("Model{RightRing}"); } }
        public Stain DyeHead { get { return As<Stain>("Dye{Head}"); } }
        public Stain DyeBody { get { return As<Stain>("Dye{Body}"); } }
        public Stain DyeHands { get { return As<Stain>("Dye{Hands}"); } }
        public Stain DyeLegs { get { return As<Stain>("Dye{Legs}"); } }
        public Stain DyeFeet { get { return As<Stain>("Dye{Feet}"); } }
        public Stain DyeEars { get { return As<Stain>("Dye{Ears}"); } }
        public Stain DyeNeck { get { return As<Stain>("Dye{Neck}"); } }
        public Stain DyeWrists { get { return As<Stain>("Dye{Wrists}"); } }
        public Stain DyeLeftRing { get { return As<Stain>("Dye{LeftRing}"); } }
        public Stain DyeRightRing { get { return As<Stain>("Dye{RightRing}"); } }
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
