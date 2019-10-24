namespace LoginDemoClient.ViewModels
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   using LoginDemoClient.Bases;
   using LoginDemoClient.Commands;

   public class LoginViewModel : FrameContent
   {      
      public RelayCommand LoginCommand { get; private set; }

      public LoginViewModel()
      {
         this.LoginCommand = new RelayCommand(Login, (p) => true);
      }

      private void Login(object parameter)
      {
         this.Deactivate();
      }
   }
}
