using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Regions;

namespace Thaliak.Events {
    public class NavigationResultArguments {
        #region Properties
        public NavigationRequestArguments Request { get; set; }
        public NavigationResult NavigationResult { get; set; }
        #endregion
    }
}
