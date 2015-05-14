using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public static class StringTokens {
        public const string TagOpen = "<";
        public const string TagClose = ">";
        public const string ElementClose = "/";
        public const string ArgumentsOpen = "(";
        public const string ArgumentsClose = ")";
        public const string ArgumentsSeperator = ",";

        public const string ElseTag = TagOpen + "Else" + ElementClose + TagClose;
        public const string CaseTagName = "Case";

        public const string InvertName = "Not";

        public const string TopLevelParameterName = "TopLevelParameter";
    }
}
