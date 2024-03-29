﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;

namespace Game_Sudoku.Screens
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        string m_message;
        Texture2D m_gradientTexture;

        InputAction m_menuSelect;
        InputAction m_menuCancel;
        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;


        #endregion

        #region Initialization

        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public MessageBoxScreen(string msg):this(msg,true)
        {

        }

        public MessageBoxScreen(string msg, bool includeUseageText)
        {
            const string usageText = " \nEnter = OK\nEsc = Cancel";
            if (includeUseageText)
            {
                this.m_message = msg + usageText;
            }
            else
            {
                this.m_message = msg;
            }

            IsPopup = true;
          
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            m_menuSelect = new InputAction(null, new Keys[] { Keys.Enter }, true);
            m_menuCancel = new InputAction(null, new Keys[] { Keys.Escape }, true);
        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                ContentManager content = ScreenManager.Game.Content;
                m_gradientTexture = content.Load<Texture2D>("gradient");
            }
            
        }
        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (m_menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }

                ExitScreen();
            }
            else
            {
                if (m_menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    // Raise the cancelled event, then exit the message box
                    if (Cancelled != null)
                    {
                        Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                    }
                    ExitScreen();
                }
            }
            base.HandleInput(gameTime, input);
        }
        #endregion

        #region Draw

        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(m_message);
            Vector2 textPostion = (viewportSize - textSize) / 2;

            // the background includes a border somewhat larger than the text itself
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPostion.X - hPad, 
                (int)textPostion.Y - vPad, 
                (int)textSize.X + hPad * 2, 
                (int)textSize.Y + vPad * 2);
            
            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            // Draw the background rectangle
            spriteBatch.Draw(m_gradientTexture, backgroundRectangle, color);

            spriteBatch.DrawString(font, m_message, textPostion, color);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}
