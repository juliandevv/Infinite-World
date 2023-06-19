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

        public abstract Vector3 MinValues
        {
            get;
        }

        public abstract float GetMatchValue(float heightValue, float heatValue, float moistureValue);

        public abstract void Load(ContentManager content);

        public abstract Tile GetTile(float heightValue);

        public abstract Feature GetFeature(float heightValue);
    }

    class Desert : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private List<Feature> _features = new List<Feature>();
        private Vector3 _minValues = new Vector3();

        public Desert(Vector3 minValues)
        {
            _minValues = minValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 0.35f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.35f, 1.55f), content.Load<Texture2D>(@"Tiles\SandTile1")));
        }

        public override Vector3 MinValues{ get{ return _minValues; } }
       
        public override float GetMatchValue(float heightValue, float heatValue, float moistureValue)
        {
            float matchValue = (heightValue - _minValues.X) + (heatValue - _minValues.Y) + (moistureValue - _minValues.Z);
            return matchValue;
        }

        public override Tile GetTile(float heightValue)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(heightValue))
                {
                    return tile;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)];
        }

        public override Feature GetFeature(float heightValue)
        {
            foreach (Feature feature in _features)
            {
                if (feature.SatisfyCondition(heightValue))
                {
                    return feature;
                }
            }
            return null;
        }
    }

    class Grassland : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<Feature> _features = new List<Feature>();

        public Grassland(Vector3 minValues)
        {
            _minValues = minValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 0.35f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.35f, 0.45f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.45f, 0.65f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.65f, 0.9f), content.Load<Texture2D>(@"Tiles\GrassTile2")));
            _tiles.Add(new Tile(new Vector2(0.9f, 1.5f), content.Load<Texture2D>(@"Tiles\MountainTile1")));

            _features.Add(new Feature(new Vector2(0.4f, 0.9f), 1, content.Load<Texture2D>(@"Features\Tree1")));
        }

        public override Vector3 MinValues { get { return _minValues; } }

        public override float GetMatchValue(float heightValue, float heatValue, float moistureValue)
        {
            float matchValue = (heightValue - _minValues.X) + (heatValue - _minValues.Y) + (moistureValue - _minValues.Z);
            return matchValue;
        }

        public override Tile GetTile(float heightValue)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(heightValue))
                {
                    return tile;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)];
        }

        public override Feature GetFeature(float heightValue)
        {
            foreach (Feature feature in _features)
            {
                if (feature.SatisfyCondition(heightValue))
                {
                    return feature;
                }
            }
            return null;
        }
    }

    class Ocean : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<Feature> _features = new List<Feature>();

        public Ocean(Vector3 minValues)
        {
            _minValues = minValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 0.27f), content.Load<Texture2D>(@"Tiles\DeepWaterTile2")));
            _tiles.Add(new Tile(new Vector2(0.27f, 1.55f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
        }

        public override Vector3 MinValues { get { return _minValues; } }

        public override float GetMatchValue(float heightValue, float heatValue, float moistureValue)
        {
            float matchValue = (heightValue - _minValues.X) + (heatValue - _minValues.Y) + (moistureValue - _minValues.Z);
            return matchValue;
        }

        public override Tile GetTile(float heightValue)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(heightValue))
                {
                    return tile;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)];
        }

        public override Feature GetFeature(float heightValue)
        {
            foreach (Feature feature in _features)
            {
                if (feature.SatisfyCondition(heightValue))
                {
                    return feature;
                }
            }
            return null;
        }
    }

    class Jungle : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<Feature> _features = new List<Feature>();

        public Jungle(Vector3 minValues)
        {
            _minValues = minValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 1.55f), content.Load<Texture2D>(@"Tiles\JungleTile1")));
            //_tiles.Add(new Tile(new Vector2(0.27f, 1.55f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
        }

        public override Vector3 MinValues { get { return _minValues; } }

        public override float GetMatchValue(float heightValue, float heatValue, float moistureValue)
        {
            float matchValue = (heightValue - _minValues.X) + (heatValue - _minValues.Y) + (moistureValue - _minValues.Z);
            return matchValue;
        }

        public override Tile GetTile(float heightValue)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(heightValue))
                {
                    return tile;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)];
        }

        public override Feature GetFeature(float heightValue)
        {
            foreach (Feature feature in _features)
            {
                if (feature.SatisfyCondition(heightValue))
                {
                    return feature;
                }
            }
            return null;
        }
    }
    class Tundra : Biome
    {
        private List<Tile> _tiles = new List<Tile>();
        private Vector3 _minValues = new Vector3();
        private List<Feature> _features = new List<Feature>();

        public Tundra(Vector3 minValues)
        {
            _minValues = minValues;
        }

        public override void Load(ContentManager content)
        {
            _tiles.Add(new Tile(new Vector2(0.0f, 1.55f), content.Load<Texture2D>(@"Tiles\TundraTile")));
            //_tiles.Add(new Tile(new Vector2(0.27f, 1.55f), content.Load<Texture2D>(@"Tiles\ShallowWaterTile2")));
        }

        public override Vector3 MinValues { get { return _minValues; } }

        public override float GetMatchValue(float heightValue, float heatValue, float moistureValue)
        {
            float matchValue = (heightValue - _minValues.X) + (heatValue - _minValues.Y) + (moistureValue - _minValues.Z);
            return matchValue;
        }

        public override Tile GetTile(float heightValue)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.SatisfyCondition(heightValue))
                {
                    return tile;
                }
            }
            return _tiles[_generator.Next(0, _tiles.Count)];
        }

        public override Feature GetFeature(float heightValue)
        {
            foreach (Feature feature in _features)
            {
                if (feature.SatisfyCondition(heightValue))
                {
                    return feature;
                }
            }
            return null;
        }
    }
}
