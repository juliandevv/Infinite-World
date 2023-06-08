using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinite_World
{
    internal class Player
    {
        private Texture2D _texture;
        private Vector2 _position;

        public Player(Texture2D texture)
        {
            _texture = texture;
            _position = new Vector2(0, 0);
        }

        public void Update(MouseState mouseState)
        {
            //Vector2 toCursor = new Vector2(_position.X - mouseState.X, _position.Y - mouseState.Y);
            Vector2 cursorPosition = mouseState.Position.ToVector2();
            _position = Vector2.Lerp(_position, cursorPosition, 0.1f);
            //if (_position == mouseState.Position.ToVector2())
            //{
            //    _position = 
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
