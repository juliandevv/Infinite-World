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

        //public abstract float BiomeMatch(Vector3 values);

        public abstract Vector3 MinValues
        {
            get;
        }

        public abstract void Load(ContentManager content);

        public abstract Texture2D GetTile(Vector3 values);
    }

    class Desert : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<float> _maxValues = new List<float>();

        public Desert(Vector3 minValues, List<float> maxValues)
        {
            //_tiles = tiles;
            _minValues = minValues;
            _maxValues = maxValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 0.35f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.35f, 0.55f), content.Load<Texture2D>(@"Tiles\SandTile1")));
            _tiles.Add(new Tile(new Vector2(0.55f, 0.65f), content.Load<Texture2D>(@"Tiles\SandTile1")));
            _tiles.Add(new Tile(new Vector2(0.65f, 0.9f), content.Load<Texture2D>(@"Tiles\SandTile1")));
            _tiles.Add(new Tile(new Vector2(0.9f, 1.5f), content.Load<Texture2D>(@"Tiles\SandTile1")));
            //_tiles.Add(content.Load<Texture2D>(@"Tiles\ShallowWaterTile2"));
        }

        public override Vector3 MinValues{ get{ return _minValues; } }

        //public override float BiomeMatch(Vector3 values)
        //{
        //    if (noiseValue > _minValues[0] && noiseValue < _maxValues[0])
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public override Texture2D GetTile(Vector3 values)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(values.X))
                {
                    return tile.Texture;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)].Texture;
        }
    }

    class Grassland : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<float> _maxValues = new List<float>();

        public Grassland(Vector3 minValues, List<float> maxValues)
        {
            //_tiles = tiles;
            _minValues = minValues;
            _maxValues = maxValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 0.35f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.35f, 0.55f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.55f, 0.65f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.65f, 0.9f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.9f, 1.5f), content.Load<Texture2D>(@"Tiles\MountainTile1")));
            //_tiles.Add(content.Load<Texture2D>(@"Tiles\ShallowWaterTile2"));
        }

        public override Vector3 MinValues { get { return _minValues; } }

        //public override float BiomeMatch(Vector3 values)
        //{
        //    if (noiseValue > _minValues[0] && noiseValue < _maxValues[0])
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public override Texture2D GetTile(Vector3 values)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(values.X))
                {
                    return tile.Texture;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)].Texture;
        }
    }

    class Ocean : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<float> _maxValues = new List<float>();

        public Ocean(Vector3 minValues, List<float> maxValues)
        {
            //_tiles = tiles;
            _minValues = minValues;
            _maxValues = maxValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 0.35f), content.Load<Texture2D>(@"Tiles\DeepWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.35f, 0.55f), content.Load<Texture2D>(@"Tiles\DeepWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.55f, 0.65f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.65f, 0.9f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.9f, 1.5f), content.Load<Texture2D>(@"Tiles\SandTile1")));
            //_tiles.Add(content.Load<Texture2D>(@"Tiles\ShallowWaterTile2"));
        }

        public override Vector3 MinValues { get { return _minValues; } }

        //public override float BiomeMatch(Vector3 values)
        //{
        //    if (noiseValue > _minValues[0] && noiseValue < _maxValues[0])
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public override Texture2D GetTile(Vector3 values)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(values.X))
                {
                    return tile.Texture;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)].Texture;
        }
    }
}
