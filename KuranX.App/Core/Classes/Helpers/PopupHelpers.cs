using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using KuranX.App.Core.Classes.Tools;

namespace KuranX.App.Core.Classes.Helpers
{
    public static class PopupHelpers
    {
      

        public static  void popupClosed(DraggablePopupHelper drag , Popup popuptemp, Popup moveBar = null)
        {

            try
            {

                Tools.Tools.errWrite($"[{DateTime.Now} popupClosed_Click] -> NoteFrame");

                drag.Dispose();
                drag = null;
                if(moveBar != null) moveBar.IsOpen = false;
                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                Tools.Tools.logWriter("Click", ex);
            }
        }
    }
}
