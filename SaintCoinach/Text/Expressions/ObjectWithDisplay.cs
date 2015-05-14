using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Expressions {
    public class ObjectWithDisplay {
        private readonly object _Display;
        private readonly object _Object;

        public object Display { get { return _Display; } }
        public object Object { get { return _Object; } }

        public ObjectWithDisplay(object display, object obj) {
            if (display == null)
                throw new ArgumentNullException("display");
            if (obj == null)
                throw new ArgumentNullException("obj");
            _Display = display;
            _Object = obj;
        }

        public override string ToString() {
            if (Display == null)
                return string.Empty;
            return Display.ToString();
        }
    }
}
