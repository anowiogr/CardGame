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

        public const int Column_Row = 16;
        public static int ClickedTimes;

        public MainBoard[,] VisibleBoard;
        public MainBoard[,] BoardU1;
        public MainBoard[,] BoardU2;

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

            VisibleBoard = new MainBoard[Column_Row, Column_Row];
            for(int i =0; i < Column_Row; i++)
            {
                for(int j = 0; j < Column_Row; j++)
                {
                    VisibleBoard[i, j] = new MainBoard { X = i, Y = j };
                }
            }

            //Adding container for User1
            User1.Content = UserBoard1;

            for (int i = 0; i < 5; i++)
            {
                UserBoard1.ColumnDefinitions.Add(new ColumnDefinition());
                UserBoard1.RowDefinitions.Add(new RowDefinition());
            }

            BoardU1 = new MainBoard[5, 1];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    BoardU1[i, j] = new MainBoard { X = i, Y = j };
                }
            }

            
            //Adding Pack schema to game
            ImageBrush Pack = new ImageBrush();
            Pack.ImageSource = new BitmapImage(new Uri("images/boardproperty/rewersCard.png", UriKind.Relative));
            CardPack.Background = Pack;

            //Create Button
            ActionButton.Content = ActionText;
            ClickedTimes = 0;
            ActionButton.Background = new SolidColorBrush(Colors.IndianRed);
            ActionText.Text = "KONIEC TURY";







        }

        //Adding action to button
        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedTimes++;
            if (ClickedTimes % 2 == 0 && ClickedTimes <= 14)
            {
                ActionButton.Background = new SolidColorBrush(Colors.IndianRed);
                ActionText.Text = "ZAKOŃCZ TURĘ";
            }
            else if (ClickedTimes % 2 != 0 && ClickedTimes <= 14)
            {
                 ActionButton.Background = new SolidColorBrush(Colors.DarkRed);
                 ActionText.Text = "TURA PRZECIWNIKA";
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
