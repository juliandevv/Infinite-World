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
        private Map mapGenerator;
        private Noise noiseGenerator;

        public TerrainChunk(Vector2 location)
        {
            _address = location;
            _chunkSize = new Vector2(100, 100);
            _texture = null;
        }

        public TerrainChunk(Vector2 location, Vector2 size)
        {
            _address = location;
            _chunkSize = size;
            _texture = null;
        }

        public void LoadChunk(int mapSeed, GraphicsDevice graphics, SpriteBatch spriteBatch, List<Biome> biomes)
        {
            mapGenerator = new Map();
            noiseGenerator = new Noise();

            float[,] heightMap = noiseGenerator.GenerateNoiseMap(mapSeed, _chunkSize, _address * 100, 3.0, 0.06f, 4);
            float[,] heatMap = noiseGenerator.GenerateNoiseMap(mapSeed, _chunkSize, _address * 100, 3.0, 0.04f, 2);
            float[,] moistureMap = noiseGenerator.GenerateNoiseMap(mapSeed, _chunkSize, _address * 100, 3.0, 0.03f, 1);

            _texture = mapGenerator.GenerateTileMap(heightMap, heatMap, moistureMap, graphics, spriteBatch, biomes);
            //Debug.WriteLine("Chunk Size:" + _texture.Bounds);
            //Debug.WriteLine("Chunk Address" + _address);
            //Debug.WriteLine("Chunk Noise Location" + _address * 100);
            //Debug.WriteLine("Chunk Draw Location" + _address * 1600);
        }

        public void DrawChunk(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _address * 1600, Color.White);
        }

        public RenderTarget2D Texture { get { return _texture; } }
        public Vector2 Address { get { return _address; } }
        public Vector2 Size { get { return _chunkSize; } }
    }
}
