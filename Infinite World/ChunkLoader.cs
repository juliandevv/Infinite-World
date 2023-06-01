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

        public List<TerrainChunk> Initialize(SpriteBatch spriteBatch, GraphicsDevice graphics, int mapSeed, List<Biome> biomes)
        {
            List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
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

            return visibleChunks;
        }

        public List<TerrainChunk> Update(Vector2 location, int mapSeed, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
        {
            List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
            
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
           
            mapBounds = new Rectangle((int)location.X - 2400, (int)location.Y - 2400, 4800, 4800);

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
