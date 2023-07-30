using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;


namespace KuranX.App.Core.Classes.Helpers
{
    public static class PopupHelpers
    {


        public static List<string> activeDrag = new List<string>();
        public static List<(DraggablePopupHelper, string)> drag = new List<(DraggablePopupHelper, string)>();

        public static void popupClosed(Popup popupName, Popup moveBar = null)
        {

            try
            {

                Tools.Tools.errWrite($"[{DateTime.Now} popupClosed_Click] -> NoteFrame");

                dispose_drag(popupName);

                if (moveBar != null) moveBar.IsOpen = false;
                popupName.IsOpen = false;
            }
            catch (Exception ex)
            {
                Tools.Tools.logWriter("PopupHelpers", ex);
            }
        }

        public static void load_drag(Popup popupName)
        {
            try
            {
                if (drag.FirstOrDefault(e => e.Item2 == popupName.Name) == default)
                {
                    drag.Add((new DraggablePopupHelper(popupName), popupName.Name));
                }
            }
            catch (Exception ex)
            {
                Tools.Tools.logWriter("PopupHelpers", ex);
            }
        }

        public static void dispose_drag(Popup popupName)
        {

            try
            {
                var result = drag.FirstOrDefault(e => e.Item2 == popupName.Name);
                if (result != default)
                {

                    result.Item1.Dispose();
                    result.Item1 = null;
                    drag.RemoveAll(e => e.Item2 == popupName.Name);
                }
            }
            catch (Exception ex)
            {
                Tools.Tools.logWriter("PopupHelpers", ex);
            }


        }
    }
}
