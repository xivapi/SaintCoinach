using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using SharpYaml.Serialization;

namespace SaintCoinach.Ex.Relational.Definition {
    public class RelationDefinition {
        #region Fields

        private bool _IsCompiled;
        private ICollection<SheetDefinition> _SheetDefinitions = new List<SheetDefinition>();
        private Dictionary<string, SheetDefinition> _SheetMap;
        private Dictionary<string, SheetDefinition> _SheetDefinitionMap = new Dictionary<string, SheetDefinition>();

        #endregion

        #region Properties

        public ICollection<SheetDefinition> SheetDefinitions {
            get { return _SheetDefinitions; }
            internal set { _SheetDefinitions = value; }
        }

        public string Version { get; set; }

        #endregion

        #region Compile

        public void Compile() {
            _SheetMap = _SheetDefinitions.ToDictionary(_ => _.Name, _ => _);

            foreach (var sheet in SheetDefinitions)
                sheet.Compile();

            _IsCompiled = true;
        }

        #endregion

        #region Helpers

        public bool TryGetSheet(string name, out SheetDefinition def) {
            if (_IsCompiled)
                return _SheetMap.TryGetValue(name, out def);

            return _SheetDefinitionMap.TryGetValue(name, out def);
        }

        public SheetDefinition GetOrCreateSheet(string name) {
            if (!TryGetSheet(name, out var def)) {
                def = new SheetDefinition { Name = name };
                _SheetDefinitions.Add(def);
                _SheetDefinitionMap[name] = def;
            }
            return def;
        }

        #endregion
    }
}
