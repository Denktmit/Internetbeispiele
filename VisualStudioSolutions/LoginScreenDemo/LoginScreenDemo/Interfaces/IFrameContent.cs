namespace LoginDemoClient.Interfaces
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   public interface IFrameContent
   {
      void OnActivated();      

      bool CanDeactivated();

      event EventHandler OnChangeRequested;
   }
}
