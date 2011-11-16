﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Sudoku.Map
{
    class Level
    {
        #region Fields

        private static string[,] m_emptyMatrix = new string[9, 9];

        public string[,] EmptyMatrix
        {
            get 
            { 
                return m_emptyMatrix;
            }
            
        }


        
        private static Random m_rand = new Random();

        #endregion

        #region Methods & Constructor
       

        public int levelstatus;
        /// <summary>
        /// Initialize empty matrix
        /// </summary>
        public Level()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_emptyMatrix[i, j] = "";
                }
            }
        }

        /// <summary>
        /// Taking out cells from matrix (left empty)
        /// </summary>
        /// <param name="level">Game level</param>
        private static void FillEmpty(int level)
        {
            // quantity of empty cells
            int qCells;
            switch(level)
            {
                // Easy
                // 40 -> 45 empty cells
                case 0:
                    
                    qCells = m_rand.Next(40, 46);
                    MakingEmptyCells(qCells);
                    break;
                // Medium
                // 46 -> 49 empty cells
                case 1:
                    qCells = m_rand.Next(46, 50);
                    MakingEmptyCells(qCells);
                    break;
                // Hard
                // 50 -> 54 empty cells
                case 2:
                    qCells = m_rand.Next(50, 55);
                    MakingEmptyCells(qCells);
                    break;
            }
        }

        /// <summary>
        /// Fill empty cells with 0
        /// </summary>
        /// <param name="quantityOfCells"> Cells need to be left empty</param>
        private static void MakingEmptyCells(int quantityOfEmptyCells)
        {
            for (int i = 0; i < quantityOfEmptyCells; i++)
            {
                // check if cells already selected
                bool isDuplicate = false;
                do
                {
                    int x = m_rand.Next(0, 9);
                    int y = m_rand.Next(0, 9);
                    if (m_emptyMatrix[x, y] == "0")
                    {
                        isDuplicate = true;
                    }
                    else
                    {
                        m_emptyMatrix[x, y] = "0";
                        isDuplicate = false;
                    }
                    
                } while (isDuplicate == true);
            }
        }

        #endregion

    }
}
