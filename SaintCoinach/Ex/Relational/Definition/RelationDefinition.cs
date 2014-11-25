using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class RelationDefinition : ICloneable {
        #region Fields
        private bool _IsCompiled = false;
        private ICollection<SheetDefinition> _SheetDefinitions = new List<SheetDefinition>();
        private Dictionary<string, SheetDefinition> _SheetMap = null;
        #endregion

        #region Properties
        public ICollection<SheetDefinition> SheetDefinitions {
            get { return _SheetDefinitions; }
            internal set {
                _SheetDefinitions = value;
            }
        }
        #endregion

        #region Helpers
        public bool TryGetSheet(string name, out SheetDefinition def) {
            if (_IsCompiled)
                return _SheetMap.TryGetValue(name, out def);

            var res = SheetDefinitions.Where(_ => string.Equals(_.Name, name, StringComparison.OrdinalIgnoreCase));
            if (res.Any())
                def = res.First();
            else
                def = null;

            return (def != null);
        }

        public SheetDefinition GetOrCreateSheet(string name) {
            SheetDefinition def;
            if (!TryGetSheet(name, out def))
                SheetDefinitions.Add(def = new SheetDefinition() { Name = name });
            return def;
        }
        #endregion

        #region Compile
        public void Compile() {
            _SheetMap = _SheetDefinitions.ToDictionary(_ => _.Name, _ => _, StringComparer.OrdinalIgnoreCase);

            foreach (var sheet in SheetDefinitions)
                sheet.Compile();

            _IsCompiled = true;
        }
        #endregion

        #region ICloneable Members

        object ICloneable.Clone() {
            throw new NotImplementedException();
        }

        #endregion

        #region Serialization
        public void Serialize(string filePath) {
            using (var writer = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
                Serialize(writer);
        }
        public void Serialize(System.IO.TextWriter writer) {
            var serializer = new Serialization.ExRelationSerializer();
            serializer.Serialize(writer, this);
        }

        public static RelationDefinition Deserialize(string filePath) {
            using (var reader = new System.IO.StreamReader(filePath, Encoding.UTF8))
                return Deserialize(reader);
        }
        public static RelationDefinition Deserialize(System.IO.TextReader reader) {
            var deserializer = new YamlDotNet.Serialization.Deserializer();

            deserializer.RegisterTagMapping("tag:yaml.org,2002:color_conv", typeof(ValueConverters.ColorConverter));
            deserializer.RegisterTagMapping("tag:yaml.org,2002:icon_conv", typeof(ValueConverters.IconConverter));
            deserializer.RegisterTagMapping("tag:yaml.org,2002:link_conv", typeof(ValueConverters.SheetLinkConverter));

            deserializer.RegisterTagMapping("tag:yaml.org,2002:group_def", typeof(Definition.GroupDataDefinition));
            deserializer.RegisterTagMapping("tag:yaml.org,2002:repeat_def", typeof(Definition.RepeatDataDefinition));
            deserializer.RegisterTagMapping("tag:yaml.org,2002:single_def", typeof(Definition.SingleDataDefinition));

            return deserializer.Deserialize<Definition.RelationDefinition>(reader);
        }
        #endregion
    }
}
