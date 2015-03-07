using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class QuestRequirements {
        public class PrerequisiteQuestsRequirement {
            #region Properties
            public IEnumerable<Quest> Quests { get; private set; }
            public PreqrequisiteType Type { get; private set; }
            #endregion

            #region Constructor
            internal PrerequisiteQuestsRequirement(Quest quest) {
                const int QuestCount = 3;

                this.Type = (PreqrequisiteType)quest.AsInt32("PreviousQuestJoin");

                var quests = new List<Quest>();
                for (var i = 0; i < QuestCount; ++i) {
                    var q = quest.As<Quest>("PreviousQuest", i);

                    if (q.Key != 0)
                        quests.Add(q);
                }
                this.Quests = quests;
            }
            #endregion
        }
    }
}
