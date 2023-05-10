using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Tile
    {
        public enum Biomes
        {
            Desert,
            Grassland
        }

        private Vector2 _conditions;
        private Color _colour;
        private Texture2D _texture;
        private Biomes _biome;

        public Tile(Vector2 conditions, Color colour, Texture2D texture, Biomes biome)
        {
            _conditions = conditions;
            _colour = colour;
            _texture = texture;
            _biome = biome;
        }

        public bool SatisfyCondition(float noiseValue)
        {
            //Debug.WriteLine(noiseValue);
            if (noiseValue >= _conditions.X && noiseValue < _conditions.Y)
            {
                //Debug.WriteLine(true);
                return true;
            }

            //Debug.WriteLine(false);
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture, position, Color.White);
        }
      
        public Color Colour { get { return _colour; } } 

        public Biomes Biome { get { return _biome; } }

        public int Length { get { return _texture.Width; } }
    }
}
