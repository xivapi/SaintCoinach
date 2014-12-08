using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Events {
    public class NavigationRequestArguments {
        private Guid _Id = Guid.NewGuid();

        #region Properties
        public Guid Id { get { return _Id; } }
        public Uri Uri { get; set; }
        public string Region { get; set; }
        public bool ForceNewTab { get; set; }
        #endregion
    }
}
