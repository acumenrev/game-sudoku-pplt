
#region Library
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using System.IO.IsolatedStorage;
using System.IO;
using System;
using System.Threading;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameStateManagement;
#endregion

namespace SudokuWinphone
{
    class GameplayScreen:GameScreen
    {
        #region Fields
        //SpriteBacth
        SpriteBatch m_spriteBatch;
        //Texture2D
        Texture2D m_gameplayBG;
        Texture2D m_buttonReseton;
        Texture2D m_buttonSettingon;
        Texture2D m_buttonQuiton;
        Texture2D m_buttonResetoff;
        Texture2D m_buttonSettingoff;
        Texture2D m_buttonQuitoff;
        Texture2D m_buttonSolve;
        Texture2D m_buttonCheck;
        Texture2D m_wrongMess;

        //SpriteFont
        SpriteFont m_gameplayFont;
        SpriteFont m_gametimeFont;

        //Demo String
        int DemoString = 0;
        float m_pauseAlpha;

        //Flag Time and Sound
        public static bool m_flagtime;
        public static bool m_flagsound;

        // Matrix Lock and Result
        int[,] m_lockMatrixNumber = new int[9, 9];
        int[,] m_resultMatrixNumber = new int[9, 9];

        //Sudoku
        Sudoku.Sudoku m_sudoku;

        // Vector
        Vector2 m_v2=Vector2.Zero; // Vector of number tap
        Vector2 m_vtime = new Vector2(636,268); // Vector of time on screen
        // Time in Sudoku
        Sudoku.clsTime m_time;
        #endregion
        #region Init
        //Contructor
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            // Accept what gesture?
            EnabledGestures = GestureType.Tap;
            //
           
