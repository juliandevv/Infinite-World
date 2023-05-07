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
        private Color[] _rawData;
        private Color[,] _pixelData;
        private Texture2D _texture;


        public Tile(Vector2 conditions, Color colour, Texture2D texture)
        {
            _conditions = conditions;
            _colour = colour;
            _rawData = new Color[8 * 8];
            _pixelData = new Color[8 , 8];
            _texture = texture;
            texture.GetData<Color>(_rawData);
            
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    _pixelData[i, j] = _rawData[i * j];
                }
            }
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

        public Color GetPixelValue(int x, int y)
        {
            return _pixelData[x, y];
        }

        public Color[,] Raw { get { return _pixelData; } }

        public Color Colour { get { return _colour; } } 

        public int Length { get { return _texture.Width; } }
    }
}
