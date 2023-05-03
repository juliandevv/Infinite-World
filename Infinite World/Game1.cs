using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using ScottPlot;
using System.Linq;

namespace Infinite_World
{
    public class Game1 : Game
    {
        Texture2D grass;
        Texture2D map;
        Vector2 mapDimensions;
        float[,] heatMap;
        Color[] colours;
        Random generator = new Random();
        int scrollValue;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferHeight = 512;
            _graphics.PreferredBackBufferWidth = 512;
            _graphics.ApplyChanges();

            mapDimensions = new Vector2(512, 512);
            heatMap = Noise.GenerateNoiseMap(11111, mapDimensions, 2.0f);
            colours = Noise.GenerateColourMap(heatMap);

            scrollValue = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            grass = Content.Load<Texture2D>("Grass");
            map = new Texture2D(GraphicsDevice, (int)mapDimensions.X, (int)mapDimensions.Y);
            map.SetData(colours);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Too slow
            //heatMap = Noise.GenerateNoiseMap(11111, mapDimensions, 2.0f);
            //colours = Noise.GenerateColourMap(heatMap);
            //map.SetData(colours);

            if (Mouse.GetState().ScrollWheelValue != scrollValue)
            {
                scrollValue = Mouse.GetState().ScrollWheelValue;
                Debug.WriteLine(scrollValue);
                float scale = Mouse.GetState().ScrollWheelValue / -120;
                if (scale < 0)
                {
                    scale = 1;
                }
                updateMap(10 / scale);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(map, new Rectangle(0, 0, (int)mapDimensions.X, (int)mapDimensions.Y), Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void updateMap(float scale)
        {
            heatMap = Noise.GenerateNoiseMap(11111, mapDimensions, scale);
            colours = Noise.GenerateColourMap(heatMap);
            map.SetData(colours);
        }

    }
}