            //Sudoku & Load Map
            m_sudoku = new Sudoku.Sudoku();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_lockMatrixNumber[i, j] = m_sudoku.m_Sudoku[i, j];
                }

            }
            //Time (What's Hell? Plz explain it for me here)
            m_time = Sudoku.clsTime.getInstance();
        }

        public override void LoadContent()
        {
            //LoadConten Texture2D
            m_gameplayBG = Load<Texture2D>("gamescreenBG");
            m_buttonQuiton = Load<Texture2D>("Buttons/quiton");
            m_buttonQuitoff =Load<Texture2D>("Buttons/quitoff");
            m_buttonReseton = Load<Texture2D>("Buttons/reseton");
            m_buttonResetoff = Load<Texture2D>("Buttons/resetoff");
            m_buttonSettingon = Load<Texture2D>("Buttons/settingon");
            m_buttonSettingoff = Load<Texture2D>("Buttons/settingoff");
            m_buttonSolve = Load<Texture2D>("Buttons/Solve");
            m_buttonCheck = Load<Texture2D>("Buttons/Check");
            m_wrongMess = Load<Texture2D>("Buttons/wrongmess");

            //LoadConten Font
            m_gameplayFont = Load<SpriteFont>("GameFont");
            m_gametimeFont = Load<SpriteFont>("TimeFont");
            //
            Thread.Sleep(1000);
            base.LoadContent();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
#endregion
#region Update & Draw
        // Update
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //get Time sudoku
            if (m_flagtime)
            {
                m_time.IncreaseTime(gameTime);
            }
            //pause screen
            if (coveredByOtherScreen)
            {
                m_pauseAlpha = Math.Min(m_pauseAlpha + 1f / 32, 2);
            }
            else
            {
                m_pauseAlpha = Math.Max(m_pauseAlpha - 1f / 32, 0);
            }

            //
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        // HandleInput
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            // Gesture Tap 
            foreach (var gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    m_v2.X = gesture.Position.X;
                    m_v2.Y = gesture.Position.Y;
                    ChangeNumber();
                    EventButton();
                    //DemoString += 2;
                }
            }
            base.HandleInput(input);
        }
        // Draw
        public override void Draw(GameTime gameTime)
        {
            m_spriteBatch = ScreenManager.SpriteBatch;
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_gameplayBG, Vector2.Zero, Color.White);

            m_spriteBatch.DrawString(m_gameplayFont, GetLevel(), Vector2.Zero, Color.White);
            //Draw Matrix Sudoku
            DrawMatrix();
            //Draw Button Sudoku
            DrawButton();
            //Draw Time Sudoku
            m_spriteBatch.DrawString(m_gametimeFont,m_time.GetTime().ToString(),m_vtime, Color.White);
 
            m_spriteBatch.End();
            //

            if (TransitionPosition > 0 || m_pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, m_pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);

            }
            base.Draw(gameTime);
        }
        // Draw Matrix Sudoku
        public void DrawMatrix()
        {

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_lockMatrixNumber[j, i] != 0)
                    {
                        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                        string a = m_lockMatrixNumber[j, i].ToString();
                        float x = (i * 60) + 35;
                        float y = (j * 50) + 25;
                        spriteBatch.DrawString(m_gameplayFont, a, new Vector2(x, y), Color.DarkSlateBlue);
                    }

                    //if (m_lockMatrixNumber[j, i] == 0 &&  m_mapDemo.m_matrixMap[j, i] != 0)
                    if (m_lockMatrixNumber[j, i] == 0 && m_sudoku.m_Sudoku[j, i] != 0)
                    {
                        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                        //string a = m_mapDemo.m_matrixMap[j, i].ToString();
                        string a = m_sudoku.m_Sudoku[j, i].ToString();
                        float x = (i * 60) + 35;
                        float y = (j * 50) + 25;
                        spriteBatch.DrawString(m_gameplayFont, a, new Vector2(x, y), Color.White);
                    }




                }
            }


        }
        // Draw Button in sudoku screen
        public void DrawButton()
        {

            //m_spriteBatch.DrawString(m_gameplayFont, m_v2.X.ToString(), new Vector2(0, 0), Color.Black);
            //m_spriteBatch.DrawString(m_gameplayFont, m_v2.Y.ToString(), new Vector2(0, 20), Color.Black);
            m_spriteBatch.Draw(m_buttonReseton, new Vector2(620, 335), Color.White);
            m_spriteBatch.Draw(m_buttonSettingon, new Vector2(620, 375), Color.White);
            m_spriteBatch.Draw(m_buttonQuiton, new Vector2(620, 415), Color.White);
            m_spriteBatch.Draw(m_buttonSolve, new Vector2(720, 100), Color.White);
            m_spriteBatch.Draw(m_buttonCheck, new Vector2(570, 80), Color.White);
            if (m_v2.X > 620 && m_v2.X < 745)
            {
                if ( m_v2.Y > 335 &&  m_v2.Y < 370)
                {
                    m_spriteBatch.Draw(m_buttonResetoff, new Vector2(620, 335), Color.White);
                }
            }

            if ( m_v2.X > 620 &&  m_v2.X < 745)
            {
                if ( m_v2.Y > 375 &&  m_v2.Y < 410)
                {
                    m_spriteBatch.Draw(m_buttonSettingoff, new Vector2(620, 375), Color.White);
                }
            }

            if ( m_v2.X > 620 &&  m_v2.X < 745)
            {
                if ( m_v2.Y > 415 &&  m_v2.Y < 455)
                {
                    m_spriteBatch.Draw(m_buttonQuitoff, new Vector2(620, 415), Color.White);
                }
            }
        }
        // Tap To Change Number On GameScreen
        public void ChangeNumber()
        {
            float x = (m_v2.X - 15) / 60;
            float y = (m_v2.Y - 15) / 50;
                if (x >= 0 && x <= 9)
                {
                    if (y >= 0 && y <= 9)
                    {
                        if (m_lockMatrixNumber[(int)y, (int)x] == 0)
                        {
                            //int numbercurrent = (int)m_mapDemo.m_matrixMap[(int)y, (int)x];
                            int numbercurrent = m_sudoku.m_Sudoku[(int)y, (int)x];
                            if (numbercurrent == 9)
                            {
                                numbercurrent = 0;
                            }
                            //m_mapDemo.m_matrixMap[(int)y, (int)x] = numbercurrent + 1;
                            m_sudoku.m_Sudoku[(int)y, (int)x] = numbercurrent + 1;
                          
                        }

                    }

                }
            }
        //Get Level Of Map
        private string GetLevel()
        {
            string level = string.Empty;
            switch (Sudoku.Level.m_level)
            {
                case 0:
                    level = "Easy";
                    break;
                case 1:
                    level = "Medium";
                    break;
                case 2:
                    level = "Hard";
                    break;
            }


            return level;
        }
        //Eventhandel Of Button
        public void EventButton()
        {
            //Check the answer matrix
            if (m_v2.X > 590 && m_v2.X < 650)
            {
                if (m_v2.Y > 130 && m_v2.Y < 190)
                {
                    if (m_sudoku.checkketqua() == true)
                    {
                        //ScreenManager.AddScreen(new CongratulationScreen(), ControllingPlayer);
                        if (OptionsMenuScreen.m_bMusic == true)
                        {
                            //m_finishSound.Play();
                        }

                    }
                    if (m_sudoku.checkketqua() == false)
                    {
                        //m_flagwrongmess = true;

                    }
                }
            }
            // Sovle Your Sudoku
            if (m_v2.X > 700 && m_v2.X < 743)
            {
                if (m_v2.Y > 150 && m_v2.Y < 190)
                {
                    m_sudoku.Solve();
                    m_sudoku.ShowSolve();
                }
            }
            // Reset to new Map
            if (m_v2.X > 620 && m_v2.X < 745)
            {
                if (m_v2.Y > 335 && m_v2.Y < 370)
                {
                    Sudoku.clsTime.getInstance().ResetTime();
                    GameplayScreen.m_flagtime = true;
                    LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new GameplayScreen());

                }
            }
            // Pause screen
            if (m_v2.X > 620 && m_v2.X < 745)
            {
                if (m_v2.Y > 375 && m_v2.Y < 410)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);

                    m_flagtime = false;
                    m_flagsound = true;
                }
            }
            // Exit 
            if (m_v2.X > 620 && m_v2.X < 745)
            {
                if (m_v2.Y > 415 && m_v2.Y < 455)
                {
                    //const string message = "Are you sure you want to quit this game?";
                    //MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
                    //confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

                    //ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
                    Guide.BeginShowMessageBox("TNT - Sudoku", "Do you want to quit your game?",
                                                new String[] { "Yes", "No" }, 0, MessageBoxIcon.Warning,
                                                ShowDialogEnded, null);
                }
            }
        }
        private void ShowDialogEnded(IAsyncResult result)
        {
            int? res = Guide.EndShowMessageBox(result);
            if(res.HasValue){
                if (res.Value==0)
                {
                    foreach (GameScreen screen in ScreenManager.GetScreens())
                        screen.ExitScreen();

                    ScreenManager.AddScreen(new BackgroundScreen(),
                        null);
                    ScreenManager.AddScreen(new MainMenuScreen(), null);
                }

            }
           
        }
    }

#endregion
    
}
