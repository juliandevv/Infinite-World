using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Tile
    {
        private Vector2 _conditions;
        private Color _colour;

        public Tile(Vector2 conditions, Color colour)
        {
            _conditions = conditions;
            _colour = colour;
        }

        public bool SatisfyCondition(float noiseValue)
        {
            if (noiseValue >= _conditions.X && noiseValue < _conditions.Y)
            {
                return true;
            }
           
            return false;
            
        }

        public Color Colour { get { return _colour; } } 
    }
}
