using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class ColorMap {
        public IO.File File { get; private set; }
        public Color[] Colors { get; private set; }

        public ColorMap(IO.File file) {
            this.File = file;

            Build();
        }

        private void Build() {
            var buffer = File.GetData();
            this.Colors = new Color[buffer.Length / 4];

            for (var i = 0; i < buffer.Length; i += 4) {
                var r = buffer[i];
                var g = buffer[i + 1];
                var b = buffer[i + 2];
                var a = buffer[i + 3];
                this.Colors[i / 4] = Color.FromArgb(a, r, g, b);
            }
        }
    }
}
