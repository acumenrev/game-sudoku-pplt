using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameStateManagement;

namespace SodukuWinphone
{
    class GameplayScreen:GameScreen
    {
        #region Fields
        //Texture2D
        Texture2D m_gameplayBG;

        //SpriteFont
        SpriteFont m_gameplayFont;

        //SpriteBacth
        SpriteBatch m_spriteBatch;

        //Demo String
        int DemoString = 0;

        // Matrix Lock and Result
        int[,] m_lockMatrixNumber = new int[9, 9];
        int[,] m_resultMatrixNumber = new int[9, 9];

        //Sudoku
        Sudoku.Sudoku m_sudoku;

        // Vector Change Number
        Vector2 m_v2=Vector2.Zero;
        #endregion
        #region Init
        //Contructor
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            // Accept what gesture?
            EnabledGestures = GestureType.Tap;

            //Sudoku & Load Map
            m_sudoku = new Sudoku.Sudoku();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_lockMatrixNumber[i, j] = m_sudoku.m_Sudoku[i, j];
                }

            }
        }

        public override void LoadContent()
        {
            //LoadConten Texture2D
            m_gameplayBG = Load<Texture2D>("gamescreenBG");
            //LoadConten Font
            m_gameplayFont = Load<SpriteFont>("GameFont");
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
            m_spriteBatch.DrawString(m_gameplayFont, DemoString.ToString(), Vector2.Zero, Color.White);
            DrawMatrix();
            m_spriteBatch.End();
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
                        spriteBatch.DrawString(m_gameplayFont, a, new Vector2(x, y), Color.Blue);
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


        }

#endregion
    
}
