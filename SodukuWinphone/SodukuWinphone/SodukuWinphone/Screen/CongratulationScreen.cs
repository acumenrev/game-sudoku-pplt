using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using GameStateManagement;

namespace SudokuWinphone
{
    class CongratulationScreen : MenuScreen
    {
        #region Fields

        MenuEntry m_congMsgMenuEntry;
        MenuEntry m_messageMenuEntry;
        MenuEntry m_timeMenuEntry;
        MenuEntry m_replayMenuEntry;
        MenuEntry m_quitMenuEntry;

        #endregion

        #region Initialization

        public CongratulationScreen():base("Congratulation")
        {
            IsPopup = true;
            GameplayScreen.m_flagtime = false;
            // Create our menu entries

            m_congMsgMenuEntry = new MenuEntry("Congratulation!");
            m_messageMenuEntry = new MenuEntry("You win!");
            m_timeMenuEntry = new MenuEntry(string.Empty);
            m_replayMenuEntry = new MenuEntry("Play again");
            m_quitMenuEntry = new MenuEntry("Quit");

            SetMenuEntryText();
            // Hook up the menu event handlers
            
            m_replayMenuEntry.Selected += ReplayMenuEntrySelected;
            m_quitMenuEntry.Selected += QuitMenuEntrySelected;

            // Add entries to the menu
            MenuEntries.Add(m_congMsgMenuEntry);
            MenuEntries.Add(m_messageMenuEntry);
            MenuEntries.Add(m_timeMenuEntry);
            MenuEntries.Add(m_replayMenuEntry);
            MenuEntries.Add
                
                (m_quitMenuEntry);
            
        }

        private void SetMenuEntryText()
        {

            m_timeMenuEntry.Text = "Time: " + Sudoku.clsTime.getInstance().GetTime();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Replay Game menu entry is selected.
        /// </summary>
        void ReplayMenuEntrySelected(object sender,PlayerIndexEventArgs e)
        {
            ExitScreen();
            ScreenManager.AddScreen(new LevelScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Guide.BeginShowMessageBox("TNT - Sudoku", "Do you want to quit your game?",
                                                                      new String[] { "Yes", "No" }, 0, MessageBoxIcon.Warning,
                                                                      ShowDialogEnded, null);


        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());

        }
        private void ShowDialogEnded(IAsyncResult result)
        {
            int? res = Guide.EndShowMessageBox(result);
            if (res.HasValue)
            {
                if (res.Value == 0)
                {
                    foreach (GameScreen screen in ScreenManager.GetScreens())
                        screen.ExitScreen();

                    ScreenManager.AddScreen(new BackgroundScreen(),
                        null);
                    ScreenManager.AddScreen(new MainMenuScreen(), null);
                }

            }

        }
        #endregion
    }
}
