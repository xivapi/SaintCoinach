using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CustomTalk : XivRow {
        public struct ScriptCommand {
            public string Instruction;
            public int Argument;
        }

        #region Fields

        private ENpc[] _ENpcs;
        private ScriptCommand[] _ScriptCommands;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Text { get { return AsString("Text"); } }

        public IEnumerable<ENpc> ENpcs { get { return _ENpcs ?? (_ENpcs = Sheet.Collection.ENpcs.FindWithData(Key).ToArray()); } }
        public IEnumerable<ScriptCommand> ScriptCommands { get { return _ScriptCommands ?? (_ScriptCommands = BuildScriptCommands()); } }

        #endregion

        #region Constructors
        
        public CustomTalk(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        
        #endregion

        private ScriptCommand[] BuildScriptCommands() {
            const int Count = 30;

            var commands = new List<ScriptCommand>();

            for(var i = 0; i < Count; ++i) {
                var instr = AsString("Script{Instruction}", i).ToString();
                if (string.IsNullOrWhiteSpace(instr))
                    continue;
                commands.Add(new ScriptCommand {
                    Instruction = instr,
                    Argument = AsInt32("Script{Arg}", i)
                });
            }

            return commands.ToArray();
        }

        public override string ToString() {
            return Name;
        }
    }
}
