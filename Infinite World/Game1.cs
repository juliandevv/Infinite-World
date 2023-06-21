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
            Pause,
            Help,
            Game
        }

        //TITLE SCREEN
        SpriteFont titleFont;
        SpriteFont textFont;
        Texture2D playButtonTexture;
        Button playButton;
        Button settingsButton;
        Button helpButton;
        TerrainChunk titleChunk;

        //SETTINGS SCREEN
        Button plusButton, minusButton, backButton;

        //MAIN GAME

        // TEXTURES
        Texture2D camera;
        Vector2 mapOffsets;

        // MAP
        Vector2 mapDimensions;
        Vector2 noiseMapDimensions;
        Vector2 cameraPosition;
        int mapSeed;
        float[,] heightMap, heatMap, moistureMap;
        List<Biome> biomes = new List<Biome>();
        ChunkLoader chunkLoader;
        List<TerrainChunk> visibleChunks = new List<TerrainChunk> ();
        List<TerrainChunk> lastVisibleChunks = new List<TerrainChunk>();
        int halfBufferWidth;
        int halfBufferHeight;
        static float heatModifier;

        // INPUT
        KeyboardState keyboardState, lastKeyboardState = Keyboard.GetState();
        MouseState mouseState;
        MouseState lastMouseState;
        Button menuButton;
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
            
            chunkLoader = new ChunkLoader();

            currentScreen = Screen.Title;

            noiseMapDimensions = new Vector2(300, 300);
            heatModifier = 1f;

            //Plots for debugging

            //Plot heightPlot = new Plot(300, 300);
            //Plot heatPlot = new Plot(300, 300);
            //Plot moisturePlot = new Plot(300, 300);

            mapDimensions = noiseMapDimensions * 8;
            cameraPosition = new Vector2(600, 600);
            mapOffsets = new Vector2(cameraPosition.X - 2400, cameraPosition.Y - 2400);

            //offsets = new Vector2(0, 0);
            speed = 5;

            mapSeed = generator.Next(0, 10000);

            //Noise Maps for debug

            heightMap = noiseGenerator.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 5.0, 0.06f, 4, 1f);
            heatMap = noiseGenerator.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 10.0, 0.04f, 2, heatModifier);
            moistureMap = noiseGenerator.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 10.0, 0.03f, 1, 1f);

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
            //biomes.Add(new Tundra(new Vector3(0.8f, 0.0f, 0.0f)));

            base.Initialize();

            foreach(Biome biome in biomes)
            {
                biome.Load(Content);
            }

            player = new Player(camera);

            menuButton = new Button("Return To Menu", playButtonTexture, new Rectangle(CenterString("Return To Menu", titleFont), 400, (int)titleFont.MeasureString("Return To Menu").X, (int)titleFont.MeasureString("Return To Menu").Y), titleFont);

            visibleChunks = chunkLoader.Initialize(_spriteBatch, GraphicsDevice, mapSeed, biomes, 4);
            lastVisibleChunks = visibleChunks;

            // Title Screen
            playButton = new Button("Play", playButtonTexture, new Rectangle(CenterString("Play", titleFont), 350, (int)titleFont.MeasureString("Play").X, (int)titleFont.MeasureString("Play").Y), titleFont);
            settingsButton = new Button("Settings", playButtonTexture, new Rectangle(CenterString("Settings", titleFont), 500, (int)titleFont.MeasureString("Settings").X, (int)titleFont.MeasureString("Settings").Y), titleFont);
            helpButton = new Button("Help", playButtonTexture, new Rectangle(CenterString("Help", titleFont), 650, (int)titleFont.MeasureString("Help").X, (int)titleFont.MeasureString("Help").Y), titleFont);

            titleChunk = new TerrainChunk(Vector2.Zero, new Vector2(300, 300));
            titleChunk.LoadChunk(mapSeed, GraphicsDevice, _spriteBatch, biomes);

            //Settings Screen
            plusButton = new Button("+", playButtonTexture, new Rectangle(halfBufferWidth + 200, halfBufferHeight - 150, (int)titleFont.MeasureString("+").X, (int)titleFont.MeasureString("+").Y), titleFont);
            minusButton = new Button("-", playButtonTexture, new Rectangle(halfBufferWidth - 220, halfBufferHeight - 150, (int)titleFont.MeasureString("-").X, (int)titleFont.MeasureString("-").Y), titleFont);
            backButton = new Button("Back", playButtonTexture, new Rectangle(CenterString("Back", titleFont), 600, (int)titleFont.MeasureString("Back").X, (int)titleFont.MeasureString("Back").Y), titleFont);

        }

        protected override void LoadContent()
        {
             camera = Content.Load<Texture2D>(@"Camera");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);

            // Title Screen
            titleFont = Content.Load<SpriteFont>(@"Fonts\TitleFont");
            textFont = Content.Load<SpriteFont>(@"Fonts\Text");
            playButtonTexture = Content.Load<Texture2D>("PlayButton");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

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

                case Screen.Pause:
                    UpdatePause();
                        break;

                case Screen.Help:
                    UpdateHelp();
                    break;
            }

            lastMouseState = mouseState;

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

                case Screen.Pause:
                    DrawPause();
                    break;

                case Screen.Help:
                    DrawHelp();
                    break;
            }

            base.Draw(gameTime);
        }

        // Main game update loop
        public void UpdateMain(GameTime gameTime)
        {
            if (IsMouseVisible == true)
            {
                IsMouseVisible = false;
            }

            elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            player.Update(mouseState, cameraPosition);

            UpdateCamera();

            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                currentScreen = Screen.Pause;
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
            if (IsMouseVisible == false)
            {
                IsMouseVisible = true;
            }

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                titleChunk.Texture.Dispose();
                currentScreen = Screen.Game;
            }

            if (playButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Game;
            }

            else if (settingsButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Settings;
            }

            else if (helpButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Help;
            }
        }

        public void UpdateSettings()
        {
            if(IsMouseVisible == false)
            {
                IsMouseVisible = true;
            }

            if (plusButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                heatModifier += 1;
            }
            else if (minusButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                heatModifier -= 1;
            }
            else if (backButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Title;
            }

            if (heatModifier < 1) { heatModifier = 1; }
            else if (heatModifier > 9) { heatModifier = 9; }
        }

        public void UpdatePause()
        {
            if (IsMouseVisible == false)
            {
                IsMouseVisible = true;
            }

            if (backButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Game;
            }

            else if (menuButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Title;
            }
        }

        public void UpdateHelp()
        {
            if (backButton.EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                currentScreen = Screen.Title;
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
            //pauseButton.DrawString(_spriteBatch);

            _spriteBatch.End();
        }

        // Title screen drawing loop
        public void DrawTitle()
        {
            GraphicsDevice.Clear(Color.Green);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            titleChunk.DrawChunk(_spriteBatch, Color.LightGray);
            _spriteBatch.DrawString(titleFont, "Infinite World", new Vector2(halfBufferWidth - (titleFont.MeasureString("Infinite World").X / 2), 100), Color.White);
            playButton.DrawString(_spriteBatch);
            settingsButton.DrawString(_spriteBatch);
            helpButton.DrawString(_spriteBatch);

            _spriteBatch.End();
        }

        public void DrawSettings()
        {
            GraphicsDevice.Clear(Color.Yellow);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            titleChunk.DrawChunk(_spriteBatch, Color.LightGray);
            _spriteBatch.DrawString(titleFont, "Settings", new Vector2(halfBufferWidth - (titleFont.MeasureString("Settings").X / 2), 100), Color.White);
            _spriteBatch.DrawString(titleFont, $"Heat: {heatModifier}", new Vector2(halfBufferWidth - (titleFont.MeasureString($"Heat: {heatModifier}").X / 2), halfBufferHeight - 150), Color.White);
            plusButton.DrawString(_spriteBatch);
            minusButton.DrawString(_spriteBatch);
            backButton.DrawString(_spriteBatch);

            _spriteBatch.End();
        }

        public void DrawPause()
        {
            GraphicsDevice.Clear(Color.Yellow);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            chunkLoader.DrawMap(_spriteBatch, mapOffsets, visibleChunks, Color.LightGray);
            player.Draw(_spriteBatch);
            _spriteBatch.DrawString(titleFont, "Game Paused", new Vector2(halfBufferWidth - (titleFont.MeasureString("Game Paused").X / 2), 100), Color.White);
            backButton.DrawString(_spriteBatch);
            menuButton.DrawString(_spriteBatch);

            _spriteBatch.End();
        }

        public void DrawHelp()
        {
            GraphicsDevice.Clear(Color.Yellow);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            titleChunk.DrawChunk(_spriteBatch, Color.LightGray);
            backButton.DrawString(_spriteBatch);
            _spriteBatch.DrawString(textFont, "Welcome to Infinite world! \nThis is an infinite procedurally generated world for you to explore \nUse arrow keys to move around \nPress TAB at anytime to pause the game", new Vector2(100, 300), Color.White);

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
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.X -= speed * elapsedTime;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.Y += speed * elapsedTime;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                speed += 0.0005f * elapsedTime;
                cameraPosition.Y -= speed * elapsedTime;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                speed = 0.05f;
            }

            if (speed > 0.5) { speed = 0.5f; }

            //mapOffsets = new Vector2(-mouseState.Position.X - 2400, -mouseState.Position.Y - 2400);
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

        public static float Heat { get { return heatModifier; } }

        public int CenterString(string text, SpriteFont font)
        {
            int alignment = (int)(halfBufferWidth - (font.MeasureString(text).X / 2));
            return alignment;
        }
    }
}