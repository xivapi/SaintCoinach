using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.PubSubEvents;

namespace Thaliak.Events {
    public class NavigationResultEvent : PubSubEvent<NavigationResultArguments> {
        public void Publish(Guid id, Microsoft.Practices.Prism.Regions.NavigationResult result) {
            Publish(new NavigationResultArguments {
                Id = id,
                NavigationResult = result
            });
        }
    }
}
