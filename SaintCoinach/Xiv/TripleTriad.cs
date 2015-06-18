using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TripleTriad : XivRow, ILocatable {
        #region Fields
        private ENpc[] _ENpcs;
        private TripleTriadCard[] _FixedCards;
        private TripleTriadCard[] _VariableCards;
        private TripleTriadRule[] _FixedRules;
        private PrerequisiteQuestsRequirement _QuestRequirement;
        #endregion

        #region Properties

        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = BuildENpcs()); } }
        public IEnumerable<TripleTriadCard> AllCards { get { return FixedCards.Concat(VariableCards); } }
        public IEnumerable<TripleTriadCard> FixedCards { get { return _FixedCards ?? (_FixedCards = BuildFixedCards()); } }
        public IEnumerable<TripleTriadCard> VariableCards { get { return _VariableCards ?? (_VariableCards = BuildVariableCards()); } }
        public IEnumerable<TripleTriadRule> FixedRules { get { return _FixedRules ?? (_FixedRules = BuildRules()); } }
        public bool UsesRegionalRules { get { return AsBoolean("UsesRegionalRules"); } }
        public int Fee { get { return AsInt32("Fee"); } }
        public PrerequisiteQuestsRequirement QuestRequirement { get { return _QuestRequirement ?? (_QuestRequirement = BuildQuestRequirement()); } }

        public int StartTime { get { return AsInt32("StartTime"); } }
        public int EndTime { get { return AsInt32("EndTime"); } }
        
        // TODO: DefaultTalk

        #endregion

        #region Constructors

        public TripleTriad(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build
        private ENpc[] BuildENpcs() {
            return Sheet.Collection.ENpcs.FindWithData(this.Key).ToArray();
        }
        private TripleTriadCard[] BuildFixedCards() {
            const int Count = 5;

            var cards = new List<TripleTriadCard>();
            for (var i = 0; i < Count; ++i) {
                var card = As<TripleTriadCard>("TripleTriadCard{Fixed}", i);
                if (card.Key != 0)
                    cards.Add(card);
            }

            return cards.ToArray();
        }
        private TripleTriadCard[] BuildVariableCards() {
            const int Count = 5;

            var cards = new List<TripleTriadCard>();
            for (var i = 0; i < Count; ++i) {
                var card = As<TripleTriadCard>("TripleTriadCard{Variable}", i);
                if (card.Key != 0)
                    cards.Add(card);
            }

            return cards.ToArray();
        }
        private TripleTriadRule[] BuildRules() {
            const int Count = 2;

            var rules = new List<TripleTriadRule>();
            for (var i = 0; i < Count; ++i) {
                var rule = As<TripleTriadRule>("TripleTriadRule", i);
                if (rule.Key != 0)
                    rules.Add(rule);
            }
            return rules.ToArray();
        }
        private PrerequisiteQuestsRequirement BuildQuestRequirement() {
            const int Count = 3;

            var type = (PrerequisiteQuestsRequirementType)AsInt32("PreviousQuestJoin");
            var quests = new List<Quest>();
            for (var i = 0; i < Count; ++i) {
                var q = As<Quest>("PreviousQuest", i);
                if (q.Key != 0)
                    quests.Add(q);
            }

            return new PrerequisiteQuestsRequirement(type, quests);
        }
        #endregion

        #region ILocatable Members

        IEnumerable<ILocation> ILocatable.Locations {
            get { return ENpcs.SelectMany(i => i.Locations); }
        }

        #endregion
    }
}
