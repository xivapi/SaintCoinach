using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.RendererSources {
    public class EquipmentImageRendererSource : BaseImageRendererSource {
        #region Fields
        private bool _IsCurrentStained;
        private ModelDefinition _CurrentModel;
        private ImcVariant _CurrentVariant;
        private IEnumerator<Xiv.Items.Equipment> _EquipmentIterator;
        private IEnumerator<Xiv.Stain> _StainEnumerator;
        #endregion

        #region Constructor
        public EquipmentImageRendererSource(IEnumerable<Xiv.Item> items) : this(items, null) { }
        public EquipmentImageRendererSource(IEnumerable<Xiv.Item> items, IEnumerable<Xiv.Stain> stains) : this((IEnumerable<Xiv.Items.Equipment>)items.OfType<Xiv.Items.Equipment>(), stains) { }
        public EquipmentImageRendererSource(IEnumerable<Xiv.Items.Equipment> equipmentItems) : this(equipmentItems, null) { }
        public EquipmentImageRendererSource(IEnumerable<Xiv.Items.Equipment> equipmentItems, IEnumerable<Xiv.Stain> stains) {
            // Doing a ToList first LINQ enumerators can't reset.
            _EquipmentIterator = equipmentItems.ToList().GetEnumerator();
            if (stains != null)
                _StainEnumerator = stains.ToList().GetEnumerator();
        }
        #endregion

        #region IImageRendererSource Members

        public override bool MoveNext() {
            if (MoveNextStain())
                return true;

            while (_EquipmentIterator.MoveNext()) {
                var eq = _EquipmentIterator.Current;
                var charType = eq.GetModelCharacterType();
                    charType += 100;

                _CurrentModel = eq.GetModel(charType, out _CurrentVariant);
                if (_CurrentModel == null)
                    continue;

                RenderFromOppositeSide = eq.ItemUICategory.Key == 1 || eq.ItemUICategory.Key == 10;     // PGL/MNK and books
                CurrentBoundingBox = _CurrentModel.BoundingBoxes.Value2.Scale(1.35f);    // Not sure what the first one is for, but some are very strange.

                if (_StainEnumerator == null)
                    _IsCurrentStained = false;
                else
                    _IsCurrentStained = eq.IsDyeable;

                if (_IsCurrentStained) {
                    _StainEnumerator.Reset();
                    MoveNextStain();
                } else {
                    CurrentComponent = new Content.ContentModel(_Engine, _CurrentVariant, _CurrentModel);
                    CurrentTargetFile = GetTargetFile(_EquipmentIterator.Current);
                    CurrentName = GetName(_EquipmentIterator.Current);
                }

                return true;
            }
            return false;
        }
        private bool MoveNextStain() {
            if (!_IsCurrentStained)
                return false;
            if (!_StainEnumerator.MoveNext())
                return false;

            var variant = new ModelVariantIdentifier {
                ImcVariant = _CurrentVariant,
                StainKey = null
            };
            if(_StainEnumerator.Current.Key > 0)
                variant.StainKey = _StainEnumerator.Current.Key;

            CurrentComponent = new Content.ContentModel(_Engine, variant, _CurrentModel);
            CurrentTargetFile = GetTargetFile(_EquipmentIterator.Current, _StainEnumerator.Current);
            CurrentName = GetName(_EquipmentIterator.Current, _StainEnumerator.Current);

            return true;
        }

        protected System.IO.FileInfo GetTargetFile(Xiv.Items.Equipment item) {
            return GetTargetFile(item, null);
        }
        protected virtual System.IO.FileInfo GetTargetFile(Xiv.Items.Equipment item, Xiv.Stain stain) {
            const int DirectorySeperationInterval = 100;

            var fileName = string.Format("{0} - {1}", item.Key, item.Name);
            if (stain != null)
                fileName += string.Format(" (s{0:D4} {1})", stain.Key, stain.Name);
            return new System.IO.FileInfo(System.IO.Path.Combine("Renders", (item.Key - item.Key % DirectorySeperationInterval).ToString(), fileName));
        }
        protected string GetName(Xiv.Items.Equipment item) {
            return GetName(item, null);
        }
        protected virtual string GetName(Xiv.Items.Equipment item, Xiv.Stain stain) {
            var sb = new StringBuilder();
            sb.Append(item.Name);
            if(stain != null && stain.Key > 0)
                sb.AppendFormat(" (s{0:D4} {1})", stain.Key, stain.Name);
            return sb.ToString();
        }

        public override void Reset(Engine engine) {
            base.Reset(engine);

            _EquipmentIterator.Reset();
        }

        #endregion
    }
}
