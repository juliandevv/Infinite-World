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
    internal class ChunkLoader
    {
        private List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
        private static Vector2 currentChunkAddress;
        private static Rectangle mapBounds;

        public ChunkLoader() { }

        public List<TerrainChunk> Initialize(SpriteBatch spriteBatch, GraphicsDevice graphics, int mapSeed, List<Biome> biomes, int viewDist)
        {
            List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
            mapBounds = new Rectangle(-1600, -1600, 4800, 4800);
            currentChunkAddress = new Vector2(0, 0);

            for (int i = 0; i < viewDist; i++)
            {
                for (int j = 0; j < viewDist; j++)
                {
                    visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + j, currentChunkAddress.Y + i)));
                }
            }

            foreach (TerrainChunk chunk in visibleChunks)
            {
                chunk.LoadChunk(mapSeed, graphics, spriteBatch, biomes);
            }
            Debug.WriteLine("Chunks loaded");

            return visibleChunks;
        }

        public List<TerrainChunk> Update(int mapSeed, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes, int viewDistance)
        {
            List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
            Vector2 address;

            for (int i = 0; i < viewDistance; i++)
            {
                for (int j = 0; j < viewDistance; j++)
                {
                    //address = new Vector2(j, i);
                    //foreach (TerrainChunk chunk in lastVisibleChunks)
                    //{
                    //    if (chunk.Address != address)
                    //    {
                    //        visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + j, currentChunkAddress.Y + i)));
                    //    }
                    //}

                    visibleChunks.Add(new TerrainChunk(new Vector2(currentChunkAddress.X + j, currentChunkAddress.Y + i)));
                }
            }

            foreach (TerrainChunk chunk in visibleChunks)
            {
                chunk.LoadChunk(mapSeed, graphics, spriteBatch, biomes);
            }
            Debug.WriteLine("Chunks loaded");
           
            return visibleChunks;
        }

        public void DrawMap(SpriteBatch spritebatch, Vector2 mapOffsets, List<TerrainChunk> visibleChunks)
        {
            foreach (TerrainChunk chunk in visibleChunks)
            {
                spritebatch.Draw(chunk.Texture, (chunk.Address * 1600) + mapOffsets, Color.White);
            }
        }

        public bool NewChunk(Vector2 location)
        {
            Vector2 chunkAddress = new Vector2((float)Math.Floor(location.X / -1600), (float)Math.Floor(location.Y / -1600));

            if (currentChunkAddress != chunkAddress)
            {
                currentChunkAddress = chunkAddress;
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
