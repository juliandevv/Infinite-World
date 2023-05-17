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
        private int _abundance;
        private Texture2D _texture;
        private Random _generator;

        public Feature(Vector2 conditions, int abundance, Texture2D texture)
        {
            _conditions = conditions;
            _abundance = abundance;
            _texture = texture;
            _generator = new Random();
        }

        public bool SatisfyCondition(float noiseValue)
        {
            //Debug.WriteLine(noiseValue);
            if (noiseValue >= _conditions.X && noiseValue < _conditions.Y && _generator.Next(0,(int)(10 / _abundance)) == 1)
            {
                //Debug.WriteLine(true);
                return true;
            }

            //Debug.WriteLine(false);
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y, 16, 16), new Rectangle((_generator.Next(0, 4) * 16), 0, 16, 16), Color.White);
        }
    }
}
