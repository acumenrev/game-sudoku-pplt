using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameStateManagement;

namespace Game_Sudoku.Screens
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        List<MenuEntry> m_menuEntries = new List<MenuEntry>();
        int m_selectedEntry = 0;
        string m_menuTitle = "TNTSUDOKU";
        BackgroundScreen a;
        InputAction m_menuUp;
        InputAction m_menuDown;
        InputAction m_menuSelect;
        InputAction m_menuCancel;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>

        protected IList<MenuEntry> MenuEntries
        {
            get
            {
                return m_menuEntries;
            }
        }
        #endregion

        #region Initialization

        public MenuScreen(string menuTitle)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            a = new BackgroundScreen();
            m_menuUp = new InputAction(null, new Keys[] { Keys.Up }, true);

            m_menuDown = new InputAction(null, new Keys[] { Keys.Down }, true);

            m_menuSelect = new InputAction(null, new Keys[] { Keys.Enter }, true);

            m_menuCancel = new InputAction(null, new Keys[] { Keys.Escape }, true);
        }
        #endregion

        #region Handle Input


        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // For input tests we pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;
            
            // Move to the previous menu entry?
            if (m_menuUp.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                m_selectedEntry--;
                
                if (m_selectedEntry < 0)
                {
                    m_selectedEntry = m_menuEntries.Count - 1;
                }
            }

            // Move to the next menu entry?
            if (m_menuDown.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                m_selectedEntry++;
                
                if (m_selectedEntry >= m_menuEntries.Count)
                {
                    m_selectedEntry = 0;
                }
            }

            if (m_menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                
                OnSelectEntry(m_selectedEntry, playerIndex);
            }
            else
            {
                if (m_menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    
                    OnCancel(playerIndex);
                }
            }
            
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            m_menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
            GameplayScreen.m_flagTime = true;
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
            
        }
        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            // start at Y = 175, each X value is genrated per entry
            Vector2 position = new Vector2(0f, 175f);

            // Update each menu entry's location in turn
            for (int i = 0; i < m_menuEntries.Count; i++)
            {
                MenuEntry menuEntry = m_menuEntries[i];

                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    position.X += transitionOffset * 512;

                }

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);

            }
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object
            for (int i = 0; i < m_menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == m_selectedEntry);

                m_menuEntries[i].Update(this, isSelected, gameTime);

            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            
            spriteBatch.Begin();

            // Draw each menu entry in turn
            for (int i = 0; i < m_menuEntries.Count; i++)
            {
                MenuEntry menuEntry = m_menuEntries[i];

                bool isSelected = IsActive && (i == m_selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this make
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(m_menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, m_menuTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
                
        }
        #endregion
    }
}
