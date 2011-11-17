using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_Sudoku.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        // init fields
        MenuEntry m_soundMenuEntry;
        MenuEntry m_musicMenuEntry;
        MenuEntry m_backMenuEntry;

        public static bool m_bSound = true;
        public static bool m_bMusic = true;
        
        #endregion

        #region Initialization

        public OptionsMenuScreen(): base("Options")
        {
            // Create our menu entries
            m_soundMenuEntry = new MenuEntry(string.Empty);
            m_musicMenuEntry = new MenuEntry(string.Empty);
            m_backMenuEntry = new MenuEntry("Back");
            SetMenuEntryText();
            // Hook up menu event handlers
            m_backMenuEntry.Selected += OnCancel;
            m_soundMenuEntry.Selected += SoundMenuEntrySelected;
            m_musicMenuEntry.Selected += MusicMenuEntrySelected;

            // Add entries to the menu
            MenuEntries.Add(m_soundMenuEntry);
            MenuEntries.Add(m_musicMenuEntry);
            MenuEntries.Add(m_backMenuEntry);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            m_soundMenuEntry.Text = "Sound : " + (m_bSound ? "On" : "Off");
            m_musicMenuEntry.Text = "Music : " + (m_bMusic ? "On" : "Off");

        }
        #endregion

        #region Handle Input

        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            m_bSound = !m_bSound;
            
            SetMenuEntryText();
            // can xet them cho GameplayScreen
        }

        void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            m_bMusic = !m_bMusic;
            
            SetMenuEntryText();
            // xet them cho cai Gameplay screen
        }


        #endregion
    }
}
