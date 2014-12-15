using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Events {
    public class GraphicsViewRequestEvent : PubSubEvent<GraphicsViewRequestArguments> {
    }
}
