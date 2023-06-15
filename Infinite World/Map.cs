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
        public Map() { }

        public Texture2D GenerateColourMap(float[,] noiseMap, List<Tile> tiles, GraphicsDevice graphics)
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
                            //colours[y * width + x] = tile.Colour;
                        }

                    }
                }
            }

            texture.SetData(colours);
            return texture;
        }

        public RenderTarget2D GenerateTileMap(float[,] heightMap, float[,] heatMap, float[,] moistureMap, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);
            //int tileLength = tiles[0].Length;
            int[,] rectGrid = Jitter.JitterGrid(heightMap);

            //Tile drawTile;
            Feature feature;
            Biome biome;
            RenderTarget2D renderTarget = new RenderTarget2D(graphics, width * 16, height * 16);
            Vector2 tilePosition = new Vector2(0, 0);
            Vector3 values = new Vector3(0, 0, 0);

            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float heightValue = heightMap[x, y];
                    float heatValue = heatMap[x, y];
                    float moistureValue = moistureMap[x, y];

                    biome = GetBiome(biomes, heightValue, heatValue, moistureValue);
                    biome.GetTile(heightValue).Draw(spriteBatch, tilePosition);
                    feature = biome.GetFeature(heightValue);
                    if (feature != null)
                    {
                        feature.Draw(spriteBatch, tilePosition);
                    }

                    tilePosition.Y += 16;
                }
                tilePosition.Y = 0;
                tilePosition.X += 16;
            }
            spriteBatch.End();
            graphics.SetRenderTarget(null);

            return renderTarget;
        }

        public Biome GetBiome(List<Biome> biomes, float heightValue, float heatValue, float moistureValue)
        {
            List<Biome> matches = new List<Biome>();
            Biome returnBiome = biomes[0];
            float matchValue = 10;
            //Debug.WriteLine("x value: " + values.X);
            //Debug.WriteLine("y value: " + values.Y);
            //Debug.WriteLine("z value: " + values.Z);

            foreach (Biome biome in biomes)
            {
                if (heightValue >= biome.MinValues.X && heatValue >= biome.MinValues.Y && moistureValue >= biome.MinValues.Z)
                {
                    matches.Add(biome);
                }
            }

            foreach (Biome match in matches)
            {
                //Debug.WriteLine();
                if (match.GetMatchValue(heightValue, heatValue, moistureValue) < matchValue)
                {
                    //Debug.WriteLine("Biome Match");
                    returnBiome = match;
                    matchValue = match.GetMatchValue(heightValue, heatValue, moistureValue);
                    //Debug.WriteLine(matchValue);
                }
            }

            return returnBiome;
        }
    }
}
