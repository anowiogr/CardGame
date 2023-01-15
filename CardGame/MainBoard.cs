using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace CardGame
{ 
    public class MainBoard : View
    {
       
        public override string imagePath => $"pack://application:,,,/images/boardproperty/fieldback.png";
       
    }
}
