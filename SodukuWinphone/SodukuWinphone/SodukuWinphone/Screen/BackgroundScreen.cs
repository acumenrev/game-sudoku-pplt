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


namespace SudokuWinphone
{
    class BackgroundScreen:GameScreen
    {
        #region fields
        //
        ScreenManager screenManager;
        //SpriteBacth
        SpriteBatch m_spriteBatch;
        //Texture2D
        Texture2D m_Background;
        #endregion

        #region init
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            EnabledGestures = GestureType.Tap;
        }
        // Load Content
        public override void LoadContent()
        {
            m_Background = Load<Texture2D>("Background");    
            base.LoadContent();
        }
        public override void UnloadContent()
        {
            
            base.UnloadContent();
        }
        #endregion
        #region Handleinput & Draw
        //
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            foreach(var gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new GameplayScreen());
                }
            }
            base.HandleInput(input);
        }

        // Draw
        public override void Draw(GameTime gameTime)
        {
            m_spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullScreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_Background, fullScreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            m_spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

    }
}
