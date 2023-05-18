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
        Point windowSize;
        Point windowOffset;

        // MAP
        Vector2 mapDimensions;
        Vector2 noiseMapDimensions;
        Vector2 offsets;
        float zoom;
        float[,] heightMap, heatMap, moistureMap;
        List<Tile> tiles = new List<Tile>();
        List<Feature> features = new List<Feature>();
        List<Biome> biomes = new List<Biome>();

        // INPUT
        KeyboardState keyboardState, lastKeyboardState = Keyboard.GetState();
        MouseState mouseState;
        float lastScrollValue;
        float scrollValue;
        double keyDownTime;
        double keyPressTime;
        float elapsedTime;
        float speed;
        double acceleration;

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
            windowSize = new Point(1920, 1080);
            windowOffset = new Point(0, 0);
            _graphics.ApplyChanges();

            noiseMapDimensions = new Vector2(300, 300);
            Plot heightPlot = new Plot(300, 300);
            Plot heatPlot = new Plot(300, 300);
            Plot moisturePlot = new Plot(300, 300);

            mapDimensions = noiseMapDimensions * 8;
            offsets = new Vector2((_graphics.PreferredBackBufferWidth - mapDimensions.X) / 2, (_graphics.PreferredBackBufferHeight - mapDimensions.Y) / 2);
            scrollValue = 120;
            lastScrollValue = 0;
            zoom = 1;

            Debug.WriteLine(tiles.Count);

            heightMap = Noise.GenerateNoiseMap(generator.Next(0, 10000), noiseMapDimensions, 5.0f, 0.05f, 4);
            heatMap = Noise.GenerateNoiseMap(generator.Next(0, 10000), noiseMapDimensions, 5.0f, 0.04f, 2);
            //heatMap = Noise.Amplify(heatMap);
            moistureMap = Noise.GenerateNoiseMap(generator.Next(0, 10000), noiseMapDimensions, 5.0f, 0.03f, 1);

            heightPlot.AddHeatmap(FloatToDouble(heightMap));
            heightPlot.SaveFig("heightMap.png");
            heatPlot.AddHeatmap(FloatToDouble(heatMap));
            heatPlot.SaveFig("heatmap.png");
            moisturePlot.AddHeatmap(FloatToDouble(moistureMap));
            moisturePlot.SaveFig("moistureMap.png");

            //foreach (float value in heatMap)
            //{
            //    Debug.WriteLine(value);
            //}

            biomes.Add(new Desert(new Vector3(0.3f, 0.6f, 0.0f), new List<float>() { 0.7f, 1.5f, 0.3f }));
            biomes.Add(new Grassland(new Vector3(0.3f, 0.3f, 0.5f), new List<float>() { 1.5f, 0.4f, 0.3f }));
            biomes.Add(new Ocean(new Vector3(0.0f, 0.0f, 0.0f), new List<float>() { 0.5f, 0.4f, 0.3f }));

            base.Initialize();

            tiles.Add(new Tile(new Vector2(0.0f, 0.35f), deepWaterTile));
            tiles.Add(new Tile(new Vector2(0.35f, 0.55f), shallowWaterTile));
            tiles.Add(new Tile(new Vector2(0.55f, 0.65f), sandTile));
            tiles.Add(new Tile(new Vector2(0.65f, 0.9f), grassTile));
            tiles.Add(new Tile(new Vector2(0.9f, 1.5f), mountainTile));

            foreach(Biome biome in biomes)
            {
                biome.Load(Content);
            }

            tileMap = Map.GenerateTileMap(heightMap, heatMap, moistureMap, _graphics.GraphicsDevice, _spriteBatch, biomes);
            //map = Map.GenerateColourMap(heightMap, tiles, GraphicsDevice);

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

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                speed += 0.001f * elapsedTime;
                offsets.X += speed * elapsedTime;

                //offsets.X += 5;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                speed += 0.001f * elapsedTime;
                offsets.X -= speed * elapsedTime;
                //offsets.X -= 5; 
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                speed += 0.001f * elapsedTime;
                offsets.Y += speed * elapsedTime;
                //offsets.Y += 5;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                speed += 0.001f * elapsedTime;
                offsets.Y -= speed * elapsedTime;
                //offsets.Y -= 5;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                speed = 0.1f;
            }

            lastKeyboardState = keyboardState;
            
            scrollValue = mouseState.ScrollWheelValue;
            //if (scrollValue < 0)
            //{
            //    scrollValue *= -1;
            //}

            if (scrollValue != lastScrollValue)
            {
                Debug.WriteLine(lastScrollValue);
                Debug.WriteLine(scrollValue);

                zoom = 1 + (scrollValue/240);
                lastScrollValue = scrollValue;
            }

            windowSize = new Point((int)(zoom * 1920), (int)(zoom * 1080));
            windowOffset = new Point(-(windowSize.X - 1920) / 2, -(windowSize.Y - 1080) / 2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(tileMap, new Rectangle(offsets.ToPoint(), mapDimensions.ToPoint()), Color.White);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(renderTarget, new Rectangle(windowOffset, windowSize), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public double[,] FloatToDouble(float[,] floatArray)
        {
            int width = floatArray.GetLength(0);
            int height = floatArray.GetLength(1);

            double[,] doubleArray = new double[width, height];

            for (int i = 0; i < floatArray.GetLength(0); i++)
            {
                for (int j = 0; j < floatArray.GetLength(1); j++)
                {
                    doubleArray[i, j] = floatArray[i, j];
                }
            }
            return doubleArray;
        }
    }
}