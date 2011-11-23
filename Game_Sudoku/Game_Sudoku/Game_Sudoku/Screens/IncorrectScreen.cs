using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Game_Sudoku.Screens
{
    class IncorrectScreen : MenuScreen
    {
        #region Fields
        MenuEntry backMenuEntry;
        MenuEntry tryAgainMessage;
        #endregion

        #region Initialization
        public IncorrectScreen():base("Incorrect")
        {
            // create our menu entries
            backMenuEntry = new MenuEntry("Back");
            tryAgainMessage = new MenuEntry("Your sudoku is not correct.");
            
            // hook up the event handler
            backMenuEntry.Selected += OnCancel;

            // Add entries to the Menu
            MenuEntries.Add(tryAgainMessage);
            MenuEntries.Add(backMenuEntry);

        }
        
        #endregion

        #region Handle Input
        #endregion
    }
}
