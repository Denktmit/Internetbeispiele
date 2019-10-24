using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFUtils {

  public class WPFCommand : ICommand {

    private bool _windowParameter;

    // Action ist ein spezieller Delegate! (Funktionszeiger)
    private Action _execute;
    private Action<Window> _executeWP;
    // Func ist ein spezieller Delegate mit einem Rueckgabewert vom Typ Boolean! (Funktionszeiger)
    private Func<bool> _canExecute;

    public WPFCommand(Action execute) : this(execute, null) { }

    public WPFCommand(Action execute, Func<bool> canExecute) {
      _windowParameter = false;
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public WPFCommand(Action<Window> execute, Func<bool> canExecute) {
      _windowParameter = true;
      this._executeWP = execute;
      this._canExecute = canExecute;
      ;
    }

    public event EventHandler CanExecuteChanged;

    public void RaiseCanExecuteChanged() {
      if (CanExecuteChanged != null) {
        CanExecuteChanged(this, new EventArgs());
      }
      // oder CanExecuteChanged.Invoke(this, new EventArgs());
    }

    public bool CanExecute(object parameter) {
      return (_canExecute != null) ? _canExecute() : true;
    }

    // Wird aufgerufen beim Click vom Button
    public void Execute(object parameter) {
      if (_windowParameter) {
        _executeWP((Window)parameter);
      }
      else {
        _execute();
      }
    }

  }

}
