using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Infinite_World
{
    internal class Map
    {
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
                    colours[y * width + x] = Color.Lerp(Color.White, Color.Black, factor);
                }
            }

            return colours;
        }

        public static Texture2D GenerateTileMap(float[,] noiseMap, List<Tile> tiles, GraphicsDevice graphics)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            Texture2D texture = new Texture2D(graphics, width * 8, height * 8);
            Color[] colours = new Color[width * height * 64];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noiseValue = noiseMap[x, y];

                    foreach (Tile tile in tiles)
                    {
                        //Debug.WriteLine(noiseValue);

                        if (tile.SatisfyCondition(noiseValue))
                        {
                            //Debug.WriteLine(true);
                            //Debug.WriteLine(tile.Colour);
                            tile.Raw.CopyTo(colours, (y * width +x) * 64);
                        }
                        
                    }
                }
            }

            texture.SetData(colours);
            return texture;
        }
    }
}
