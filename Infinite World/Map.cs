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

        public static Texture2D GenerateColourMap(float[,] noiseMap, List<Tile> tiles, GraphicsDevice graphics)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            Texture2D texture = new Texture2D(graphics, width, height);
            Color[] colours = new Color[width * height];

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
                            //tile.Raw.CopyTo(colours, (y * width +x) * 64);
                            colours[y * width + x] = tile.Colour;
                        }
                        
                    }
                }
            }

            texture.SetData(colours);
            return texture;
        }

        public static RenderTarget2D GenerateTileMap(float[,] noiseMap, List<Tile> tiles, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            int tileLength = tiles[0].Length;

            RenderTarget2D renderTarget = new RenderTarget2D(graphics, width * tileLength, height * tileLength);
            Vector2 tilePosition = new Vector2(0, 0);

            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    float noiseValue = noiseMap[x, y];

                    foreach (Tile tile in tiles)
                    {
                        if (tile.SatisfyCondition(noiseValue))
                        {
                            tile.Draw(spriteBatch, tilePosition);
                        }
                    }
                    tilePosition.X = (x) * 8;
                }
                tilePosition.Y = y * 8;
            }
            spriteBatch.End();
            graphics.SetRenderTarget(null);

            return renderTarget;
        }
    }
}
