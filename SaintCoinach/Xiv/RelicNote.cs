using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RelicNote : XivRow {
        #region Target
        public class Target {
            #region Fields
            private MonsterNoteTarget _MonsterNoteTarget;
            private int _Count;
            #endregion

            #region Properties
            public MonsterNoteTarget MonsterNoteTarget { get { return _MonsterNoteTarget; } }
            public int Count { get { return _Count; } }
            #endregion

            #region Constructor
            public Target(MonsterNoteTarget monsterNoteTarget, int count) {
                _MonsterNoteTarget = monsterNoteTarget;
                _Count = count;
            }
            #endregion
        }
        #endregion

        #region Fate
        public class FateTarget {
            #region Fields
            private Fate _Fate;
            private PlaceName _PlaceName;
            #endregion

            #region Properties
            public Fate Fate { get { return _Fate; } }
            public PlaceName PlaceName { get { return _PlaceName; } }
            #endregion

            #region Constructor
            public FateTarget(Fate fate, PlaceName placeName) {
                _Fate = fate;
                _PlaceName = placeName;
            }
            #endregion
        }
        #endregion

        #region Fields
        private Target[] _Targets;
        private MonsterNoteTarget[] _NotoriousTarget;
        private FateTarget[] _Fates;
        private Leve[] _Leves;
        #endregion

        #region Properties
        public EventItem EventItem { get { return As<EventItem>(); } }
        public IEnumerable<Target> Targets { get { return _Targets ?? (_Targets = BuildTargets()); } }
        public IEnumerable<MonsterNoteTarget> NotoriousTargets { get { return _NotoriousTarget ?? (_NotoriousTarget = BuildNotoriousTargets()); } }
        public IEnumerable<FateTarget> Fates { get { return _Fates ?? (_Fates = BuildFates()); } }
        public IEnumerable<Leve> Leves { get { return _Leves ?? (_Leves = BuildLeves()); } }
        #endregion

        #region Constructor
        public RelicNote(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
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