using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;

namespace Thaliak.Modules.Core
{
    [ModuleExport("CoreModule", typeof(CoreModule))]
    public class CoreModule : IModule {
        public void Initialize() {

        }
    }
}
