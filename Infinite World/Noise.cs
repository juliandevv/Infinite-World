using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Noise
    {
        public static float[,] GenerateNoiseMap(int seed, Vector2 dimensions, float scale)
        {
            float[,] noiseMap = new float[(int)dimensions.X, (int)dimensions.Y];
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            noise.SetFrequency(0.05f);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);

            for (int x = 0; x < dimensions.X; x++)
            {
                for (int y = 0; y < dimensions.Y; y++)
                {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    noiseMap[x,y] = noise.GetNoise(sampleX, sampleY);
                }
            }

            return noiseMap;
        }

        public float[,] Normalize(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            float max = noiseMap.Cast<float>().Max();
            float min = noiseMap.Cast<float>().Min();
            float range = max - min;

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noiseMap[x, y] = noiseMap[x, y] - min / range;
                }
            }

            return noiseMap;
        }

        public static Color[] GenerateColourMap(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            Color[] colours = new Color[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float factor = noiseMap[x, y];
                    //if (factor < 0.5f)
                    //{
                    //    colours[y * width + x] = Color.Lerp(Color.White, Color.Red, factor);
                    //}
                    colours[y * width + x] = Color.Lerp(Color.Black, Color.White, factor);
                }
            }

            return colours;
        }

        private static float LinearStep(float x, float start, float width = 0.2f)
        {
            if (x < start)
                return 0f;
            else if (x > start + width)
                return 1;
            else
                return (x - start) / width;
        }

        private static float GetRedValue(float intensity)
        {
            return LinearStep(intensity, 0.5f);
        }

        private static float GetGreenValue(float intensity)
        {
            return LinearStep(intensity, 0.2f);
        }

        private static float GetBlueValue(float intensity)
        {
            return LinearStep(intensity, 0f)
            - LinearStep(intensity, 0.3f)
            + LinearStep(intensity, 0.7f);
        }

        private static Color getColor(float intensity)
        {
            return new Color(
                (int)(255 * GetRedValue(intensity)),
                (int)(255 * GetGreenValue(intensity)),
                (int)(255 * GetBlueValue(intensity)),
                255
                );
        }
    }
}
