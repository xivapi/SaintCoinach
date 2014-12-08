using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Interfaces {
    public interface ITitledView {
        string Title { get; }
        event EventHandler TitleChanged;
    }
}
