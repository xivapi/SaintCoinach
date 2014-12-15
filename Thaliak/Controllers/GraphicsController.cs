using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Controllers {
    [Export(typeof(GraphicsController))]
    public class GraphicsController {
        private IServiceLocator _ServiceLocator;
        private IEventAggregator _EventAggregator;


        [ImportingConstructor]
        public GraphicsController(IEventAggregator eventAggregator, IServiceLocator serviceLocator) {
            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");
            if (serviceLocator == null)
                throw new ArgumentNullException("serviceLocator");

            _EventAggregator = eventAggregator;
            _ServiceLocator = serviceLocator;

            _EventAggregator.GetEvent<Events.GraphicsViewRequestEvent>().Subscribe(OnGraphicsRequest);
        }

        private void OnGraphicsRequest(Events.GraphicsViewRequestArguments e) {
            var thread = new Thread(RunViewerAsync);
            thread.IsBackground = true;
            thread.Name = "3D";
            thread.Start(e);
        }

        static void RunViewerAsync(object state) {
            var e = (Events.GraphicsViewRequestArguments)state;

            var eng = new SaintCoinach.Graphics.ViewerEngine(e.Title);

            eng.Add(e.Component);

            eng.Run();

            GC.Collect();
        }
    }
}
