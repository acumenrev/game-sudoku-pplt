using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;

namespace Game_Sudoku.Screens
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields
        ContentManager content;
        Texture2D m_backgroundTexture;
        SoundEffect m_buttonSound;
        KeyboardState m_KeyboardEvent;
        bool m_flagsound;
        // Bien MuteSound tu Optionmenu
        
        #endregion

        #region Initialization

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            

        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                {
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
                    m_backgroundTexture = content.Load<Texture2D>("background"); // load an image with name "background"
                    m_buttonSound = content.Load<SoundEffect>("Sound/buttonpush");

                }
            }
            
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// 
        /// 
        /// </summary>

        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            PlaySound();
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullScreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(m_backgroundTexture, fullScreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }
        public void PlaySound()
        {
            m_KeyboardEvent = Keyboard.GetState();
            if (m_KeyboardEvent.IsKeyDown(Keys.Up) || m_KeyboardEvent.IsKeyDown(Keys.Down) || m_KeyboardEvent.IsKeyDown(Keys.Enter))
            {
                m_flagsound = true;

            }
            if (m_flagsound == true)
            {
                if (m_KeyboardEvent.IsKeyUp(Keys.Up) && m_KeyboardEvent.IsKeyUp(Keys.Down) && m_KeyboardEvent.IsKeyUp(Keys.Enter))
                {
                    if (OptionsMenuScreen.m_flagSound == true)
                    {
                        m_buttonSound.Play();
                    }
                    m_flagsound = false;

                }
            }
        }
        #endregion
    }
}
