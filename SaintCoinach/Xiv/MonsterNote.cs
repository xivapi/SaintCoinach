using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class MonsterNote : XivRow {
        #region Helper class
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

        #region Fields
        private Target[] _Targets;
        #endregion

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public int Reward { get { return AsInt32("Reward"); } }
        public IEnumerable<Target> Targets { get { return _Targets ?? (_Targets = BuildTargets()); } }
        #endregion

        #region Constructor
        public MonsterNote(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
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
    }
}