using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SudokuWinphone.Sudoku
{
    class Sudoku
    {
        #region Fields

        Random m_rand = new Random();
        public static Vector3[,] m_v3;
        public int[,] m_Sudoku =
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
        public int[,] m_resultMatrix = new int[9, 9];
        private int[,] m_emptyMatrix = new int[9, 9];
        SudokuWinphone.Sudoku.Level m_level;
        private int[,] m_subSquare =
            new int[,]
			{
				{0,0,0,1,1,1,2,2,2},
				{0,0,0,1,1,1,2,2,2},
				{0,0,0,1,1,1,2,2,2},
				{3,3,3,4,4,4,5,5,5},
				{3,3,3,4,4,4,5,5,5},
				{3,3,3,4,4,4,5,5,5},
				{6,6,6,7,7,7,8,8,8},
				{6,6,6,7,7,7,8,8,8},
				{6,6,6,7,7,7,8,8,8}
		};

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Sudoku()
        {
            m_v3 = new Vector3[9, 9];
            CreateSubRegions();
            m_level = new SudokuWinphone.Sudoku.Level();
            CopyToV3();
            if (Solve())
            {
                ShowSolve();
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_resultMatrix[i, j] = m_Sudoku[i, j];
                }
            }
            Merge();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loai">Có thể truyền bất cứ số nào vào</param>
        public Sudoku(int loai)
        {
            m_v3 = new Vector3[9, 9];
            CopyToV3();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Check fields
        /// </summary>
        /// <param name="m"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void CheckField(int[] m, int i, int j)
        {
            int squareIndex = m_subSquare[i, j];
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (m_subSquare[x, y] == squareIndex)
                    {
                        m[(int)m_v3[x, y].Z] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Copy each cell's value from m_Sudoku to m_v3
        /// </summary>
        public void CopyToV3()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_v3[i, j].Z = m_Sudoku[i, j];
                }
            }
        }

        /// <summary>
        /// Check map
        /// </summary>
        /// <returns></returns>
        public bool FlagCheckMap()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_v3[i, j].Z != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if ((l != j) && (m_v3[i, l].Z == m_v3[i, j].Z))
                            {
                                //  Wrong row
                                return false;
                            }
                            if ((l != i) && (m_v3[l, j].Z == m_v3[i, j].Z))
                            {
                                // Wrong column
                                return false;
                            }
                        }
                        // defined-region with index
                        int squareIndex = m_subSquare[i, j];
                        for (int x = 0; x < 9; x++)
                        {
                            for (int y = 0; y < 9; y++)
                            {
                                if (m_subSquare[x, y] == squareIndex)
                                {
                                    if (((x != i) || (y != j)) && (m_v3[x, y].Z == m_v3[i, j].Z))
                                    {
                                        // Wrong region
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Check result
        /// </summary>
        /// <returns></returns>
        public bool FlagCheckResult()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_Sudoku[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_Sudoku[i, j] != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if ((l != j) && (m_Sudoku[i, l] == m_Sudoku[i, j]))
                            {
                                // Wrong row
                                return false;
                            }
                            if ((l != i) && (m_Sudoku[l, j] == m_Sudoku[i, j]))
                            {
                                // Wrong column
                                return false;
                            }
                        }
                        int squareIndex = m_subSquare[i, j];
                        for (int x = 0; x < 9; x++)
                        {
                            for (int y = 0; y < 9; y++)
                            {
                                if (m_subSquare[x, y] == squareIndex)
                                {
                                    if (((x != i) || (y != j))
                                        && (m_Sudoku[x, y] == m_Sudoku[i, j]))
                                    {
                                        // Wrong region
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Solve sudoku
        /// </summary>
        /// <returns></returns>
        public bool Solve()
        {
            int a = 0;
            int b = 0;
            int allElements = 10;
            int[] tempArray = null;
            if (!FlagCheckMap())
            {
                return false;
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //browse empty cells or discordant cells
                    if (m_v3[i, j].Z == 0)
                    {
                        int[] M = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                        // Remove M's cells that match with m_v3's cells
                        for (int k = 0; k < 9; k++)
                        {

                            M[(int)m_v3[i, k].Z] = 0;
                        }
                        for (int l = 0; l < 9; l++)
                        {
                            M[(int)m_v3[l, j].Z] = 0;
                        }
                        // Delete M's cell that matchs with m_v3's cell's position
                        CheckField(M, i, j);
                        // count unused elements
                        int unusedElements = 0;
                        for (int h = 1; h < 10; h++)
                        {
                            if (M[h] != 0)
                            {
                                unusedElements++;
                            }
                        }
                        // check other cases if there is one of them suit
                        if (unusedElements < allElements)
                        {
                            allElements = unusedElements;
                            tempArray = M;
                            a = i;
                            b = j;
                        }
                    }
                }
            }
            // if there are no numbers in m_v3  = 0
            if (allElements == 10)
            {
                return true;
            }
            // Do not have elements to use
            if (allElements == 0)
            {
                return false;
            }
            // Try other solutions to find the most suitable one
            for (int n = 1; n < 10; n++)
            {
                if (tempArray[n] != 0)
                {
                    m_v3[a, b].Z = (float)tempArray[n];
                    if (Solve())
                    {
                        return true;
                    }
                }
            }
            // Remove position if not suitable
            m_v3[a, b].Z = 0;
            return false;
        }

        /// <summary>
        /// Show the result of map after solved
        /// </summary>
        public void ShowSolve()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_Sudoku[i, j] = (int)m_v3[i, j].Z;
                }
            }
        }

        /// <summary>
        /// Merge m_EmptyMatrix with m_Sudoku. 
        /// If cells in m_EmptyMatrix have value = 0, 
        /// then m_Sudoku's cells in those position will have the same value
        /// </summary>
        private void Merge()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_level.EmptyMatrix[i, j] == 0)
                    {
                        m_Sudoku[i, j] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Gets number in suit conditions
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int GetCase(int n)
        {
            int no = 0;
            switch (n)
            {
                case 0:
                case 1:
                case 2:
                    no = 0;
                    break;
                case 3:
                case 4:
                case 5:
                    no = 3;
                    break;
                case 6:
                case 7:
                case 8:
                    no = 6;
                    break;
            }
            return no;
        }

        /// <summary>
        /// Initialize a random number in a region
        /// </summary>
        /// <param name="xStart">Startpoint X</param>
        /// <param name="xEnd">Endpoint X</param>
        /// <param name="yStart">Startpoint </param>
        /// <param name="yEnd">Endpoint Y</param>
        private void GenerateRegion(int xStart, int xEnd, int yStart, int yEnd)
        {
            int x = m_rand.Next(xStart, xEnd);
            int y = m_rand.Next(yStart, yEnd);
            int no = m_rand.Next(1, 10);
            m_Sudoku[x, y] = no;
        }

        /// <summary>
        /// Create 3 regions: top-left, middle, bottom-right
        /// </summary>
        private void CreateSubRegions()
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        // Get case for x
                        x = GetCase(i);
                        // Get case for y
                        y = GetCase(j);
                        // Generate a random number with random position in this region
                        GenerateRegion(x, x + 3, y, y + 3);
                        i = 3;
                        j = 3;
                    }
                    if (i == 3 && j == 3)
                    {
                        x = GetCase(i);
                        y = GetCase(j);
                        GenerateRegion(x, x + 3, y, y + 3);
                        i = 6;
                        j = 6;
                    }
                    if (i == 6 && j == 6)
                    {
                        x = GetCase(i);
                        y = GetCase(j);
                        GenerateRegion(x, x + 3, y, y + 3);
                    }
                }
            }
        }
        #endregion
    }
}
