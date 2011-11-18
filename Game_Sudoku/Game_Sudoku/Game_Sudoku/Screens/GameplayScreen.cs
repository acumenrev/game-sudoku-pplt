using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using GameStateManagement;

namespace Game_Sudoku.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont m_gameFont;
        SpriteFont m_timefont;
        SpriteFont m_levelfont;
        SpriteFont m_gameFontError;
        Texture2D m_gamescreenBG;
        
        SpriteBatch m_spriteBatch;
        
        int [,] m_lockMatrixNumber = new int [9,9];
        int[,] m_resultMatrixNumber = new int[9, 9];
        MouseState mouseStateCurrent, mouseStatePrevious;
        Vector2 m_v2;
        Vector2 m_vTime;
        Xuly.Sudoku m_solveSudoku;
        SoundEffect m_finishSound;
        SoundEffect m_buttonSound;
        SoundEffect m_beginSound;
        bool m_flagsound;
        public static bool m_flagsoundmenu=false;
        KeyboardState m_KeyboardEvent;
        string m_errorSudoku;
        //Vector2 playerPosition = new Vector2(100, 100);
        //Vector2 enemyPosition = new Vector2(100, 100);

        //Random random = new Random();

        float m_pauseAlpha;
        
        // time
        clsTime m_time;
        public static bool m_flagTime;


        bool m_flagChangeNumber;
        

        InputAction m_pauseAction;
        
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            m_v2 = new Vector2(0,0);
            m_vTime = new Vector2(630, 320);
            m_pauseAction = new InputAction(null,new Keys[]{ Keys.Escape}, true);
            m_flagTime = true;
            m_time = clsTime.getInstance();
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {

            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                m_gameFont = content.Load<SpriteFont>("gamefont");
                m_gameFontError = content.Load<SpriteFont>("gameerror");
                m_gamescreenBG = content.Load<Texture2D>("Background/gamescreenBG");
                m_timefont = content.Load<SpriteFont>("timefont");
                m_levelfont = content.Load<SpriteFont>("levelfont");
                m_finishSound = content.Load<SoundEffect>("Sound/finish");
                m_buttonSound = content.Load<SoundEffect>("Sound/buttonpush");
                m_beginSound = content.Load<SoundEffect>("Sound/startgame");
                

                //Load Map
                             
                m_solveSudoku = new Xuly.Sudoku();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        m_lockMatrixNumber[i, j] = m_solveSudoku.m_Sudoku[i, j];
                    }

                }
                m_resultMatrixNumber = m_solveSudoku.m_resultMatrix;
                
                //play Sound Begin
                if (OptionsMenuScreen.m_bMusic == true)
                {
                    m_beginSound.Play();
                }

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }
        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            
            //Play sound keyboard
            PlaySound();
            //
            // catch the mouse events
            mouseStateCurrent = Mouse.GetState();
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                  mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                m_v2.X = (float)mouseStateCurrent.X;
                m_v2.Y = (float)mouseStateCurrent.Y;
                m_flagChangeNumber = true;
                SolveSudoku();
            }

            if (mouseStateCurrent.X > MainGame.m_graphics.PreferredBackBufferWidth ||
                mouseStateCurrent.Y > MainGame.m_graphics.PreferredBackBufferHeight)
            {

                m_flagTime = false;
            }
            else
            {
                m_flagTime = true;
            }
            
            mouseStatePrevious = mouseStateCurrent;

            

            // Play sound when sudoku is solved
            if (OptionsMenuScreen.m_bMusic == true)
            {
                 Finisheffect();
            }

            // Draw timer
            // if paused is active --> stop time and changing number
            if (m_flagTime == true)
            {
                m_time.IncreaseTime(gameTime);
                //
                ChangeNumber();
            }
           

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                m_pauseAlpha = Math.Min(m_pauseAlpha + 1f / 32, 1);
            }
            else
            {
                 m_pauseAlpha = Math.Max(m_pauseAlpha - 1f / 32, 0);
            }

            
            //if (IsActive)
            //{
            //    // Apply some random jutter to make the enemy move around
            //}
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {

            if (input == null)
                throw new ArgumentNullException("input");

            // look up inputs for the active player profile
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.m_CurrentKeyboardStates[playerIndex];


            PlayerIndex player;
            if (m_pauseAction.Evaluate(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                m_flagTime = false;
                m_flagsoundmenu = true;
            }
            else
            {
                // handle movement of enemy, player
            }
            
            
            base.HandleInput(gameTime, input);
        }

        /// <summary>
        /// Draw methods
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            m_spriteBatch = ScreenManager.SpriteBatch;
            
            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_gamescreenBG, new Vector2(0, 0), Color.White);
            m_spriteBatch.DrawString(m_timefont, m_time.GetTime(), new Vector2(630, 318), Color.White);
            m_spriteBatch.DrawString(m_levelfont, GetLevel(), new Vector2(635, 235),Color.White);
            DrawMatrix();
            m_spriteBatch.DrawString(m_gameFont,m_v2.X.ToString(), m_v2, Color.White);
            //if (!m_solveSudoku.Solve())
            //{
            //    m_spriteBatch.DrawString(m_gameFontError, m_errorSudoku, new Vector2(0,0), Color.Yellow);
            //}
            
            m_spriteBatch.End();

            // If the game is transitioning on or Off, it out to black
            if (TransitionPosition > 0 || m_pauseAlpha >0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, m_pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);

            }

            
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw matrix to sudoku grid
        /// </summary>
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
                        float x = (i * 60) + 50;
                        float y = (j * 60) + 30;
                        spriteBatch.DrawString(m_gameFont, a, new Vector2(x, y), Color.Blue);
                    }

                    //if (m_lockMatrixNumber[j, i] == 0 &&  m_mapDemo.m_matrixMap[j, i] != 0)
                    if (m_lockMatrixNumber[j, i] == 0 && m_solveSudoku.m_Sudoku[j, i] != 0)
                    {
                        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                        //string a = m_mapDemo.m_matrixMap[j, i].ToString();
                        string a = m_solveSudoku.m_Sudoku[j, i].ToString();
                        float x = (i * 60) + 50;
                        float y = (j * 60) + 30;
                        spriteBatch.DrawString(m_gameFont, a, new Vector2(x, y), Color.White);
                    }




                }
            }


        }

        /// <summary>
        /// Gets level status
        /// </summary>
        /// <returns></returns>
        private string GetLevel()
        {
            string level = string.Empty;
            switch(Map.Level.m_level)
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


        public void ChangeNumber()
        {
            float x = (m_v2.X - 30) / 60;
            float y = (m_v2.Y - 30) / 60;
            if (m_flagChangeNumber == true)
            {
                if (x >= 0 && x <= 9)
                {
                    if (y >= 0 && y <= 9)
                    {
                        if (m_lockMatrixNumber[(int)y, (int)x] == 0)
                        {
                            //int numbercurrent = (int)m_mapDemo.m_matrixMap[(int)y, (int)x];
                            int numbercurrent = m_solveSudoku.m_Sudoku[(int)y, (int)x];
                            if (numbercurrent == 9)
                            {
                                numbercurrent = 0;
                            }
                            //m_mapDemo.m_matrixMap[(int)y, (int)x] = numbercurrent + 1;
                            m_solveSudoku.m_Sudoku[(int)y, (int)x] = numbercurrent + 1;
                            m_flagChangeNumber = false;
                        }

                    }

                }
            }


        }


        public void PlaySound()
        {
            
            m_KeyboardEvent = Keyboard.GetState();

            if (m_flagsoundmenu == true)
            {
                if (m_KeyboardEvent.IsKeyDown(Keys.Up) || m_KeyboardEvent.IsKeyDown(Keys.Down) || m_KeyboardEvent.IsKeyDown(Keys.Enter) || m_KeyboardEvent.IsKeyDown(Keys.Escape))
                {
                    m_flagsound = true;

                }
                if (m_flagsound == true)
                {
                    if (m_KeyboardEvent.IsKeyUp(Keys.Up) && m_KeyboardEvent.IsKeyUp(Keys.Down) && m_KeyboardEvent.IsKeyUp(Keys.Enter) && m_KeyboardEvent.IsKeyUp(Keys.Escape))
                    {

                        // Turn on/off sound of pause screen
                        if (OptionsMenuScreen.m_bSound == true)
                        {
                            m_buttonSound.Play();
                        }
                        m_flagsound = false;

                    }
                }

            }    
        }

        /// <summary>
        /// Solving sudoku grid
        /// </summary>
        public void SolveSudoku()
        {
            
            if(m_v2.X>700)
            {
                m_solveSudoku.Solve();

                m_solveSudoku.ShowSolve();
                //ScreenManager.AddScreen(new CongratulationScreen(), ControllingPlayer);
                LoadingScreen.Load(ScreenManager, true, 
                    ControllingPlayer, new CongratulationScreen());
            }

            
        }

        /// <summary>
        /// Sound effect when Sudoku is solved
        /// </summary>
        public void Finisheffect()
        {

                if (m_solveSudoku.m_Sudoku == m_resultMatrixNumber)
                {
                    m_finishSound.Play();
                  
                }
         
            
        }
        #endregion
    }
}
