using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Game_Sudoku.Map
{
    class Map
    {

        public int[,] m_matrixMap = new int[9, 9];
                         
        private static Random m_rand = new Random();
        public Map()
        {
            
        }

    }
}
