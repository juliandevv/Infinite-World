using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public bool EnterButton(MouseState mouseState)
        {
            int Xpos = mouseState.X;
            int Ypos = mouseState.Y;

            if (Xpos < _bounds.X + _bounds.Width && Xpos > _bounds.X && Ypos < _bounds.Y + _bounds.Height && Ypos > _bounds.Y)
            {
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, Color.White);
        }
    }
}
