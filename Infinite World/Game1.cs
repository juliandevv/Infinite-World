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
        // TEXTURES
        Texture2D grass;
        Texture2D map;

        // MAP
        Vector2 mapDimensions;
        Vector2 offsets;
        float[,] heatMap;
        List<Tile> tiles = new List<Tile>();

        // INPUT
        KeyboardState keyboardState;

        // GRAPHICS
        RenderTarget2D renderTarget;
        float renderScale = 0.44444f;

        //MISC
        Random generator = new Random();

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
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();

            tiles.Add(new Tile(new Vector2(0.0f, 0.35f), Color.Blue));
            tiles.Add(new Tile(new Vector2(0.35f, 0.55f), Color.CornflowerBlue));
            tiles.Add(new Tile(new Vector2(0.55f, 0.65f), Color.Beige));
            tiles.Add(new Tile(new Vector2(0.65f, 0.9f), Color.Green));
            tiles.Add(new Tile(new Vector2(0.9f, 1.5f), Color.Black));

            mapDimensions = new Vector2(2500, 2500);
            offsets = new Vector2((_graphics.PreferredBackBufferWidth - mapDimensions.X) / 2, (_graphics.PreferredBackBufferHeight - mapDimensions.Y) / 2);

            Debug.WriteLine(tiles.Count);

            heatMap = Noise.GenerateNoiseMap(11311, mapDimensions, 10.0f);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            grass = Content.Load<Texture2D>("Grass");
            //map = new Texture2D(GraphicsDevice, (int)mapDimensions.X, (int)mapDimensions.Y);
            //map.SetData(colours);

            map = Map.GenerateTileMap(heatMap, tiles, _graphics.GraphicsDevice);

            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.Left))
            {
                offsets.X += 5;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                offsets.X -= 5; 
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                offsets.Y += 5;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                offsets.Y -= 5;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            renderScale = 1f / (720f / _graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //_spriteBatch.Draw(map, new Rectangle(offsets.ToPoint(), mapDimensions.ToPoint()), Color.White);
            _spriteBatch.Draw(map, offsets, Color.White);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void updateMap(float scale, Vector2 offsets)
        {
            //heatMap = Noise.GenerateNoiseMap(11111, mapDimensions, scale, offsets);
            //map = Map.GenerateTileMap(heatMap, tiles, _graphics.GraphicsDevice);
        }

    }
}