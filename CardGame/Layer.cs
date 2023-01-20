using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Layer<T> where T : View
    {
        protected View[,] _layer;

        public Layer(int boardWidth, int boardHeight)
        {
            _layer = new View[boardWidth, boardHeight];
        }

        public Layer(int boardWidth, int boardHeight, Dictionary<Type, int> typeCounts) : this(boardWidth, boardHeight)
        {
            int sumOfCounts = typeCounts.Sum(p => p.Value);
            /*if (sumOfCounts > boardWidth * boardHeight)
                throw new TooManyElementsToPutOnLayerException();*/
            for (int i = 0; i < sumOfCounts; i++)
            {
                var firstNonZero = typeCounts.First(p => p.Value > 0);
                typeCounts[firstNonZero.Key]--;
                int x = i % boardHeight;
                int y = i / boardHeight;
                var obj = (T)Activator.CreateInstance(firstNonZero.Key)!;
                _layer[x, y] = obj;
                obj.X = x;
                obj.Y = y;
            }
        }
    }
}
