using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameStateManagement;

namespace SodukuWinphone
{
    class GameplayScreen:GameScreen
    {
        #region Fields
        //Texture2D
        Texture2D m_gameplayBG;
        //SpriteFont
        SpriteFont m_gameplayFont;
        //SpriteBacth
        SpriteBatch m_spriteBatch;

        #endregion
        #region Init
        //Contructor
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            //LoadConten Texture2D
            m_gameplayBG = Load<Texture2D>("gamescreenBG");
            //LoadConten Font
            m_gameplayFont = Load<SpriteFont>("GameFont");
            base.LoadContent();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
#endregion
#region Update & Draw
        // Update
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        // HandleInput
        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }
        // Draw
        public override void Draw(GameTime gameTime)
        {
            m_spriteBatch = ScreenManager.SpriteBatch;
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_gameplayBG, Vector2.Zero, Color.White);
            m_spriteBatch.End();
            base.Draw(gameTime);
        }
#endregion
    }
}
