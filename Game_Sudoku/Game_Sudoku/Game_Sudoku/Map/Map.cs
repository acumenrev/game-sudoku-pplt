using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Sudoku.Map
{
    class Map
    {
        public float[,] MatrixMap = 
            new float[9, 9] {
                            {4,0,0,8,0,7,0,0,2},
                            {0,0,2,4,0,5,9,0,0},
                            {0,0,8,0,6,0,7,0,0},
                            {0,8,0,0,0,0,0,1,0},
                            {2,0,0,0,0,0,0,0,7},
                            {0,1,0,0,8,0,0,5,0},
                            {0,0,3,0,4,0,6,0,0},
                            {0,0,9,6,0,3,1,0,0},
                            {6,0,0,2,0,8,0,0,5}
                          };

    }
}
