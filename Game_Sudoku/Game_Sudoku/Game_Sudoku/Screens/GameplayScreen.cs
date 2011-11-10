using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        SpriteFont gameFont;
        SpriteFont m_timefont;
        Texture2D gamescreenBG;
        SpriteBatch spriteBatch;
        Map.Map Mapdemo;
        int [,] LockMatrixNumber = new int [9,9];
        MouseState mouseStateCurrent, mouseStatePrevious;
        Vector2 v2;
        Vector2 vTime;
        //Vector2 playerPosition = new Vector2(100, 100);
        //Vector2 enemyPosition = new Vector2(100, 100);

        //Random random = new Random();

        float pauseAlpha;
        
        // time
        clsTime time;
        bool flagTime;


        bool flagChangeNumber;
        

        InputAction pauseAction;
        
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            v2 = new Vector2(0,0);
            vTime = new Vector2(630, 320);
            pauseAction = new InputAction(null,new Keys[]{ Keys.Escape}, true);
            flagTime = true;
            time = new clsTime();
            
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

                gameFont = content.Load<SpriteFont>("gamefont");
                gamescreenBG = content.Load<Texture2D>("Background/gamescreenBG");
                m_timefont = content.Load<SpriteFont>("timefont");
                Mapdemo = new Map.Map();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        LockMatrixNumber[i, j] = Mapdemo.MatrixMap[i, j];
                    }

                }
                

                // A real game would probably have more content than this sample, so
                // it would take longer to load. We simulate that by delaying for a
                // while, giving you a chance to admire the beautiful loading screen.

                
                //Thread.Sleep(this.TransitionOnTime);

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
            
            // bắt sự kiện chuột
            mouseStateCurrent = Mouse.GetState();
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                v2.X = (float)mouseStateCurrent.X;
                v2.Y = (float)mouseStateCurrent.Y;
                flagChangeNumber = true;
            }

            mouseStatePrevious = mouseStateCurrent;
            
            // Draw timer
            time.IncreaseTime(gameTime);

            
            

            //
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            //if (IsActive)
            //{
            //    // Apply some random jutter to make the enemy move around
            //}

            
            
           
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

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];


            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                flagTime = false;
            }
            else
            {
                // handle movement of enemy, player
            }
            ChangeNumber();
            base.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            spriteBatch = ScreenManager.SpriteBatch;
            
            spriteBatch.Begin();

            spriteBatch.Draw(gamescreenBG, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(m_timefont, time.getTime(), new Vector2(630, 318), Color.White);
            DrawMatrix();
            spriteBatch.DrawString(gameFont, "Mouse", v2, Color.White);
            
            spriteBatch.End();

            // If the game is transitioning on or Off, it out to black
            if (TransitionPosition > 0 || pauseAlpha >0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);

            }

            
            base.Draw(gameTime);
        }
        public void DrawMatrix()
        {

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (LockMatrixNumber[j, i] != 0)
                    {
                        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                        string a = LockMatrixNumber[j, i].ToString();
                        float x = (i * 60) + 50;
                        float y = (j * 60) + 30;
                        spriteBatch.DrawString(gameFont, a, new Vector2(x, y), Color.Red);
                    }

                    if (LockMatrixNumber[j, i] == 0 && Mapdemo.MatrixMap[j, i] != 0)
                    {
                        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                        string a = Mapdemo.MatrixMap[j, i].ToString();
                        float x = (i * 60) + 50;
                        float y = (j * 60) + 30;
                        spriteBatch.DrawString(gameFont, a, new Vector2(x, y), Color.White);
                    }




                }
            }


        }
        public void ChangeNumber()
        {
            float x = (v2.X - 30) / 60;
            float y = (v2.Y - 30) / 60;
            if (flagChangeNumber == true)
            {
                if (x >= 0 && x <= 9)
                {
                    if (y >= 0 && y <= 9)
                    {
                        if (LockMatrixNumber[(int)y, (int)x] == 0)
                        {
                            int numbercurrent = Mapdemo.MatrixMap[(int)y, (int)x];
                            if (numbercurrent == 9)
                            {
                                numbercurrent = 0;
                            }
                            Mapdemo.MatrixMap[(int)y, (int)x] = numbercurrent + 1;
                            flagChangeNumber = false;
                        }

                    }

                }
            }


        }
        #endregion
    }
}
