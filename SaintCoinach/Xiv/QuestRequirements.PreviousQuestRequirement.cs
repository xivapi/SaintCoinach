using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class QuestRequirements {
        public class PreviousQuestRequirement {
            #region Properties
            public IEnumerable<Quest> PreviousQuests { get; private set; }
            public PrerequisiteQuestsRequirementType Type { get; private set; }
            #endregion

            #region Constructor
            internal PreviousQuestRequirement(Quest quest) {
                const int QuestCount = 3;

                this.Type = (PrerequisiteQuestsRequirementType)quest.AsInt32("PreviousQuestJoin");

                var contents = new List<Quest>();
                for (var i = 0; i < QuestCount; ++i) {
                    var c = quest.As<Quest>("PreviousQuest", i);

                    if (c != null && c.Key != 0)
                        contents.Add(c);
                }
                this.PreviousQuests = contents;
            }
            #endregion
        }
    }
}
