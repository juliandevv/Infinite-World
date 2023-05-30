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
        private static List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
        private static Vector2 currentChunkAddress;
        private static RenderTarget2D mapTexture;
        private static Rectangle mapBounds;

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
                            //colours[y * width + x] = tile.Colour;
                        }
                        
                    }
                }
            }

            texture.SetData(colours);
            return texture;
        }

        public static RenderTarget2D GenerateTileMap(float[,] heightMap, float[,] heatMap, float[,] moistureMap, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
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
                    values.X = heightMap[x, y];
                    values.Y = heatMap[x, y];
                    values.Z = moistureMap[x, y];

                    biome = GetBiome(biomes, values);
                    biome.GetTile(values).Draw(spriteBatch, tilePosition);
                    feature = biome.GetFeature(values);
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

        public static Biome GetBiome(List<Biome> biomes, Vector3 values)
        {
            List<Biome> matches = new List<Biome>();
            Biome returnBiome = biomes[0];
            float matchValue = 10;
            //Debug.WriteLine("x value: " + values.X);
            //Debug.WriteLine("y value: " + values.Y);
            //Debug.WriteLine("z value: " + values.Z);

            foreach (Biome biome in biomes)
            {
                if (values.X >= biome.MinValues.X && values.Y >= biome.MinValues.Y && values.Z >= biome.MinValues.Z)
                {
                    matches.Add(biome);
                }
            }

            foreach (Biome match in matches)
            {
                //Debug.WriteLine();
                if(match.GetMatchValue(values) < matchValue)
                {
                    //Debug.WriteLine("Biome Match");
                    returnBiome = match;
                    matchValue = match.GetMatchValue(values);
                    //Debug.WriteLine(matchValue);
                }
            }

            return returnBiome;
        }

        public static void Initialize(SpriteBatch spriteBatch, GraphicsDevice graphics, int mapSeed, List<Biome> biomes)
        {
            mapBounds = new Rectangle(-1600, -1600, 4800, 4800);
            currentChunkAddress = new Vector2(0, 0);

            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X, currentChunkAddress.Y)));
            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 1, currentChunkAddress.Y)));
            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 2, currentChunkAddress.Y)));

            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X, currentChunkAddress.Y + 1)));
            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 1, currentChunkAddress.Y + 1)));
            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 2, currentChunkAddress.Y + 1)));

            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X, currentChunkAddress.Y + 2)));
            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 1, currentChunkAddress.Y + 2)));
            visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 2, currentChunkAddress.Y + 2)));

            foreach (TerrainChunk chunk in visibleChunks)
            {
                chunk.LoadChunk(mapSeed, graphics, spriteBatch, biomes);
            }
            Debug.WriteLine("Chunks loaded");

            UpdateTexture(spriteBatch, graphics);
        }

        public static bool Update(Vector2 location, int mapSeed, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
        {
            Vector2 chunkAddress = new Vector2((float)Math.Floor(location.X / -1600), (float)Math.Floor(location.Y / -1600));
            if (currentChunkAddress != chunkAddress)
            {
                Debug.WriteLine("Entered New Chunk");
                Debug.WriteLine(chunkAddress);

                //mapBounds.X -= 1600;
                //mapBounds.Y -= 1600;

                currentChunkAddress = chunkAddress;
                visibleChunks.Clear();

                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X, currentChunkAddress.Y)));
                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 1, currentChunkAddress.Y)));
                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 2, currentChunkAddress.Y)));

                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X, currentChunkAddress.Y + 1)));
                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 1, currentChunkAddress.Y + 1)));
                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 2, currentChunkAddress.Y + 1)));

                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X, currentChunkAddress.Y + 2)));
                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 1, currentChunkAddress.Y + 2)));
                visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + 2, currentChunkAddress.Y + 2)));

                foreach (TerrainChunk chunk in visibleChunks)
                {
                    chunk.LoadChunk(mapSeed, graphics, spriteBatch, biomes);
                }
                Debug.WriteLine("Chunks loaded");

                //UpdateTexture(spriteBatch, graphics);
                return true;
            }

            else
            {
                mapBounds = new Rectangle((int)location.X - 2400, (int)location.Y - 2400, 4800, 4800);
                return false;
            }
        }

        public static void DrawMap(SpriteBatch spritebatch, Vector2 mapOffsets)
        {
            foreach (TerrainChunk chunk in visibleChunks)
            {
                //chunk.DrawChunk(spritebatch);
                spritebatch.Draw(chunk.Texture, (chunk.Address * 1600) + mapOffsets, Color.White);
            }
        }

        public static void UpdateTexture(SpriteBatch spritebatch, GraphicsDevice graphics)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(graphics, 4800, 4800);

            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(Color.Black);
            spritebatch.Begin();
            foreach (TerrainChunk chunk in visibleChunks)
            {
                //chunk.DrawChunk(spritebatch);
                spritebatch.Draw(chunk.Texture, (chunk.Address - currentChunkAddress) * 1600, Color.White);
            }
            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 3; j++)
            //    {
            //        spritebatch.Draw(visibleChunks[i].Texture, new Vector2(i * 1600, j * 1600), Color.White);
            //    }
            //}

            spritebatch.End();
            graphics.SetRenderTarget(null);

            mapTexture = renderTarget;
        }

        public static RenderTarget2D Texture { get { return mapTexture; } }

        public static Rectangle Bounds { get { return mapBounds; } }
    }
}
