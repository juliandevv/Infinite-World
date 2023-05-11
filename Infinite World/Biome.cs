using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Infinite_World
{
    abstract class Biome
    {
        internal Random _generator;

        public Biome()
        {
            _generator = new Random();
        }

        public abstract bool SatisfyCondition(float noiseValue);

        public abstract void Load(ContentManager content);

        public abstract Texture2D GetTile();
    }

    class Desert : Biome
    {
        private List<Texture2D> _tiles = new List<Texture2D>();
        private List<float> _minValues = new List<float>();
        private List<float> _maxValues = new List<float>();

        public Desert(List<float> minValues, List<float> maxValues)
        {
            //_tiles = tiles;
            _minValues = minValues;
            _maxValues = maxValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(content.Load<Texture2D>(@"Tiles\SandTile1"));
            _tiles.Add(content.Load<Texture2D>(@"Tiles\ShallowWaterTile2"));
        }

        public override bool SatisfyCondition(float noiseValue)
        {
            if (noiseValue > _minValues[0] && noiseValue < _maxValues[0])
            {
                return true;
            }
            return false;
        }

        public override Texture2D GetTile()
        {
            return _tiles[_generator.Next(0, _tiles.Count)];
        }
    }

    class Grassland : Biome
    {
        private List<Texture2D> _tiles = new List<Texture2D>();
        private List<float> _minValues = new List<float>();
        private List<float> _maxValues = new List<float>();

        public Grassland(List<float> minValues, List<float> maxValues)
        {
            _minValues = minValues;
            _maxValues = maxValues;
        }

        public override void Load(ContentManager content)
        {
            //_tiles.Add(content.Load<Texture2D>(@"Tiles\SandTile1"));
            _tiles.Add(content.Load<Texture2D>(@"Tiles\GrassTile2"));
        }

        public override bool SatisfyCondition(float noiseValue)
        {
            if (noiseValue > _minValues[0] && noiseValue < _maxValues[0])
            {
                return true;
            }
            return false;
        }

        public override Texture2D GetTile()
        {
            return _tiles[_generator.Next(0, _tiles.Count)];
        }
    }

    class Ocean : Biome
    {
        private List<Texture2D> _tiles = new List<Texture2D>();
        private List<float> _minValues = new List<float>();
        private List<float> _maxValues = new List<float>();

        public Ocean(List<float> minValues, List<float> maxValues)
        {
            _minValues = minValues;
            _maxValues = maxValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(content.Load<Texture2D>(@"Tiles\ShallowWaterTile2"));
            _tiles.Add(content.Load<Texture2D>(@"Tiles\DeepWaterTile2"));
        }

        public override bool SatisfyCondition(float noiseValue)
        {
            if (noiseValue > _minValues[0] && noiseValue < _maxValues[0])
            {
                return true;
            }
            return false;
        }

        public override Texture2D GetTile()
        {
            return _tiles[_generator.Next(0, _tiles.Count)];
        }
    }
}
