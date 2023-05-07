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
        public static float[,] GenerateNoiseMap(int seed, Vector2 dimensions, float scale)
        {
            Vector2 offsets = new Vector2(0, 0);
            float[,] noiseMap = new float[(int)dimensions.X, (int)dimensions.Y];
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            //dont change these look good
            noise.SetFrequency(0.05f);
            noise.SetFractalOctaves(4);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.3f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);

            for (int x = 0; x < dimensions.X; x++)
            {
                for (int y = 0; y < dimensions.Y; y++)
                {
                    float sampleX = (x / scale) + offsets.X;
                    float sampleY = (y / scale) + offsets.Y;

                    noiseMap[x,y] = noise.GetNoise(sampleX, sampleY);
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

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noiseMap[x, y] = ((noiseMap[x, y] - min) / range);
                }
            }

            return noiseMap;
        }
    }
}
