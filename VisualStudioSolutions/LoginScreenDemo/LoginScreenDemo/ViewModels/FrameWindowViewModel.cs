namespace LoginDemoClient.ViewModels
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Windows;

   using LoginDemoClient.Bases;
   using LoginDemoClient.Interfaces;

   public class FrameWindowViewModel : ViewModelBase
   {
      private LoginViewModel _Login;
      private LoginViewModel Login
      {
         get
         {
            if (this._Login == null)
            {
               this._Login = new LoginViewModel();
               this._Login.OnChangeRequested += Login_LoginSuccessful;
            }

            return this._Login;
         }
      }

      private MainViewModel _Main;
      private MainViewModel Main
      {
         get
         {
            if (this._Main == null)
            {
               this._Main = new MainViewModel();
               this._Main.OnChangeRequested += Main_OnChangeRequested;
            }

            return this._Main;
         }
      }

      private IFrameContent _currentContent;
      public IFrameContent CurrentContent
      {
         get
         {
            return this._currentContent;
         }
         private set
         {
            this._currentContent = value;
            this.RaisePropertyChanged("CurrentContent");
         }
      }

      public FrameWindowViewModel()
      {         
         this.ChangeContent(this.Login);
      }

      private void Main_OnChangeRequested(object sender, EventArgs e)
      {
         ChangeContent(this.Login);
      }

      private void Login_LoginSuccessful(object sender, EventArgs e)
      {
         ChangeContent(this.Main);     
      }

      private bool ChangeContent(IFrameContent content)
      {
        this.CurrentContent = content;
        this.CurrentContent.OnActivated();

        return true;
      }
   }
}
