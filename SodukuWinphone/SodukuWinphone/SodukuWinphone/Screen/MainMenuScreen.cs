﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace SudokuWinphone
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen():base("Main Menu")
        {
            IsPopup = true;
            // Create our menu entries
            MenuEntry playerGameMenuEntry = new MenuEntry("New Game");
            MenuEntry SolveMenuEntry = new MenuEntry("Solve Sudoku");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");            
            MenuEntry aboutMenuEntry = new MenuEntry("About");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
           
            // Hook up menu event handlers
            playerGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            SolveMenuEntry.Selected += SolveMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            aboutMenuEntry.Selected += AboutMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add Entries to the menu
            MenuEntries.Add(playerGameMenuEntry);
            MenuEntries.Add(SolveMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(aboutMenuEntry);
            MenuEntries.Add(exitMenuEntry);

        }
        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
            ScreenManager.AddScreen(new LevelScreen(), null);
            ExitScreen();
           
        }
        void SolveMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new SolveSudokuScreen());
            ExitScreen();

        }
        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
           
        }

        void AboutMenuEntrySelected(object sender,PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new AboutScreen(), e.PlayerIndex);
            ExitScreen();
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            //const string msg = "Are you sure you want to exit this game ?";
            //MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(msg);
            //confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            //ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
            ScreenManager.Game.Exit();
        }

        


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();

        }

        #endregion
    }
}
