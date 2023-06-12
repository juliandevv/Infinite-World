using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Button
    {
        private string _name;
        private Texture2D _texture;
        private Rectangle _bounds;

        public Button(string name, Texture2D texture, Rectangle bounds)
        {
            _name = name;
            _texture = texture;
            _bounds = bounds;
        }

        public bool EnterButton()
        {
            return true;
        }
    }
}
