using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert.Commands {
    public class DelegateCommand : System.Windows.Input.ICommand {
        #region Fields
        private Action _Target;
        #endregion

        #region Constructor
        public DelegateCommand(Action target) {
            _Target = target;
        }
        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter) {
            return true;
        }


        public void Execute(object parameter) {
            _Target();
        }

        #endregion

        #region ICommand Members

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged {
            add { }
            remove { }
        }

        #endregion
    }
    public class DelegateCommand<T> : System.Windows.Input.ICommand {
        #region Fields
        private Action<T> _Target;
        #endregion

        #region Constructor
        public DelegateCommand(Action<T> target) {
            _Target = target;
        }
        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter) {
            return parameter == null || (parameter is T);
        }

        public void Execute(object parameter) {
            _Target((T)parameter);
        }

        #endregion

        #region ICommand Members

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged {
            add { }
            remove { }
        }

        #endregion
    }
}
