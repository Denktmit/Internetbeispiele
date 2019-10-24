namespace LoginDemoClient.ViewModels
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Threading;

   using LoginDemoClient.Bases;
   using LoginDemoClient.Commands;   

   public class MainViewModel : FrameContent
   {
      public RelayCommand LogoutCommand { get; private set; }

      public MainViewModel()
      {
         this.LogoutCommand = new RelayCommand(Logout, p => true);
      }

      public override void OnActivated()
      {
         
      }

      private void Logout(object parameter)
      {
         this.Deactivate();
      }      
   }
}
