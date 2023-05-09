using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Feature
    {
        private Vector2 _conditions;
        private Color _colour;
        private List<Texture2D> _textures;
        private Random _generator;

        public Feature(Vector2 conditions, Color colour, List<Texture2D> textures)
        {
            _conditions = conditions;
            _colour = colour;
            _textures = textures;
            _generator = new Random();
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
            Texture2D texture = _textures[_generator.Next(0, _textures.Count)];
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
