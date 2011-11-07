using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Sudoku.Map
{
    class Map
    {
        public int[,] MatrixMap = 
            new int[9, 9] {
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,3,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,1,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0}
                          };

    }
}
