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
        public Noise() { }

        public float[,] GenerateNoiseMap(int seed, Vector2 dimensions, Vector2 offsets, double scale, float frequency, int octaves, float modifier)
        {
            float[,] noiseMap = new float[(int)dimensions.X, (int)dimensions.Y];
            FastNoiseLite noise = new FastNoiseLite();
            Random generator = new Random(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            float offsetX = generator.Next(-100000, 100000) + offsets.X;
            float offsetY = generator.Next(-100000, 100000) + offsets.Y;

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
                    double sampleX = (x + offsetX) / scale;
                    double sampleY = (y + offsetY) / scale;

                    noiseMap[x,y] = noise.GetNoise((float)sampleX, (float)sampleY);
                }
            }

            return Normalize(noiseMap, modifier);
        }

        public double[,] GenerateNoiseMap(int seed, Vector2 dimensions, Vector2 offsets, double scale, double frequency, int octaves)
        {
            double[,] noiseMap = new double[(int)dimensions.X, (int)dimensions.Y];
            FastNoise noise = new FastNoise();
            noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);

            //dont change these look good
            noise.SetSeed(seed);
            noise.SetFrequency(frequency); //0.05f for height
            noise.SetFractalOctaves(octaves); //4 for height
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.3f);
            noise.SetFractalType(FastNoise.FractalType.FBM);

            for (int x = 0; x < dimensions.X; x++)
            {
                for (int y = 0; y < dimensions.Y; y++)
                {
                    double sampleX = (x + (double)offsets.X) / scale;
                    double sampleY = (y + (double)offsets.Y) / scale;

                    noiseMap[x, y] = noise.GetNoise(sampleX, sampleY);
                }
            }

            return Normalize(noiseMap);
        }

        public float[,] Normalize(float[,] noiseMap, float modifier)
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
                    noiseMap[x, y] = ((noiseMap[x, y] - min) / range) * modifier;
                }
            }

            return noiseMap;
        }

        public double[,] Normalize(double[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            double max = noiseMap.Cast<double>().Max();
            //Debug.WriteLine(max);
            double min = noiseMap.Cast<double>().Min();
            //Debug.WriteLine(min);
            double range = max - min;

            for (int x = 0; x < width; x++)
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
