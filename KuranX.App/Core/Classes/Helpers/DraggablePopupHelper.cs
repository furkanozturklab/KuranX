using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;

namespace KuranX.App.Core.Classes.Helpers
{
    public class DraggablePopupHelper : IDisposable

    {

        private bool isDragging = false;
        private Point lastMousePosition;
        private Popup actionPopup;
        private Border border;

      

      
        public DraggablePopupHelper(Border selectBorder, Popup selectPopup)
        {
            border = selectBorder;
            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            border.MouseLeftButtonUp += Border_MouseLeftButtonUp;
            border.MouseMove += Border_MouseMove;
            actionPopup = selectPopup;

            Debug.WriteLine("inizilazie draggble popup");
        }


        public void Dispose()
        {
            border.MouseLeftButtonDown -= Border_MouseLeftButtonDown;
            border.MouseLeftButtonUp -= Border_MouseLeftButtonUp;
            border.MouseMove -= Border_MouseMove;
            actionPopup.HorizontalOffset = 0;
            actionPopup.VerticalOffset = 0;
   

        }



        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("down button");
            isDragging = true;
            var border = sender as Border;
            lastMousePosition = e.GetPosition(border);
            border.CaptureMouse();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var border = sender as Border;
            border.ReleaseMouseCapture();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(null);
                double left = mousePosition.X - lastMousePosition.X + actionPopup.HorizontalOffset;
                double top = mousePosition.Y - lastMousePosition.Y + actionPopup.VerticalOffset;
                actionPopup.HorizontalOffset = left;
                actionPopup.VerticalOffset = top;
            }
        }







    }
}
