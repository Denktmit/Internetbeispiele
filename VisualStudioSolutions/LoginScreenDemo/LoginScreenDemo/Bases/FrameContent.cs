namespace LoginDemoClient.Bases
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   using LoginDemoClient.Interfaces;

   public abstract class FrameContent : ViewModelBase, IFrameContent
   {
      public event EventHandler OnChangeRequested;
      
      protected void Deactivate()
      {
         if (this.CanDeactivated())
         {
            this.OnDeactivated();

            var handler = OnChangeRequested;
            if (handler != null)
            {
               handler(this, EventArgs.Empty);
            }
         }
      }

      public virtual void OnActivated()
      {
      }

      public virtual void OnDeactivated()
      {
      }

      public virtual bool CanDeactivated()
      {
         return true;
      }      
   }
}
