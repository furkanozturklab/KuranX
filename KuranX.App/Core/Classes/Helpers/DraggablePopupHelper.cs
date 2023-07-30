using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;


namespace KuranX.App.Core.Classes.Helpers
{
    public class DraggablePopupHelper : IDisposable

    {

        private bool isDragging = false;
        private Point lastMousePosition;
        private Popup actionPopup;
        private Border border;
  
      

      
        public DraggablePopupHelper( Popup selectPopup)
        {
            border = (Border)selectPopup.Child;
            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            border.MouseLeftButtonUp += Border_MouseLeftButtonUp;
            border.MouseMove += Border_MouseMove;   
            actionPopup = selectPopup;
            actionPopup.Opacity = 1;

            
        }


        public void Dispose()
        {
            border.MouseLeftButtonDown -= Border_MouseLeftButtonDown;
            border.MouseLeftButtonUp -= Border_MouseLeftButtonUp;
            border.MouseMove -= Border_MouseMove;
            //actionPopup.Opacity = 0;
            //actionPopup.HorizontalOffset = 0;
            //actionPopup.VerticalOffset = 0;
   

        }

        public Popup getPopup()
        {
            if (actionPopup != null) return actionPopup;
            else return null;
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

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
