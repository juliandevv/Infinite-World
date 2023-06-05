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

        private Vector2 _conditions;
        private Color _colour;
        private Texture2D _texture;
        private Random _generator;

        public Tile(Vector2 conditions, Texture2D texture)
        {
            _conditions = conditions;
            _texture = texture;
            _generator = new Random();
            //_biome = biome;
        }

        public bool SatisfyCondition(double noiseValue)
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
            spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y, 16, 16), new Rectangle((_generator.Next(0,4) * 16), 0, 16, 16), Color.White);
        }
      
        //public Color Colour { get { return _colour; } } 

        public Texture2D Texture { get { return _texture; } }

        public int Length { get { return _texture.Width; } }
    }
}
