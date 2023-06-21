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
        private float _rotation;

        public Player(Texture2D texture)
        {
            _texture = texture;
            _position = new Vector2(0, 0);
        }

        public void Update(MouseState mouseState, Vector2 cameraPosition)
        {
            //Vector2 toCursor = new Vector2(_position.X - mouseState.X, _position.Y - mouseState.Y);
            Vector2 cursorPosition = mouseState.Position.ToVector2();
            //Vector2 cursorPosition = new Vector2(500, 640);
            //Debug.WriteLine(cursorPosition.ToString());
            _position = Vector2.Lerp(_position, cursorPosition, 0.01f);
            Vector2 deltaPosition = _position - cursorPosition;
            _rotation = (float)Math.Atan2(deltaPosition.Y, deltaPosition.X) - (float)(Math.PI / 2);
            //if (_position == mouseState.Position.ToVector2())
            //{
            //    _position = 
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, new Rectangle(_position.ToPoint(), new Point(_texture.Width, _texture.Height)), new Rectangle(_position.ToPoint(), new Point(_texture.Width, _texture.Height)), Color.White, _rotation, _position, SpriteEffects.None, 0);
            spriteBatch.Draw(_texture, new Rectangle(_position.ToPoint(), new Point(_texture.Width , _texture.Height)), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White, _rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 0f);
            //spriteBatch.Draw(_texture, new Rectangle(_position.ToPoint(), new Point(_texture.Width, _texture.Height)), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White, _rotation, new Vector2(0, 0), SpriteEffects.None, 0f);

        }
    }
}
