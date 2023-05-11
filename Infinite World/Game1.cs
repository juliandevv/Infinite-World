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
        RenderTarget2D tileMap;
        Texture2D grassTile, shallowWaterTile, deepWaterTile, sandTile, mountainTile;
        List<Texture2D> trees = new List<Texture2D>();
        //List<Texture2D> desertTiles = new List<Texture2D>();
        List<Texture2D> grasslandTiles = new List<Texture2D>();

        // MAP
        Vector2 mapDimensions;
        Vector2 noiseMapDimensions;
        Vector2 offsets;
        float zoom;
        float[,] heatMap;
        List<Tile> tiles = new List<Tile>();
        List<Feature> features = new List<Feature>();
        List<Biome> biomes = new List<Biome>();

        // INPUT
        KeyboardState keyboardState;
        MouseState mouseState;
        int lastScrollValue;
        int scrollValue;

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

            noiseMapDimensions = new Vector2(300, 300);
            mapDimensions = noiseMapDimensions * 8;
            offsets = new Vector2((_graphics.PreferredBackBufferWidth - mapDimensions.X) / 2, (_graphics.PreferredBackBufferHeight - mapDimensions.Y) / 2);
            scrollValue = 120;
            lastScrollValue = 0;
            zoom = 1;

            Debug.WriteLine(tiles.Count);

            heatMap = Noise.GenerateNoiseMap(11311, noiseMapDimensions, 15.0f);

            biomes.Add(new Desert(new List<float>() { 0.5f, 0.4f, 0.3f }, new List<float>() { 0.7f, 0.4f, 0.3f }));
            biomes.Add(new Grassland(new List<float>() { 0.0f, 0.4f, 0.3f }, new List<float>() { 0.5f, 0.4f, 0.3f }));

            base.Initialize();

            tiles.Add(new Tile(new Vector2(0.0f, 0.35f), Color.Blue, deepWaterTile));
            tiles.Add(new Tile(new Vector2(0.35f, 0.55f), Color.CornflowerBlue, shallowWaterTile));
            tiles.Add(new Tile(new Vector2(0.55f, 0.65f), Color.Beige, sandTile));
            tiles.Add(new Tile(new Vector2(0.65f, 0.9f), Color.Green, grassTile));
            tiles.Add(new Tile(new Vector2(0.9f, 1.5f), Color.Black, mountainTile));

            foreach(Biome biome in biomes)
            {
                biome.Load(Content);
            }

            features.Add(new Feature(new Vector2(0.65f, 0.9f), Color.Brown, trees));


            tileMap = Map.GenerateTileMap(heatMap, tiles, features, _graphics.GraphicsDevice, _spriteBatch, biomes);
            map = Map.GenerateColourMap(heatMap, tiles, GraphicsDevice);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            grass = Content.Load<Texture2D>("Grass");
            grassTile = Content.Load<Texture2D>(@"Tiles\GrassTile2");
            shallowWaterTile = Content.Load<Texture2D>(@"Tiles\ShallowWaterTile2");
            deepWaterTile = Content.Load<Texture2D>(@"Tiles\DeepWaterTile2");
            //sandTile = Content.Load<Texture2D>(@"Tiles\SandTile1");
            mountainTile = Content.Load<Texture2D>(@"Tiles\MountainTile1");

            trees.Add(Content.Load<Texture2D>(@"Features\Tree1"));
            trees.Add(Content.Load<Texture2D>(@"Features\Tree2"));

            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
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

            
            scrollValue = mouseState.ScrollWheelValue;
            if (scrollValue < 0)
            {
                scrollValue *= -1;
            }

            if (scrollValue != lastScrollValue)
            {
                Debug.WriteLine(lastScrollValue);
                Debug.WriteLine(scrollValue);

                zoom = 1 + (scrollValue/240);
                mapDimensions = noiseMapDimensions * 8;
                mapDimensions *= zoom;
                lastScrollValue = scrollValue;
            }

            
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //renderScale = 1f / (720f / _graphics.GraphicsDevice.Viewport.Height);

            //GraphicsDevice.SetRenderTarget(renderTarget);
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin();

            //_spriteBatch.Draw(map, new Rectangle(offsets.ToPoint(), new Point(800, 800)), Color.White);
            ////_spriteBatch.Draw(map, offsets, Color.White);
            //for (int i = 0; i < tiles.Count; i++)
            //{
            //    tiles[i].Draw(_spriteBatch, new Vector2(i * 10, 50));
            //}

            //_spriteBatch.End();

            //GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(tileMap, new Rectangle(offsets.ToPoint(), mapDimensions.ToPoint()), Color.White);
            _spriteBatch.Draw(map, new Vector2(800, 0), Color.White);


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}