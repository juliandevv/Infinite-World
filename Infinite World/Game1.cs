using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using ScottPlot;
using System.Linq;
using System.Collections.Generic;

namespace Infinite_World
{
    public class Game1 : Game
    {
        Texture2D grass;
        Texture2D map;
        Vector2 mapDimensions;
        Vector2 offsets;
        float[,] heatMap;
        Color[] colours;
        List<Tile> tiles = new List<Tile>();
        Random generator = new Random();
        int scrollValue;
        KeyboardState keyboardState;

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

            tiles.Add(new Tile(new Vector2(0.0f, 0.35f), Color.Blue));
            tiles.Add(new Tile(new Vector2(0.35f, 0.55f), Color.CornflowerBlue));
            tiles.Add(new Tile(new Vector2(0.55f, 0.65f), Color.Beige));
            tiles.Add(new Tile(new Vector2(0.65f, 0.9f), Color.Green));
            tiles.Add(new Tile(new Vector2(0.9f, 1.5f), Color.Black));

            offsets = new Vector2(30, 20);

            Debug.WriteLine(tiles.Count);

            mapDimensions = new Vector2(512, 512);
            heatMap = Noise.GenerateNoiseMap(11311, mapDimensions, 10.0f, offsets);

            scrollValue = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            grass = Content.Load<Texture2D>("Grass");
            //map = new Texture2D(GraphicsDevice, (int)mapDimensions.X, (int)mapDimensions.Y);
            //map.SetData(colours);

            map = Map.GenerateTileMap(heatMap, tiles, _graphics.GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                offsets.X += 1;
                updateMap(10, offsets);
            }

            //if (Mouse.GetState().ScrollWheelValue != scrollValue)
            //{
            //    scrollValue = Mouse.GetState().ScrollWheelValue;
            //    Debug.WriteLine(scrollValue);
            //    float scale = Mouse.GetState().ScrollWheelValue / -120;
            //    if (scale < 0)
            //    {
            //        scale = 1;
            //    }
            //    updateMap(10 / scale);
            //}

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

        public void updateMap(float scale, Vector2 offsets)
        {
            heatMap = Noise.GenerateNoiseMap(11111, mapDimensions, scale, offsets);
            map = Map.GenerateTileMap(heatMap, tiles, _graphics.GraphicsDevice);
        }

    }
}