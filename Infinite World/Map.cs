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

        public static RenderTarget2D GenerateTileMap(float[,] noiseMap, List<Tile> tiles, List<Feature> features, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            int tileLength = tiles[0].Length;
            int[,] rectGrid = Jitter.JitterGrid(noiseMap);

            Texture2D drawTile;
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

                    drawTile = GetBiome(biomes, noiseValue).GetTile();
                    spriteBatch.Draw(drawTile, tilePosition, Color.White);

                    //foreach (Tile tile in tiles)
                    //{
                    //    if (tile.SatisfyCondition(noiseValue))
                    //    {
                    //        tile.Draw(spriteBatch, tilePosition);
                    //    }
                    //}
                    //foreach(Feature feature in features)
                    //{
                    //    if (rectGrid[x + (int)noiseValue, y] == 0 && feature.SatisfyCondition(noiseValue))
                    //    {
                    //        feature.Draw(spriteBatch, tilePosition);
                    //    }
                    //}

                    tilePosition.X = (x) * 8;
                }
                tilePosition.Y = y * 8;
            }
            spriteBatch.End();
            graphics.SetRenderTarget(null);

            return renderTarget;
        }

        public static Biome GetBiome(List<Biome> biomes, float noiseValue)
        {
            foreach(Biome biome in biomes)
            {
                if (biome.SatisfyCondition(noiseValue))
                {
                    return biome;
                }
            }
            return biomes[0];
        }
    }
}
