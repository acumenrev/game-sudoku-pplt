using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Sudoku.Screens
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
            GameplayScreen.m_flagTime = false;
            // Create our menu entries

            m_congMsgMenuEntry = new MenuEntry("Congratulation!");
            m_messageMenuEntry = new MenuEntry("You win!");
            m_timeMenuEntry = new MenuEntry(string.Empty);
            m_replayMenuEntry = new MenuEntry("Replay");
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
            MenuEntries.Add(m_quitMenuEntry);
            
        }

        private void SetMenuEntryText()
        {

            m_timeMenuEntry.Text = "Time: " + clsTime.getInstance().GetTime();
        }

        #endregion

        #region Handle Input

        void ReplayMenuEntrySelected(object sender,PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LevelScreen(), e.PlayerIndex);

            
            //Map.Level.m_level = LevelScreen.m_level;
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());

        }

        void QuitMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string msg = "Are you sure you want to exit ?";
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(msg);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, e.PlayerIndex);


        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());

        }

        #endregion
    }
}
