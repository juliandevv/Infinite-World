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
        float[] heatMap;
        Random generator = new Random();
        Plot plt = new ScottPlot.Plot(512, 512);
        float[] xAxis, yAxis;

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


            plt.AddColorbar(ScottPlot.Drawing.Colormap.Viridis);

            heatMap = generateNoise(generator.Next(0, 99999), new Vector2(64, 64));

            float range = heatMap.Max() - heatMap.Min();
            for (int i = 0; i < heatMap.Length; i++)
            {
                heatMap[i] = (heatMap[i] - heatMap.Min()) / range;
            }

            //int index = 0;
            //for (int y = 0; y < 32; y++)
            //{
            //    for (int x = 0; x < 32; x++)
            //    {
            //        System.Drawing.Color c = ScottPlot.Drawing.Colormap.Viridis.GetColor(heatMap[index++]);
            //        plt.AddPoint(x, y, c);
            //    }
            //}

            //plt.SaveFig("HeatMap.png");
            //Debug.WriteLine(heatMap.Length);

            //foreach(float value in heatMap)
            //{
            //    Debug.WriteLine(value);
            //}
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            grass = Content.Load<Texture2D>("Grass");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            int index = 0;
            for(int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (heatMap[index++] <= 0.4)
                    {
                        _spriteBatch.Draw(grass, new Rectangle(i*8, j*8, 8, 8), Color.White);
                    }
                    else if (heatMap[index] <= 0.6)
                    {
                        _spriteBatch.Draw(grass, new Rectangle(i * 8, j * 8, 8, 8), Color.SandyBrown);
                    }
                    //else if (heatMap[index] <= 1)
                    //{
                    //    _spriteBatch.Draw(grass, new Rectangle(i * 8, j * 8, 8, 8), Color.LightGoldenrodYellow);
                    //}
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public float[] generateNoise(int seed, Vector2 dimensions)
        {
            FastNoiseLite noise = new FastNoiseLite(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            noise.SetFrequency(0.05f);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            //noise.SetFractalOctaves(8);

            float[] heightMap = new float[(int)dimensions.X * (int)dimensions.Y];

            int index = 0;
            for(int x = 0; x < dimensions.X; x++)
            {
                for(int y = 0; y < dimensions.Y; y++)
                {
                    heightMap[index++] = noise.GetNoise(x, y);
                }
            }

            return heightMap;

        }
    }
}