using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search.Sources {
    [Export(typeof(Interfaces.ISearchDataSource))]
    public class RecipeSource : GenericXivSource<Xiv.Recipe> {
        public override bool IncludeByDefault {
            get { return false; }
        }
    }
}