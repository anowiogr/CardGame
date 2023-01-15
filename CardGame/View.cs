using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CardGame
{
    public class View
    {
        protected Image back = null;
        public virtual string imagePath => $"images/{GetType().BaseType.Name}/{GetType().Name}.png";

        public Grid Grid { get; set; }

        public View()
        {
            Grid = MainWindow.GameBoard;
 
        }

        public virtual Image Image
        {
            get
            {
                if (back == null)
                {
                    back = new Image();
                    back.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                    Grid.Children.Add(back);
                }
                return back;
            }
        }
        public virtual int X
        {
            get { return Grid.GetColumn(Image); }
            set { Grid.SetColumn(Image, value); }
        }

        public virtual int Y
        {
            get { return Grid.GetRow(Image); }
            set { Grid.SetRow(Image, value); }
        }
    }
}
