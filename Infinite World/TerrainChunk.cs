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
    internal class TerrainChunk
    {
        private Vector2 _address = new Vector2();
        private Vector2 _chunkSize = new Vector2();
        private RenderTarget2D _texture;


        public TerrainChunk(Vector2 location)
        {
            _address = location;
            _chunkSize = new Vector2(100, 100);
            _texture = null;
        }

        public void LoadChunk(int mapSeed, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
        {
            float[,] heightMap = Noise.GenerateNoiseMap(mapSeed, _chunkSize, _address * 100, 5.0f, 0.06f, 4);
            float[,] heatMap = Noise.GenerateNoiseMap(mapSeed, _chunkSize, _address * 100, 5.0f, 0.04f, 2);
            float[,] moistureMap = Noise.GenerateNoiseMap(mapSeed, _chunkSize, _address * 100, 5.0f, 0.03f, 1);

            _texture = Map.GenerateTileMap(heightMap, heatMap, moistureMap, graphics, spriteBatch, biomes);
            Debug.WriteLine("Chunk Size:" + _texture.Bounds);
            Debug.WriteLine("Chunk Noise Location" + _address * 100);
            Debug.WriteLine("Chunk Draw Location" + _address * 1600);
        }

        public void DrawChunk(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _address * 1600, Color.White);
        }

        public Vector2 Address { get { return _address; } }
    }
}
