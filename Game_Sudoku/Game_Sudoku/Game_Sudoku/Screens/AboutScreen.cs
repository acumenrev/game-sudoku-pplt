using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Game_Sudoku.Screens
{
    class AboutScreen : MenuScreen
    {
        #region Fields

        MenuEntry m_backMenuEntry;
        MenuEntry m_projectMenuEntry;
        MenuEntry m_versionMenuEntry;
        MenuEntry m_authorMenuEntry;
        MenuEntry m_thinhMenuEntry;
        MenuEntry m_triMenuEntry;
        
        

        #endregion

        #region Initialization

        public AboutScreen():base("About")
        {
            // Create our menu entries
            m_backMenuEntry = new MenuEntry("Back");
            m_projectMenuEntry = new MenuEntry(string.Empty);
            m_versionMenuEntry = new MenuEntry(string.Empty);
            m_authorMenuEntry = new MenuEntry(string.Empty);
            m_thinhMenuEntry = new MenuEntry(string.Empty);
            m_triMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();
            // Hook up the event handler
            m_backMenuEntry.Selected += OnCancel;
            

            // Add entries to the menu
            MenuEntries.Add(m_projectMenuEntry);
            MenuEntries.Add(m_versionMenuEntry);
            MenuEntries.Add(m_authorMenuEntry);
            MenuEntries.Add(m_thinhMenuEntry);
            MenuEntries.Add(m_triMenuEntry);
            MenuEntries.Add(m_backMenuEntry);
        }

       

        private void SetMenuEntryText()
        {
            m_projectMenuEntry.Text = "Project: TNTSudoku                        ";
            m_versionMenuEntry.Text = "Verion:  1.00                             ";
            m_authorMenuEntry.Text = "Authors:  Bui Trong Nghia - 09520185      ";
            m_thinhMenuEntry.Text = "Tran Van Thinh - 09520286";
            m_triMenuEntry.Text = "Vo Minh Tri - 09520319";
        }

        

        #endregion

        
    }
}
