using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace CardGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /* Szybki opis gry
     * --------------------
     * Plansza 16x16 pól
     * każde pole posiada "rodzaj" (złoża złota, stali, ropy, pola uprawne, fontanny życia) ustawione w sposób randomowy
     * gra dla dwóch userów
     * talia kart wspólna dla obu = razem 70 kart (35kart dla usera, 7 rund po 5 kart(losowanych z wspólnej talii))
     * na start każdy user otrzyma jednostki życia i ropy w ilości =
     * posiadamy 16 rawiantów kart co oznacza że karty będą po 4 z każdego rodzaju w talii + 6 dodatkowcych z tych rodzajów (które do ustalenia)
     * karty do zmiany pola wymagają ropy. jeden ruch = 1 jednostka, jeżeli zabraknie ropy, ruch nie może się odbyć
     * rodzaje kart zdefinowane przez prowadzącego zajęcia w pliku z ogólnym opisem gry
     * 
     * Faza ruchu:
     * ważna jest kolejność ułożenia kart przez użytkownika >> od tego zależeć będzie kolejność wykonywania ruchów
     * wygrany to ten który ma najwięcej złota, w przypadku równej wartości liczą się życie > stal > pola > ropa
     */

    public partial class MainWindow : Window
    {
        public static Grid GameBoard = new Grid();
        public static TextBlock ActionText = new TextBlock();
        public static Grid UserBoard1 = new Grid();
        public static Grid UserBoard2 = new Grid();
        public static Grid UserProperty1 = new Grid();
        public static Grid UserProperty2 = new Grid();



        public const int Column_Row = 16;
        public static int ClickedTimes;
        protected Image back;
         
        public MainBoard[,] VisibleBoard;
        public User1Board[,] BoardU1;
        public User2Board[,] BoardU2;



        public MainWindow()
        {
            InitializeComponent();

            //Adding Background
            ImageBrush FieldBack = new ImageBrush();
            FieldBack.ImageSource = new BitmapImage(new Uri("images/boardproperty/backgrounds/backgroundWulcano.jpeg", UriKind.Relative));
            GameGrid.Background = FieldBack;

            //Drawing Board Schema
            MainScroll.Content = GameBoard;
                        
            for(int i = 0; i < Column_Row; i++)
            {
                GameBoard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41)});
                GameBoard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            }

            //VisibleBoard = new MainBoard[Column_Row, Column_Row];
            //for(int i =0; i < Column_Row; i++)
            // {
            //    for(int j = 0; j < Column_Row; j++)
            //    {
            //       VisibleBoard[i, j] = new MainBoard { X = i, Y = j };
            //    }
            // }

            Layer<MainBoard> fieldLayer = new Layer<MainBoard>(Column_Row, Column_Row, new Dictionary<Type, int>
            {
                { typeof(MainBoard), Column_Row * Column_Row }
            });

            //Adding container for User1
            User1.Content = UserBoard1;
            User1.HorizontalContentAlignment = HorizontalAlignment.Center;

            for (int i = 0; i < 5; i++)
            {
                UserBoard1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
                UserBoard1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            }

            BoardU1 = new User1Board[5, 1];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    BoardU1[i, j] = new User1Board { X = i, Y = j };
                }
            }

            //Adding container for User2
            User2.Content = UserBoard2;
            User2.HorizontalContentAlignment = HorizontalAlignment.Center;
            User2.VerticalContentAlignment = VerticalAlignment.Bottom;
            User2.Background = new SolidColorBrush(Colors.Beige);

            for (int i = 0; i < 5; i++)
            {
                UserBoard2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
                UserBoard2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            }

            BoardU2 = new User2Board[5, 1];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    BoardU2[i, j] = new User2Board { X = i, Y = j };
                }
            }


            //Adding property to board
            
            SProperty1.Content = UserProperty1;
           // UserProperty1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });

            RowDefinition RowGold = new RowDefinition();
            ImageBrush Gold = new ImageBrush();
            Gold.ImageSource = new BitmapImage(new Uri("images/userproperty/gold.png", UriKind.Relative));
            RowGold.Height = new GridLength(40);

            RowDefinition RowIron = new RowDefinition();
            ImageBrush Iron = new ImageBrush();
            Iron.ImageSource = new BitmapImage(new Uri("images/userproperty/iron.png", UriKind.Relative));
            RowIron.Height = new GridLength(40);

            RowDefinition RowLife = new RowDefinition();
            ImageBrush Life = new ImageBrush();
            Life.ImageSource = new BitmapImage(new Uri("images/userproperty/life.png", UriKind.Relative));
            RowLife.Height = new GridLength(40);

            RowDefinition RowOil = new RowDefinition();
            ImageBrush Oil = new ImageBrush();
            Oil.ImageSource = new BitmapImage(new Uri("images/userproperty/oil.png", UriKind.Relative));
            RowOil.Height = new GridLength(40);

            RowDefinition RowFC = new RowDefinition();
            ImageBrush FC = new ImageBrush();
            FC.ImageSource = new BitmapImage(new Uri("images/userproperty/fieldCops.png", UriKind.Relative));
            RowFC.Height = new GridLength(40);

            
           
            UserProperty1.Background = new SolidColorBrush(Colors.Blue);
            UserProperty1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
            UserProperty1.RowDefinitions.Add(RowLife);
            UserProperty1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
            UserProperty1.RowDefinitions.Add(RowGold);
            UserProperty1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
            UserProperty1.RowDefinitions.Add(RowIron);
            UserProperty1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
            UserProperty1.RowDefinitions.Add(RowOil);
            UserProperty1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
            UserProperty1.RowDefinitions.Add(RowFC);

           /* SProperty2.Content = UserProperty2;
            UserProperty2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(41) });
            UserProperty2.RowDefinitions.Add(RowLife);
            UserProperty2.RowDefinitions.Add(RowGold);
            UserProperty2.RowDefinitions.Add(RowIron);
            UserProperty2.RowDefinitions.Add(RowOil);
            UserProperty2.RowDefinitions.Add(RowFC);*/

            //Create Button
            ActionButton.Content = ActionText;
            ClickedTimes = 0;
            ActionButton.Background = new SolidColorBrush(Colors.IndianRed);
            ActionText.Text = "START";







        }

        //Adding action to button
        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedTimes++;
            if (ClickedTimes == 0) 
            {
                ActionButton.Background = new SolidColorBrush(Colors.IndianRed);
                ActionText.Text = "START";
            }
            else if (ClickedTimes % 2 == 0 && ClickedTimes != 0 && ClickedTimes <= 15)
            {
                ActionButton.Background = new SolidColorBrush(Colors.IndianRed);
                ActionText.Text = "TURA PRZECIWNIKA";
            }
            else if (ClickedTimes % 2 != 0 && ClickedTimes <= 15)
            {
                ActionButton.Background = new SolidColorBrush(Colors.DarkRed);
                ActionText.Text = "ZAKOŃCZ TURĘ";
            }
            else
            {
                ActionButton.Background = new SolidColorBrush(Colors.Gray);
                ActionText.Text = "TRWA ROZGRYWKA AUTOMATYCZNA";
                ActionButton.IsEnabled = false;
            }
           
        }
    }
}
