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
        TerrainChunk testChunk;
        ChunkLoader chunkLoader;
        List<TerrainChunk> visibleChunks = new List<TerrainChunk> ();

        // INPUT
        KeyboardState keyboardState, lastKeyboardState = Keyboard.GetState();
        MouseState mouseState;
        float lastScrollValue;
        float scrollValue;
        float elapsedTime;
        float speed;

        // GRAPHICS
        RenderTarget2D renderTarget;

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
            _graphics.PreferredBackBufferHeight = 1200;
            _graphics.PreferredBackBufferWidth = 1200;
            windowSize = new Point(1920, 1080); 
            windowOffset = new Point(0, 0);
            _graphics.ApplyChanges();

            windowBounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            cameraBounds = new Rectangle(50, 50, _graphics.PreferredBackBufferWidth - 100, _graphics.PreferredBackBufferHeight - 100);

            //Chunk Testing
            testChunk = new TerrainChunk(new Vector2(0, 0), new Vector2(200, 100));

            chunkLoader = new ChunkLoader();

            noiseMapDimensions = new Vector2(300, 300);
            Plot heightPlot = new Plot(300, 300);
            Plot heatPlot = new Plot(300, 300);
            Plot moisturePlot = new Plot(300, 300);

            mapDimensions = noiseMapDimensions * 8;
            cameraPosition = new Vector2(600, 600);
            mapOffsets = new Vector2(cameraPosition.X - 2400, cameraPosition.Y - 2400);

            //offsets = new Vector2(0, 0);
            scrollValue = 120;
            lastScrollValue = 0;
            zoom = 1;
            speed = 5;

            mapSeed = generator.Next(0, 10000);

            //heightMap = Noise.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 5.0f, 0.06f, 4);
            //heatMap = Noise.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 5.0f, 0.04f, 2);
            //moistureMap = Noise.GenerateNoiseMap(mapSeed, noiseMapDimensions, Vector2.Zero, 5.0f, 0.03f, 1);

            //heightPlot.AddHeatmap(FloatToDouble(heightMap));
            //heightPlot.SaveFig("heightMap.png");
            //heatPlot.AddHeatmap(FloatToDouble(heatMap));
            //heatPlot.SaveFig("heatmap.png");
            //moisturePlot.AddHeatmap(FloatToDouble(moistureMap));
            //moisturePlot.SaveFig("moistureMap.png");

            //foreach (float value in heatMap)
            //{
            //    Debug.WriteLine(value);
            //}

            biomes.Add(new Desert(new Vector3(0.3f, 0.6f, 0.0f), new List<float>() { 0.7f, 1.5f, 0.3f }));
            biomes.Add(new Grassland(new Vector3(0.3f, 0.3f, 0.5f), new List<float>() { 1.5f, 0.4f, 0.3f }));
            biomes.Add(new Ocean(new Vector3(0.0f, 0.0f, 0.0f), new List<float>() { 0.5f, 0.4f, 0.3f }));


            base.Initialize();

            foreach(Biome biome in biomes)
            {
                biome.Load(Content);
            }

            //Map.Initialize(_spriteBatch, GraphicsDevice, mapSeed, biomes);
            visibleChunks = chunkLoader.Initialize(_spriteBatch, GraphicsDevice, mapSeed, biomes);

            testChunk.LoadChunk(mapSeed, GraphicsDevice, _spriteBatch, biomes);
            //tileMap = Map.GenerateTileMap(heightMap, heatMap, moistureMap, _graphics.GraphicsDevice, _spriteBatch, biomes);
        }

        protected override void LoadContent()
        {
            camera = Content.Load<Texture2D>(@"Camera");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            UpdateCamera();

            scrollValue = mouseState.ScrollWheelValue;
             
            if (scrollValue != lastScrollValue)
            {
                zoom = 1 + (scrollValue/240);
                lastScrollValue = scrollValue;
            }

            //Map.Update(cameraPosition, mapSeed, GraphicsDevice, _spriteBatch, biomes);
            if (chunkLoader.NewChunk(cameraPosition))
            {
                foreach(TerrainChunk chunk in visibleChunks)
                {
                    chunk.Texture.Dispose();
                }

                visibleChunks.Clear();
                visibleChunks = chunkLoader.Update(cameraPosition, mapSeed, GraphicsDevice, _spriteBatch, biomes);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(renderTarget);
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin();

            //_spriteBatch.Draw(tileMap, new Rectangle(offsets.ToPoint(), mapDimensions.ToPoint()), Color.White);

            //_spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //_spriteBatch.Draw(renderTarget, new Rectangle(windowOffset, windowSize), Color.White);
            //_spriteBatch.Draw(Map.Texture, Map.Bounds, Color.White);
            //Map.DrawMap(_spriteBatch, mapOffsets);
            chunkLoader.DrawMap(_spriteBatch, mapOffsets, visibleChunks);
            _spriteBatch.Draw(camera, new Rectangle((int)cameraPosition.X, (int)cameraPosition.Y, 48, 48), Color.Black);
            //_spriteBatch.Draw(testChunk.Texture, new Rectangle(0, 400, 800, 400), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

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

            //if (!cameraBounds.Contains(cameraPosition))
            //{
            //    mapOffsets = -cameraPosition + new Vector2(-1600, -1600);
            //    //Debug.WriteLine(mapOffsets);
            //}

            lastKeyboardState = keyboardState;
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