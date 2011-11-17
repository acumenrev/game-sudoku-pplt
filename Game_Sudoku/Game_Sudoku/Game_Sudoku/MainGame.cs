using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;

namespace Game_Sudoku
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager m_graphics;
        SpriteBatch m_spriteBatch;
        ScreenManager m_screenManager;
        ScreenFactory m_screenFactory;

        public MainGame()
        {

            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_graphics.PreferredBackBufferWidth = 800;
            m_graphics.PreferredBackBufferHeight = 600;

            TargetElapsedTime = TimeSpan.FromTicks(333333);
           
            this.IsMouseVisible = true;


            // Create the screen factory and add it to the Services
            m_screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), m_screenFactory);

            // Create the screen manager component
            m_screenManager = new ScreenManager(this);
            Components.Add(m_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            // Activate the first screens
            m_screenManager.AddScreen(new Screens.BackgroundScreen(), null);

            m_screenManager.AddScreen(new Screens.MainMenuScreen(), null);


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            m_graphics.GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
