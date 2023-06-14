using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using ScottPlot;
using System.Linq;
using System.Collections.Generic;
using static ScottPlot.Generate;

namespace Infinite_World
{
    public class Game1 : Game
    {
        public enum Screen
        {
            Title,
            Settings,
            Game
        }

        //TITLE SCREEN
        SpriteFont titleFont;
        Texture2D playButtonTexture, settingsButtonTexture;
        Button playButton;
        Button settingsButton;

        // TEXTURES
        Texture2D camera;
        Vector2 mapOffsets;
        Point windowSize;
        Point windowOffset;
        Rectangle windowBounds;
        Rectangle cameraBounds;

        // MAP
        Vector2 mapDimensions;
        Vector2 noiseMapDimensions;
        Vector2 cameraPosition;
        int mapSeed;
        float zoom;
        float[,] heightMap, heatMap, moistureMap;
        List<Biome> biomes = new List<Biome>();
        TerrainChunk titleChunk;
        ChunkLoader chunkLoader;
        List<TerrainChunk> visibleChunks = new List<TerrainChunk> ();
        List<TerrainChunk> lastVisibleChunks = new List<TerrainChunk>();
        int halfBufferWidth;
        int halfBufferHeight;

        // INPUT
        KeyboardState keyboardState, lastKeyboardState = Keyboard.GetState();
        MouseState mouseState;
        float lastScrollValue;
        float scrollValue;
        float elapsedTime;
        float speed;
        Player player;

        // GRAPHICS
        RenderTarget2D renderTarget;

        //MISC
        Random generator = new Random();
        Noise noiseGenerator = new Noise();
        Screen currentScreen;

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
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();

            halfBufferWidth = _graphics.PreferredBackBufferWidth / 2;
            halfBufferHeight = _graphics.PreferredBackBufferHeight / 2;

            windowSize = new Point(1920, 1080); 
            windowOffset = new Point(0, 0);

            windowBounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            cameraBounds = new Rectangle(50, 50, _graphics.PreferredBackBufferWidth - 100, _graphics.PreferredBackBufferHeight - 100);

            chunkLoader = new ChunkLoader();

            currentScreen = Screen.Title;

            noiseMapDimensions = new Vector2(300, 300);

            //Plots for debugging

            //Plot heightPlot = new Plot(300, 300);
            //Plot heatPlot = new Plot(300, 300);
            //Plot moisturePlot = new Plot(300, 300);

            mapDimensions = noiseMapDimensions * 8;
            cameraPosition = new Vector2(600, 600);
            mapOffsets = new Vector2(cameraPosition.X - 2400, cameraPosition.Y - 2400);

            //offsets = new Vector2(0, 0);
            scrollValue = 120;
            lastScrollValue = 0;
            zoom = 1;
            speed = 5;

            mapSeed = generator.Next(0, 10000);

            //Noise Maps for debug

