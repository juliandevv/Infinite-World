using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Noise
    {
        public static float[,] GenerateNoiseMap(int seed, Vector2 dimensions, Vector2 offsets, float scale, float frequency, int octaves)
        {
            float[,] noiseMap = new float[(int)dimensions.X, (int)dimensions.Y];
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            //dont change these look good
            noise.SetSeed(seed);
            noise.SetFrequency(frequency); //0.05f for height
            noise.SetFractalOctaves(octaves); //4 for height
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.3f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);

            for (int x = 0; x < dimensions.X; x++)
            {
                for (int y = 0; y < dimensions.Y; y++)
                {
                    double sampleX = (x + offsets.X) / 3;
                    double sampleY = (y + offsets.Y) / 3;

                    noiseMap[x,y] = noise.GetNoise((float)sampleX, (float)sampleY);
                }
            }

            return Normalize(noiseMap);
        }

        public static float[,] Normalize(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            float max = noiseMap.Cast<float>().Max();
            //Debug.WriteLine(max);
            float min = noiseMap.Cast<float>().Min();
            //Debug.WriteLine(min);
            float range = max - min;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noiseMap[x, y] = ((noiseMap[x, y] - min) / range);
                }
            }

            return noiseMap;
        }

        public static float[,] Amplify(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            float scaleFactor;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    scaleFactor = Math.Abs(y - (height / 2f)) * (-1f) + (height / 2f);
                    //Debug.WriteLine(scaleFactor);
                    noiseMap[x, y] = noiseMap[x, y] * scaleFactor;
                }
            }
            
            return Normalize(noiseMap);
        }
    }
}
