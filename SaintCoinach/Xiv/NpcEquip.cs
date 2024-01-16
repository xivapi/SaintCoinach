using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class NpcEquip : XivRow {
        #region Properties
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

        #endregion
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NpcEquip" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public NpcEquip(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