            heightMap = noiseGenerator.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 5.0, 0.06f, 4);
            heatMap = noiseGenerator.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 10.0, 0.04f, 2);
            moistureMap = noiseGenerator.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 10.0, 0.03f, 1);

            //Heatmaps for debug

            //heightPlot.AddHeatmap(FloatToDouble(heightMap));
            //heightPlot.SaveFig("heightMap.png");
            //heatPlot.AddHeatmap(FloatToDouble(heatMap));
            //heatPlot.SaveFig("heatmap.png");
            //moisturePlot.AddHeatmap(FloatToDouble(moistureMap));
            //moisturePlot.SaveFig("moistureMap.png");

            biomes.Add(new Desert(new Vector3(0.3f, 0.6f, 0.0f)));
            biomes.Add(new Grassland(new Vector3(0.3f, 0.3f, 0.5f)));
            biomes.Add(new Ocean(new Vector3(0.0f, 0.0f, 0.0f)));
            biomes.Add(new Jungle(new Vector3(0.3f, 0.7f, 0.9f)));

            base.Initialize();

            foreach(Biome biome in biomes)
            {
                biome.Load(Content);
            }

            player = new Player(camera);

            visibleChunks = chunkLoader.Initialize(_spriteBatch, GraphicsDevice, mapSeed, biomes, 4);
            lastVisibleChunks = visibleChunks;

            // Title Screen
            playButton = new Button("Play", playButtonTexture, new Rectangle(halfBufferWidth - 150, halfBufferHeight - 150, (int)titleFont.MeasureString("Play").X, (int)titleFont.MeasureString("Play").Y), titleFont);
            settingsButton = new Button("Settings", playButtonTexture, new Rectangle(halfBufferWidth - (int)(titleFont.MeasureString("Settings").X / 2), halfBufferHeight, (int)titleFont.MeasureString("Settings").X, (int)titleFont.MeasureString("Settings").Y), titleFont);

            titleChunk = new TerrainChunk(Vector2.Zero, new Vector2(300, 300));
            titleChunk.LoadChunk(mapSeed, GraphicsDevice, _spriteBatch, biomes);
        }

        protected override void LoadContent()
        {
             camera = Content.Load<Texture2D>(@"Camera");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);

            // Title Screen
            titleFont = Content.Load<SpriteFont>(@"Fonts\TitleFont");
            playButtonTexture = Content.Load<Texture2D>("PlayButton");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentScreen)
            {
                case Screen.Title:
                    UpdateTitle();
                    break;

                case Screen.Game:
                    UpdateMain(gameTime);
                    break;

                case Screen.Settings:
                    UpdateSettings();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (currentScreen)
            {
                case Screen.Title:
                    DrawTitle();
                    break;

                case Screen.Game:
                    DrawMain();
                    break;

                case Screen.Settings:
                    DrawSettings();
                    break;
            }

            base.Draw(gameTime);
        }

        // Main game update loop
        public void UpdateMain(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            IsMouseVisible = false;

            elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            player.Update(mouseState);

            UpdateCamera();

            scrollValue = mouseState.ScrollWheelValue;

            if (scrollValue != lastScrollValue)
            {
                zoom = 1 + (scrollValue / 240);
                lastScrollValue = scrollValue;
            }

            //Map.Update(cameraPosition, mapSeed, GraphicsDevice, _spriteBatch, biomes);
            if (chunkLoader.NewChunk(cameraPosition))
            {
                foreach (TerrainChunk chunk in visibleChunks)
                {
                    chunk.Texture.Dispose();
                }

                visibleChunks = chunkLoader.Update(mapSeed, GraphicsDevice, _spriteBatch, biomes, 4, lastVisibleChunks);
                lastVisibleChunks = visibleChunks;
            }
        }

        // Title screen update loop
        public void UpdateTitle()
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            IsMouseVisible = true;

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                titleChunk.Texture.Dispose();
                currentScreen = Screen.Game;
            }

            if (playButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                titleChunk.Texture.Dispose();
                currentScreen = Screen.Game;
            }

            else if (settingsButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                currentScreen = Screen.Settings;
            }
        }

        public void UpdateSettings()
        {
            if (playButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                titleChunk.Texture.Dispose();
                currentScreen = Screen.Game;
            }
        }

        // Main game drawing loop
        public void DrawMain()
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            chunkLoader.DrawMap(_spriteBatch, mapOffsets, visibleChunks);
            player.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        // Title screen drawing loop
        public void DrawTitle()
        {
            GraphicsDevice.Clear(Color.Green);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            titleChunk.DrawChunk(_spriteBatch);
            _spriteBatch.DrawString(titleFont, "Infinite World", new Vector2(halfBufferWidth - (titleFont.MeasureString("Infinite World").X / 2), 100), Color.White);
            playButton.DrawString(_spriteBatch);
            settingsButton.DrawString(_spriteBatch);

            _spriteBatch.End();
        }

        public void DrawSettings()
        {
            GraphicsDevice.Clear(Color.Yellow);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            titleChunk.DrawChunk(_spriteBatch);
            _spriteBatch.DrawString(titleFont, "Settings", new Vector2(halfBufferWidth - (titleFont.MeasureString("Settings").X / 2), 100), Color.White);
            playButton.DrawString(_spriteBatch);

            _spriteBatch.End();
        }

        //Handle arrow key inputs
        public void UpdateCamera()
        {
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.X += speed * elapsedTime;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.X -= speed * elapsedTime;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.Y += speed * elapsedTime;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.Y -= speed * elapsedTime;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                speed = 0.1f;
            }

            mapOffsets = new Vector2(cameraPosition.X - 2400, cameraPosition.Y - 2400);
            //Debug.WriteLine("camerPosition: " + cameraPosition);

            lastKeyboardState = keyboardState;
        }

        //For converting noise maps
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