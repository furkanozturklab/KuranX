using KuranX.App.Core.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace KuranX.App.Core.UC.PopupC
{
    /// <summary>
    /// Interaction logic for MoveBar.xaml
    /// </summary>
    public partial class MoveBarController : UserControl
    {


        public static readonly DependencyProperty HeaderTextProp =
         DependencyProperty.Register("HeaderText", typeof(string), typeof(MoveBarController), new PropertyMetadata("Hareket Kontrolu")); // Varsayılan değeri null olarak ayarla

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProp); }
            set { SetValue(HeaderTextProp, value); }
        }


        public Popup pp_moveBar, movePP;
        public Page? useFrame;

     



        public MoveBarController()
        {
            InitializeComponent();
        }


        public void parentFind()
        {

            DependencyObject parent = this.Parent;
            while (parent != null)
            {

                if (parent is Movebar selected)
                {
                    pp_moveBar = selected.getPopupMove();
                    movePP = selected.getPopupBase();
                    break;
                }

                parent = LogicalTreeHelper.GetParent(parent);
            }



        }

        public void ppMoveActionOpacity_Click(object sender, RoutedEventArgs e)
        {

            Tools.errWrite($"[{DateTime.Now} ppMoveActionOpacity_Click] -> MoveBarController");
            parentFind();
            var btntemp = sender as Button;


            switch (btntemp.Uid.ToString())
            {
                case "Up":
                    movePP.Child.Opacity = 1;
                    movePP.Child.IsEnabled = true;
                    break;

                case "Down":
                    movePP.Child.Opacity = 0.1;
                    movePP.Child.IsEnabled = false;
                    break;
            }
        }

        public void ppMoveActionOfset_Click(object sender, RoutedEventArgs e)
        {

            Tools.errWrite($"[{DateTime.Now} ppMoveActionOfset_Click] -> MoveBarController");
            parentFind();
  
            var btntemp = sender as Button;


            switch (btntemp.Uid.ToString())
            {
                case "Left":
                    movePP.HorizontalOffset -= 50;
                    break;

                case "Top":
                    movePP.VerticalOffset -= 50;
                    break;

                case "Bottom":
                    movePP.VerticalOffset += 50;
                    break;

                case "Right":
                    movePP.HorizontalOffset += 50;
                    break;

                case "UpLeft":
                    movePP.Placement = PlacementMode.Absolute;
                    movePP.VerticalOffset = 0;
                    movePP.HorizontalOffset = 0;
                    break;

                case "Reset":
                    movePP.Placement = PlacementMode.Center;
                    movePP.VerticalOffset = 0;
                    movePP.HorizontalOffset = 0;
                    break;

                case "Close":
                    pp_moveBar.IsOpen = false;
                    movePP.Child.Opacity = 1;
                    break;
            }
        }


    }
}