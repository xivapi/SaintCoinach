using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Events {
    public class NavigationRequestEvent : PubSubEvent<NavigationRequestArguments> {
        private Services.IViewLocator _ViewLocator;
        private Services.IViewLocator ViewLocator { get { return _ViewLocator ?? (_ViewLocator = ServiceLocator.Current.GetInstance<Services.IViewLocator>()); } }

        public Guid Publish(object target, bool forceNewTab) {
            return Publish(target, forceNewTab, null);
        }
        public Guid Publish(object target, bool forceNewTab, string region) {
            var uri = ViewLocator.GetUri(target, ViewContexts.MainContext);
            if (uri == null)
                return Guid.Empty;

            var args = new NavigationRequestArguments {
                ForceNewTab = forceNewTab,
                Uri = uri,
                Region = region
            };
            Publish(args);
            return args.Id;
        }
    }
}
