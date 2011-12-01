
using System.IO.IsolatedStorage;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;


namespace SudokuWinphone
{
    class LoginScreen : GameScreen
    {
        #region fields
        //
        ScreenManager screenManager;
        //SpriteBacth
        SpriteBatch m_spriteBatch;
        //Texture2D
        Texture2D m_Background;
        Texture2D m_Boom;
        SpriteFont m_Font;
        bool m_way = true;
        int m_aphal = 0;

        #endregion

        #region init
        public LoginScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            EnabledGestures = GestureType.Tap;
        }
        // Load Content
        public override void LoadContent()
        {
            m_Background = Load<Texture2D>("BG/Wall");
            m_Boom = Load<Texture2D>("BG/Boom");
            m_Font = Load<SpriteFont>("GameFont");
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

            TransitionAlphaWord();
            foreach (var gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    ExitScreen();
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
            m_spriteBatch.Draw(m_Boom, new Vector2(0, 40), Color.White);
            m_spriteBatch.DrawString(m_Font,"Tap To MainMenu",new Vector2(260,360), new Color(m_aphal, m_aphal, m_aphal));
            m_spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
        public void TransitionAlphaWord()
        {
            
            if (m_way)
            {
                m_aphal = m_aphal + 10;
            }
            else
            {
                m_aphal = m_aphal - 10;
            }
            if (m_aphal > 250)
            {
                m_way = false;
            }
            if (m_aphal==50)
            {
                m_way = true;
            }
        }
    }
}
