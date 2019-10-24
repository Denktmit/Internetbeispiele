namespace LoginDemoClient.Commands
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Windows.Input;

   public class RelayCommand : ICommand
   {
      public event EventHandler CanExecuteChanged;

      public Action<object> Action { get; private set; }

      public Predicate<object> Predicate { get; private set; }

      public RelayCommand(Action<object> action, Predicate<object> predicate)
      {
         this.Action = action;
         this.Predicate = predicate;
      }

      public bool CanExecute(object parameter)
      {
         return this.Predicate == null || this.Predicate(parameter);
      }      

      public void Execute(object parameter)
      {
         if (this.Action != null)
         {
            this.Action(parameter);
         }
      }
   }
}
