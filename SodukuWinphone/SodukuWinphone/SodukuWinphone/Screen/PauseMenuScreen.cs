using Microsoft.Xna.Framework;
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
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization
  
        /// <summary>
        ///  Constructor
        /// </summary>
        public PauseMenuScreen():base("Paused")
        {
            IsPopup = true;
            // Create our menu entries
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers
            resumeGameMenuEntry.Selected += ResumeGameMenuEntry;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
            
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Guide.BeginShowMessageBox("TNT - Sudoku", "Do you want to quit your game?",
                                                                      new String[] { "Yes", "No" }, 0, MessageBoxIcon.Warning,
                                                                      ShowDialogEnded, null);
        }

        void ResumeGameMenuEntry(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
            GameplayScreen.m_pauseAlpha = 0f;
            GameplayScreen.m_flagbutton = false;
            GameplayScreen.m_flagtime = true;
            GameplayScreen.m_flagsound = false;
        }

        void OptionsMenuEntrySelected(object sender,PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>

        private void ShowDialogEnded(IAsyncResult result)
        {
            int? res = Guide.EndShowMessageBox(result);
            if (res.HasValue)
            {
                if (res.Value == 0)
                {
                    foreach (GameScreen screen in ScreenManager.GetScreens())
                        screen.ExitScreen();
                    GameplayScreen.m_flagscreen = false;
                    ScreenManager.AddScreen(new BackgroundScreen(),
                        null);
                    ScreenManager.AddScreen(new MainMenuScreen(), null);
                }

            }

        }
        #endregion
    }
}
