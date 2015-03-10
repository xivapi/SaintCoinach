using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    partial class QuestRequirements {
        public class InstanceContentRequirement {
            #region Properties
            public IEnumerable<InstanceContent> InstanceContents { get; private set; }
            public PrerequisiteQuestsRequirementType Type { get; private set; }
            #endregion

            #region Constructor
            internal InstanceContentRequirement(Quest quest) {
                const int QuestCount = 3;

                this.Type = (PrerequisiteQuestsRequirementType)quest.AsInt32("InstanceContentJoin");

                var contents = new List<InstanceContent>();
                for (var i = 0; i < QuestCount; ++i) {
                    var c = quest.As<InstanceContent>("InstanceContent", i);

                    if (c.Key != 0)
                        contents.Add(c);
                }
                this.InstanceContents = contents;
            }
            #endregion

        }
    }
}
