using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.PubSubEvents;

namespace Thaliak.Events {
    public class TabCloseRequestEvent : PubSubEvent<Guid> {

    }
}
