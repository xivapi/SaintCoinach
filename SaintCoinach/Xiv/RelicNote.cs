using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RelicNote : XivRow {
        #region Fields

        private FateTarget[] _Fates;
        private Leve[] _Leves;
        private MonsterNoteTarget[] _NotoriousTarget;
        private Target[] _Targets;

        #endregion

        #region Properties

        public EventItem EventItem { get { return As<EventItem>(); } }
        public IEnumerable<Target> Targets { get { return _Targets ?? (_Targets = BuildTargets()); } }

        public IEnumerable<MonsterNoteTarget> NotoriousTargets {
            get { return _NotoriousTarget ?? (_NotoriousTarget = BuildNotoriousTargets()); }
        }

        public IEnumerable<FateTarget> Fates { get { return _Fates ?? (_Fates = BuildFates()); } }
        public IEnumerable<Leve> Leves { get { return _Leves ?? (_Leves = BuildLeves()); } }

        #endregion

        #region Constructors

        #region Constructor

        public RelicNote(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Target

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

        #region Fate

        public class FateTarget {
            #region Properties

            public Fate Fate { get; private set; }
            public PlaceName PlaceName { get; private set; }

            #endregion

            #region Constructors

            #region Constructor

            public FateTarget(Fate fate, PlaceName placeName) {
                Fate = fate;
                PlaceName = placeName;
            }

            #endregion

            #endregion
        }

        #endregion

        #region Build

        private Target[] BuildTargets() {
            const int Count = 10;

            var targets = new Target[Count];
            for (var i = 0; i < targets.Length; ++i) {
                var monster = As<MonsterNoteTarget>("MonsterNoteTarget{Common}", i);
                var count = AsInt32("MonsterCount", i);

                targets[i] = new Target(monster, count);
            }

            return targets;
        }

        private MonsterNoteTarget[] BuildNotoriousTargets() {
            const int Count = 3;

            var targets = new MonsterNoteTarget[Count];
            for (var i = 0; i < targets.Length; ++i)
                targets[i] = As<MonsterNoteTarget>("MonsterNoteTarget{NM}", i);

            return targets;
        }

        private FateTarget[] BuildFates() {
            const int Count = 3;

            var fates = new FateTarget[Count];
            for (var i = 0; i < fates.Length; ++i) {
                var fate = As<Fate>("Fate", i);
                var place = As<PlaceName>("PlaceName{Fate}", i);

                fates[i] = new FateTarget(fate, place);
            }

            return fates;
        }

        private Leve[] BuildLeves() {
            const int Count = 3;

            var leves = new Leve[Count];
            for (var i = 0; i < leves.Length; ++i)
                leves[i] = As<Leve>("Leve", i);
            return leves;
        }

        #endregion
    }
}
