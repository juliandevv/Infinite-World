using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Jitter
    {
        private static Random _generator = new Random();

        public static int[,] JitterGrid(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            int[,] rectGrid = new int[width , height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    rectGrid[x, y] = _generator.Next(0, 2);
                }
            }

            //for (int x = 0; x < width; x++)
            //{
            //    for (int y = 0; y < height; y++)
            //    {
            //        if (noiseMap[x,y] > 0.55 && noiseMap[x,y] < 0.9)
            //        {

            //        }
            //    }
            //}

            return rectGrid;
        }
    }
}
