﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
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
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class Incorretscreen : MenuScreen
    {
        #region Initialization
  
        /// <summary>
        ///  Constructor
        /// </summary>
        public Incorretscreen():base("")
        {
            IsPopup = true;
            // Create our menu entries
            MenuEntry resumeGameMenuEntry = new MenuEntry("Your input isn't corret, Please check again!");



            // Hook up menu event handlers
            resumeGameMenuEntry.Selected += ResumeGameMenuEntry;


            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);

            
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>


        void ResumeGameMenuEntry(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
            GameplayScreen.m_pauseAlpha = 0f;
            GameplayScreen.m_flagbutton = false;
            GameplayScreen.m_flagtime = true;
            GameplayScreen.m_flagsound = false;
            SolveSudokuScreen.m_pauseAlpha = 0f;
            SolveSudokuScreen.m_flagbutton = false;
            SolveSudokuScreen.m_flagsound = false;
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>




        #endregion
    }
}
