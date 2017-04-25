using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    using Ex;

    public class LanguageCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public LanguageCommand(ARealmReversed realm)
            : base("lang", "Change the language.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                OutputInformation("Current language: {0}", _Realm.GameData.ActiveLanguage);
                return true;
            }
            paramList = paramList.Trim();
            if (!Enum.TryParse<Language>(paramList, out var newLang)) {
                newLang = LanguageExtensions.GetFromCode(paramList);
                if (newLang == Language.Unsupported) {
                    OutputError("Unknown language.");
                    return false;
                }
            }
            _Realm.GameData.ActiveLanguage = newLang;
            return true;
        }
    }
}
