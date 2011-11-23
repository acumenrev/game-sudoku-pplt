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
    class SolveSudokuScreen : GameScreen
    {
        #region Fields
        SpriteBatch m_spriteBatch;
        ContentManager content;
        SpriteFont m_gameFont;
        SpriteFont m_timefont;
        SpriteFont m_levelfont;
        SpriteFont m_gameFontError;
        Texture2D m_gamescreenBG;
        Texture2D m_buttonReseton;
        Texture2D m_buttonSettingon;
        Texture2D m_buttonQuiton;
        Texture2D m_buttonResetoff;
        Texture2D m_buttonSettingoff;
        Texture2D m_buttonQuitoff;
        Texture2D m_buttonSolve;
        int[,] m_sudoku =
            new int[9, 9]
			{
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0}
			};
        int[,] m_lockMatrix = new int[9, 9];
       
        MouseState mouseStateCurrent, mouseStatePrevious;
        Vector2 m_v2;
        SoundEffect m_buttonSound;
        SoundEffect m_beginSound;
        Xuly.Sudoku m_solveSudoku;
        bool m_flagChangeNumber;
        bool m_flagSound = true;
        float m_pauseAlpha;
        private static bool m_flagSolve = false;
        private static bool m_flagSoundMenu = false;
      
        KeyboardState m_keyboardEvent;
        InputAction m_pauseAction;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public SolveSudokuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            m_v2 = new Vector2(0, 0);
            m_pauseAction = new InputAction(null, new Keys[] { Keys.Escape }, true);
            m_solveSudoku = new Xuly.Sudoku(1);
        }

        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                m_gameFont = content.Load<SpriteFont>("gamefont");
                m_gameFontError = content.Load<SpriteFont>("gameerror");
                m_timefont = content.Load<SpriteFont>("timefont");
                m_levelfont = content.Load<SpriteFont>("levelfont");
                m_buttonSolve = content.Load<Texture2D>("Buttons/Solve");
                m_gamescreenBG = content.Load<Texture2D>("Background/gamescreenBG");
                m_buttonQuiton = content.Load<Texture2D>("Buttons/quiton");
                m_buttonQuitoff = content.Load<Texture2D>("Buttons/quitoff");
                m_buttonReseton = content.Load<Texture2D>("Buttons/reseton");
                m_buttonResetoff = content.Load<Texture2D>("Buttons/resetoff");
                m_buttonSettingon = content.Load<Texture2D>("Buttons/settingon");
                m_buttonSettingoff = content.Load<Texture2D>("Buttons/settingoff");

                m_buttonSound = content.Load<SoundEffect>("Sound/buttonpush");
                m_beginSound = content.Load<SoundEffect>("Sound/startgame");
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
            // Save the current state of mouse
            mouseStatePrevious = mouseStateCurrent;
            PlaySound();
            
            ChangeNumber();
            // Gradually fade in or out depending on whether we are covered by the pause screen.

            if (coveredByOtherScreen)
            {
                m_pauseAlpha = Math.Min(m_pauseAlpha + 1f / 32, 1);
            }
            else
            {
                m_pauseAlpha = Math.Max(m_pauseAlpha - 1f / 32, 0);
            }

            
            base.Update(gameTime, otherScreenHasFocus, false);
            
        }

        /// <summary>
        /// Draw buttons, matrix
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            m_spriteBatch = ScreenManager.SpriteBatch;
            m_spriteBatch.Begin();

            // Render background
            m_spriteBatch.Draw(m_gamescreenBG, new Vector2(0, 0), Color.White);
            
            // Draw Matrix
            DrawMatrix();
            // Draw buttons
            DrawButtons();
            // Draw message when player's answer is wrong
            //DrawWrongMess();
          
            m_spriteBatch.End();

            // If the game is transitioning on or Off, it out to black
            if (TransitionPosition > 0 || m_pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, m_pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);

            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            mouseStateCurrent = Mouse.GetState();
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                  mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                m_v2.X = (float)mouseStateCurrent.X;
                m_v2.Y = (float)mouseStateCurrent.Y;
                m_flagChangeNumber = true;
                
                EventButtons();
            }
            // look up inputs for the active player profile
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.m_CurrentKeyboardStates[playerIndex];
            PlayerIndex player;
            // catch the event handlers when Esc is pressed
            if (m_pauseAction.Evaluate(input, ControllingPlayer, out player))
            {
                //Add PauseScreen to current screen
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                m_flagSoundMenu = true;
            }
            base.HandleInput(gameTime, input);
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

                    // Draw lock matrix
                    if (m_lockMatrix[j, i] != 0)
                    {
                        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                        string a = m_lockMatrix[j, i].ToString();
                        float x = (i * 60) + 50;
                        float y = (j * 60) + 30;
                        spriteBatch.DrawString(m_gameFont, a, new Vector2(x, y), Color.Blue);
                    }

                    if (m_lockMatrix[j,i] == 0 && m_solveSudoku.m_Sudoku[j, i] != 0)
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
        /// Draw buttons
        /// </summary>
        public void DrawButtons()
        {
            //m_spriteBatch.DrawString(m_gameFont, mouseStateCurrent.X.ToString(), new Vector2(0, 0), Color.Black);
            //m_spriteBatch.DrawString(m_gameFont, mouseStateCurrent.Y.ToString(), new Vector2(0, 20), Color.Black);
            // Draw button Reset - On
            m_spriteBatch.Draw(m_buttonReseton, new Vector2(610, 400), Color.White);
            // Draw button Setting - ON
            m_spriteBatch.Draw(m_buttonSettingon, new Vector2(610, 440), Color.White);
            // Draw button Quit
            m_spriteBatch.Draw(m_buttonQuiton, new Vector2(610, 480), Color.White);
            // Draw button Solve
            m_spriteBatch.Draw(m_buttonSolve, new Vector2(700, 150), Color.White);
            // Draw button Reset - Off
            if (mouseStateCurrent.X > 610 && mouseStateCurrent.X < 735)
            {
                if (mouseStateCurrent.Y > 400 && mouseStateCurrent.Y < 440)
                {
                    m_spriteBatch.Draw(m_buttonResetoff, new Vector2(610, 400), Color.White);
                }
            }

            // Draw button Setting - OFF
            if (mouseStateCurrent.X > 610 && mouseStateCurrent.X < 735)
            {
                if (mouseStateCurrent.Y > 445 && mouseStateCurrent.Y < 480)
                {
                    m_spriteBatch.Draw(m_buttonSettingoff, new Vector2(610, 440), Color.White);
                }
            }
            // Draw button Quit - OFF
            if (mouseStateCurrent.X > 610 && mouseStateCurrent.X < 735)
            {
                if (mouseStateCurrent.Y > 485 && mouseStateCurrent.Y < 515)
                {
                    m_spriteBatch.Draw(m_buttonQuitoff, new Vector2(610, 480), Color.White);
                }
            }
        }

        #endregion

        #region Methods

        

        /// <summary>
        /// Handle input from buttons
        /// </summary>
        public void EventButtons()
        {
            if (m_flagSolve == false)
            {
                // handle when player clicks solve button
                if (mouseStateCurrent.X > 700 && mouseStateCurrent.X < 743)
                {
                    if (mouseStateCurrent.Y > 150 && mouseStateCurrent.Y < 190)
                    {
                        //copy cells from m_sudoku to m_lockMatrix
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                if (m_solveSudoku.m_Sudoku[i, j] != 0)
                                {
                                    m_lockMatrix[i, j] = m_solveSudoku.m_Sudoku[i, j];
                                }
                            }
                        }

                        m_solveSudoku.CopyToV3();
                        if (m_solveSudoku.Solve() == true)
                        {
                            m_solveSudoku.ShowSolve();
                            m_flagSolve = true;
                        }
                        else
                        {
                            // if player's answer is wrong, then draw wrong message dialog
                            ScreenManager.AddScreen(new IncorrectScreen(), ControllingPlayer);
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    m_lockMatrix[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }

            // handle when player clicks Reset button
            if (mouseStateCurrent.X > 610 && mouseStateCurrent.X < 735)
            {
                if (mouseStateCurrent.Y > 400 && mouseStateCurrent.Y < 440)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            m_solveSudoku.m_Sudoku[i, j] = 0;
                            m_lockMatrix[i,j] = 0;
                        }
                    }
                    m_flagSolve = false;
                }
            }


            if (m_v2.X > 610 && m_v2.X < 735)
            {
                if (m_v2.Y > 445 && m_v2.Y < 480)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                    m_flagSoundMenu = true;
                }
            }

            // popup message when player wanna quit game
            if (mouseStateCurrent.X > 610 && mouseStateCurrent.X < 735)
            {
                if (mouseStateCurrent.Y > 485 && mouseStateCurrent.Y < 515)
                {
                    const string message = "Are you sure you want to quit this game?";
                    MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
                    confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
                    ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
                }
            }
        }

        /// <summary>
        /// handle event when player clicks on a sudoku's cell
        /// </summary>
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
                        int numbercurrent = m_solveSudoku.m_Sudoku[(int)y, (int)x];
                        if (numbercurrent == 9)
                        {
                            numbercurrent = 0;
                        }
                        m_solveSudoku.m_Sudoku[(int)y, (int)x] = numbercurrent + 1;
                        m_flagChangeNumber = false;
                    }

                }
            }

        }

        /// <summary>
        /// Show ConfirmQuitMessageBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());

        }

        /// <summary>
        /// Play sound
        /// </summary>
        public void PlaySound()
        {

            m_keyboardEvent = Keyboard.GetState();

            if (m_flagSoundMenu == true)
            {
                if (m_keyboardEvent.IsKeyDown(Keys.Up) || m_keyboardEvent.IsKeyDown(Keys.Down) || m_keyboardEvent.IsKeyDown(Keys.Enter) || m_keyboardEvent.IsKeyDown(Keys.Escape))
                {
                    m_flagSound = true;

                }
                if (m_flagSound == true)
                {
                    if (m_keyboardEvent.IsKeyUp(Keys.Up) && m_keyboardEvent.IsKeyUp(Keys.Down) && m_keyboardEvent.IsKeyUp(Keys.Enter) && m_keyboardEvent.IsKeyUp(Keys.Escape))
                    {

                        // Turn on/off sound of pause screen
                        if (OptionsMenuScreen.m_flagSound == true)
                        {
                            m_buttonSound.Play();
                        }
                        m_flagSound = false;

                    }
                }

            }
        }

        #endregion


    }
}
