using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class MonsterNote : XivRow {
        #region Fields

        private Target[] _Targets;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public int Reward { get { return AsInt32("Reward"); } }
        public IEnumerable<Target> Targets { get { return _Targets ?? (_Targets = BuildTargets()); } }

        #endregion

        #region Constructors

        #region Constructor

        public MonsterNote(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private Target[] BuildTargets() {
            const int Count = 4;

            var targets = new List<Target>();
            for (var i = 0; i < Count; ++i) {
                var target = As<MonsterNoteTarget>("MonsterNoteTarget", i);
                var count = AsInt32("Count", i);
                if (target.Key != 0 && count != 0)
                    targets.Add(new Target(target, count));
            }

            return targets.ToArray();
        }

        #endregion

        public override string ToString() {
            return Name;
        }

        #region Helper class

        public class Target {
            #region Properties

            public MonsterNoteTarget MonsterNoteTarget { get; private set; }
            public int Count { get; private set; }

            #endregion

            #region Constructors

            #region Constructor

            public Target(MonsterNoteTarget monsterNoteTarget, int count) {
                MonsterNoteTarget = monsterNoteTarget;
                Count = count;
            }

            #endregion

            #endregion
        }

        #endregion
    }
}
