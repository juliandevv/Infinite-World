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
        private SpriteFont _font;
        private bool _entered;

        public Button(string name, Texture2D texture, Rectangle bounds, SpriteFont font)
        {
            _name = name;
            _texture = texture;
            _bounds = bounds;
            _font = font;
            _entered = false;
        }

        public bool EnterButton(MouseState mouseState)
        {
            int Xpos = mouseState.X;
            int Ypos = mouseState.Y;

            if (Xpos < _bounds.X + _bounds.Width && Xpos > _bounds.X && Ypos < _bounds.Y + _bounds.Height && Ypos > _bounds.Y)
            {
                _entered = true;
                return true;
            }

            _entered = false;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, Color.White);
        }

        public void DrawString(SpriteBatch spriteBatch)
        {
            if (_entered)
            {
                spriteBatch.DrawString(_font, _name, new Vector2(_bounds.X, _bounds.Y), Color.LightGray);

            }
            else
            {
                spriteBatch.DrawString(_font, _name, new Vector2(_bounds.X, _bounds.Y), Color.White);
            }
        }
    }
}
