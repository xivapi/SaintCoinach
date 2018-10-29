using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.RendererSources {
    public class MonsterImageRendererSource : BaseImageRendererSource {
        private IEnumerator<Xiv.ModelChara> _MonsterIterator;

        public MonsterImageRendererSource(IEnumerable<Xiv.ModelChara> monsters) {
            _MonsterIterator = monsters.ToList().GetEnumerator();
        }

        public override bool MoveNext() {
            while (_MonsterIterator.MoveNext()) {
                var modelChara = _MonsterIterator.Current;
                var modelIdentifier = $"{modelChara.ModelKey}-{modelChara.BaseKey}-{modelChara.Variant}";
                CurrentName = $"{modelChara.Type}/{modelIdentifier}";

                try {
                    var result = modelChara.GetModelDefinition();
                    var model = result.Item1;
                    var variant = result.Item2;
                    CurrentComponent = new Content.ContentModel(_Engine, variant, model);
                    CurrentTargetFile = new System.IO.FileInfo(System.IO.Path.Combine("Renders", "Monsters", modelIdentifier));
                    CurrentBoundingBox = model.BoundingBoxes.Value2.Scale(1.3f);
                } catch (InvalidOperationException) {
                    // Can't find this model def, move to the next.
                    continue;
                }

                return true;
            }

            return false;
        }

        public override void Reset(Engine engine) {
            base.Reset(engine);

            _MonsterIterator.Reset();
        }
    }
}
