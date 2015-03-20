using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public partial class QuestRequirements {
        #region Fields
        private readonly Quest _Quest;
        private ClassJobRequirement[] _ClassJobs;
        private Quest[] _QuestExclusion;
        private InstanceContentRequirement _InstanceContents;
        private PreviousQuestRequirement _PreviousQuests;
        #endregion

        #region Properties
        public Quest Quest { get { return _Quest; } }
        public IEnumerable<ClassJobRequirement> ClassJobs { get { return _ClassJobs ?? (_ClassJobs = BuildClassJobs()); } }
        public InstanceContentRequirement InstanceContent { get { return _InstanceContents ?? (_InstanceContents = new InstanceContentRequirement(_Quest)); } }
        public PreviousQuestRequirement PreviousQuest { get { return _PreviousQuests ?? (_PreviousQuests = new PreviousQuestRequirement(_Quest)); } }
        public int QuestLevelOffset { get { return Quest.AsInt32("QuestLevelOffset"); } }
        public IEnumerable<Quest> QuestExclusion { get { return _QuestExclusion ?? (_QuestExclusion = BuildQuestExclusion()); } }
        public GrandCompany GrandCompany { get { return Quest.As<GrandCompany>(); } }
        public GrandCompanyRank GrandCompanyRank { get { return Quest.As<GrandCompanyRank>(); } }

        public int StartBell { get { return Quest.AsInt32("Bell{Start}"); } }
        public int EndBell { get { return Quest.AsInt32("Bell{End}"); } }

        public BeastReputationRank BeastReputationRank { get { return Quest.As<BeastReputationRank>(); } }

        public Mount Mount { get { return Quest.As<Mount>("Mount{Required}"); } }

        public bool RequiresHousing { get { return Quest.AsBoolean("IsHouseRequired"); } }
        #endregion

        #region Constructors
        public QuestRequirements(Quest quest) {
            _Quest = quest;
        }
        #endregion

        #region Build
        private ClassJobRequirement[] BuildClassJobs() {
            const int Count = 2;

            var cjr = new List<ClassJobRequirement>();
            for (var i = 0; i < Count; ++i) {
                var cjc = Quest.As<ClassJobCategory>(i);
                var lv = Quest.AsInt32("ClassLevel", i);

                if (cjc.Key != 0 && lv > 0)
                    cjr.Add(new ClassJobRequirement(cjc, lv));
            }

            return cjr.ToArray();
        }
        private Quest[] BuildQuestExclusion() {
            const int Count = 3;

            var quests = new List<Quest>();
            for (var i = 0; i < Count; ++i) {
                var q = Quest.As<Quest>("QuestLock", i);
                if (q != null && q.Key != 0)
                    quests.Add(q);
            }
            return quests.ToArray();
        }
        #endregion
    }
}
