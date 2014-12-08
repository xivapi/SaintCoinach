using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Thaliak {
    public class EnterpriseLoggingAdapter : ILoggerFacade {
        public EnterpriseLoggingAdapter() {
            
            Logger.SetLogWriter(new LogWriterFactory().Create());
        }

        #region ILoggerFacade Members

        public void Log(string message, Category category, Priority priority) {
            Logger.Write(message, category.ToString(), (int)priority);
        }

        #endregion
    }
}
