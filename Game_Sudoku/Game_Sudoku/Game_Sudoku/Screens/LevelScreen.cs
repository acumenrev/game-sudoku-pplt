using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace Game_Sudoku.Screens
{
    class LevelScreen : MenuScreen
    {
        #region Fields

        MenuEntry m_easyMenuEntry;
        MenuEntry m_mediumMenuEntry;
        MenuEntry m_hardMenuEntry;
        MenuEntry m_backMenuEntry;


        #endregion

        #region Initialization

        public LevelScreen():base("Choose level")
        {
            // Create our menu entries
            m_backMenuEntry = new MenuEntry("Back");
            m_easyMenuEntry = new MenuEntry("Easy");
            m_mediumMenuEntry = new MenuEntry("Medium");
            m_hardMenuEntry = new MenuEntry("Hard");


            // Hook up menu event handlers
            m_backMenuEntry.Selected += OnCancel;
            m_easyMenuEntry.Selected += EasyMenuEntrySelected;
            m_mediumMenuEntry.Selected += MediumMenuEntrySelected;
            m_hardMenuEntry.Selected += HardMenuEntrySelected;

            // Add entries to the menu
            MenuEntries.Add(m_easyMenuEntry);
            MenuEntries.Add(m_mediumMenuEntry);
            MenuEntries.Add(m_hardMenuEntry);
            MenuEntries.Add(m_backMenuEntry);
        }
        

        #endregion

        #region Handle Input

        void EasyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Map.Level.m_level = 0;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }

        void MediumMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Map.Level.m_level = 1;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }

        void HardMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Map.Level.m_level = 2;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }
        #endregion
    }
}
