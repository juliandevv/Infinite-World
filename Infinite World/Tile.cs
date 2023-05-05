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


        public Tile(Vector2 conditions, Color colour, Texture2D texture)
        {
            _conditions = conditions;
            _colour = colour;
            _rawData = new Color[8 * 8];
            texture.GetData<Color>(_rawData);
        }

        public bool SatisfyCondition(float noiseValue)
        {
            Debug.WriteLine(noiseValue);
            if (noiseValue >= _conditions.X && noiseValue < _conditions.Y)
            {
                Debug.WriteLine(true);
                return true;
            }

            Debug.WriteLine(false);
            return false;
        }

        public Color[] Raw { get { return _rawData; } }

        public Color Colour { get { return _colour; } } 
    }
}
