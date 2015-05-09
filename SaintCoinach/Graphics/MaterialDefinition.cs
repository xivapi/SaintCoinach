using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class MaterialDefinition {
        #region Path expansion
        /* Found referenced in random models (no idea which ones), and still have to be located
            /lambert1.mtrl
            /Lambert139.mtrl
            /Lambert140.mtrl
            /Lambert141.mtrl
            /Lambert148.mtrl
            /motion_Black.mtrl
            /motion_Black5.mtrl
            /motion_gray5.mtrl
            /motion_red.mtrl
            /motion_Skin.mtrl
            /motion_Skin5.mtrl
            /motion_White.mtrl
            /motion_White5.mtrl
            /Scene_Material.mtrl
            /Scene_Material0.mtrl
            /Scene_Material11.mtrl
            /Scene_Material12.mtrl
         */

        private class PathExpander {
            public Regex Pattern { get; private set; }
            public string Replacement { get; private set; }
            public string StainReplacement { get; private set; }
            public bool ContainsVariant { get; private set; }

            public PathExpander(string pattern, string replacementFormat, string replacementStainFormat, bool containsVariant) {
                this.Pattern = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled);
                this.Replacement = replacementFormat;
                this.StainReplacement = replacementStainFormat;
                this.ContainsVariant = containsVariant;
            }

            public bool TryExpand(string input, out string path, out string stainedPath) {
                if (!Pattern.IsMatch(input)) {
                    path = null;
                    stainedPath = null;
                    return false;
                }
                path = Pattern.Replace(input, Replacement);
                if (string.IsNullOrWhiteSpace(StainReplacement))
                    stainedPath = null;
                else
                    stainedPath = Pattern.Replace(input, StainReplacement);
                return true;
            }
        }

        static PathExpander[] PathExpanders = new PathExpander[] {
            new PathExpander(
                @"^/(?<basename>mt_c(?<c>[0-9]{4})a(?<a>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/accessory/a${a}/material/v{0:D4}/${basename}.mtrl",
                @"chara/accessory/a${a}/material/v{0:D4}/staining/${basename}_s{1:D4}.mtrl",
                true),
            new PathExpander(
                @"^/(?<basename>mt_c(?<c>[0-9]{4})b(?<b>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/human/c${c}/obj/body/b${b}/material/${basename}.mtrl",
                null,
                false),
            new PathExpander(
                @"^/(?<basename>mt_c(?<c>[0-9]{4})h(?<h>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/human/c${c}/obj/hair/h${h}/material/v{0:D4}/${basename}.mtrl",
                null,
                true),
            new PathExpander(
                @"^/(?<basename>mt_c(?<c>[0-9]{4})f(?<f>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/human/c${c}/obj/face/f${f}/material/${basename}.mtrl",
                null,
                false),
            new PathExpander(
                @"^/(?<basename>mt_c(?<c>[0-9]{4})t(?<t>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/human/c${c}/obj/tail/t${t}/material/${basename}.mtrl",
                null,
                false),
            new PathExpander(
                @"^/(?<basename>mt_c(?<c>[0-9]{4})e(?<e>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/equipment/e${e}/material/v{0:D4}/${basename}.mtrl",
                @"chara/equipment/e${e}/material/v{0:D4}/staining/${basename}_s{1:D4}.mtrl",
                true),
            new PathExpander(
                @"^/(?<basename>mt_d(?<d>[0-9]{4})e(?<e>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/demihuman/d${d}/obj/equipment/e${e}/material/v{0:D4}/${basename}.mtrl",
                null,
                true),
            new PathExpander(
                @"^/(?<basename>mt_m(?<m>[0-9]{4})b(?<b>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/monster/m${m}/obj/body/b${b}/material/v{0:D4}/${basename}.mtrl",
                null,
                true),
            new PathExpander(
                @"^/(?<basename>mt_w(?<w>[0-9]{4})b(?<b>[0-9]{4})_(?<suffix>[^\.]+))\.mtrl$",
                @"chara/weapon/w${w}/obj/body/b${b}/material/v{0:D4}/${basename}.mtrl",
                @"chara/weapon/w${w}/obj/body/b${b}/material/v{0:D4}/staining/${basename}_s{1:D4}.mtrl",
                true),
        };

        static bool TryExpand(string input, out string path, out string stainedPath, out bool containsVariants) {
            var i = input.LastIndexOf('/');
            var search = input.Substring(i);

            foreach (var pe in PathExpanders) {
                if (pe.TryExpand(search, out path, out stainedPath)) {
                    containsVariants = pe.ContainsVariant;
                    return true;
                }
            }
            path = null;
            stainedPath = null;
            containsVariants = false;
            return false;
        }
        #endregion

        #region Fields
        private string _DefaultPath;
        private string _PathFormat;
        private string _StainedPathFormat;
        private bool _VariantsAvailable;
        private IO.PackCollection _Packs;
        #endregion

        #region Properties
        public ModelDefinition Definition { get; private set; }
        public string Name { get { return Definition.MaterialNames[Index]; } }
        public bool VariantsAvailable { get { return _VariantsAvailable; } }
        public bool StainsAvailable { get { return _StainedPathFormat != null; } }
        public int Index { get; private set; }
        #endregion

        #region Constructor
        internal MaterialDefinition(ModelDefinition definition, int index) {
            this.Definition = definition;
            this.Index = index;
            _Packs = definition.File.Pack.Collection;

            if (_Packs.FileExists(Name)) {
                _DefaultPath = Name;
                _VariantsAvailable = false;
                _StainedPathFormat = _PathFormat = null;
            } else {
                if (!TryExpand(Name, out _PathFormat, out _StainedPathFormat, out _VariantsAvailable))
                    throw new NotSupportedException();

                if (VariantsAvailable)
                    _DefaultPath = string.Format(_PathFormat, 0);
                else
                    _DefaultPath = _PathFormat;
            }
        }
        #endregion

        #region Get
        public Material Get(ModelVariantIdentifier variantId) {
            if (variantId.StainKey.HasValue && StainsAvailable)
                return Get(variantId.ImcVariant, variantId.StainKey.Value);
            return Get(variantId.ImcVariant);
        }
        public Material Get() {
            var path = _DefaultPath;
            return Create(path, ImcVariant.Default);
        }
        public Material Get(ImcVariant variant) {
            var path = _DefaultPath;
            if (VariantsAvailable)
                path = string.Format(_PathFormat, variant.Variant & 0xFF);
            return Create(path, variant);
        }
        public Material Get(ImcVariant variant, int stainKey) {
            if (!StainsAvailable)
                throw new NotSupportedException();

            var path = string.Format(_StainedPathFormat, variant.Variant & 0xFF, stainKey);
            return Create(path, variant);
        }

        private Material Create(string path, ImcVariant variant) {
            var file = _Packs.GetFile(path);
            return new Material(this, file, variant);
        }
        #endregion
    }
}
