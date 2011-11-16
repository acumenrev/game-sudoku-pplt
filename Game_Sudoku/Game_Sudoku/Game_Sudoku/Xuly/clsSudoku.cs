using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_Sudoku.Xuly
{
    class clsSudoku
    {
        static Random rand;
        public static Vector3[,] v3;
        public float[,] Mapsolve = new float[9, 9];
        Map.Map mapdemo;

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

        public clsSudoku()
        {
            v3 = new Vector3[9, 9];
            mapdemo = new Map.Map();
            rand = new Random();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    v3[i, j].Z =mapdemo.MatrixMap[i,j];
                }
            }
         
            
        }

        public void checkfield(int[] m, int i, int j)
        {
            int squareIndex = m_subSquare[i, j];   // mien xac dinh theo index
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (m_subSquare[x, y] == squareIndex)
                    {
                        m[(int)v3[x, y].Z] = 0;
                    }
                }

            }
            /*int xvitrixet;
            int yvitrixet;
            int xsodu = (j + 1) % 3;
            int ysodu = (i + 1) % 3;
            

            if (xsodu == 0)
            {
                xvitrixet = j - 2;
            }
            else
                xvitrixet = j + 1 - xsodu;

            if (ysodu == 0)
            {
                yvitrixet = i - 2;
            }
            else
                yvitrixet = i + 1 - ysodu;

            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    m[(int)v3[xvitrixet + x, yvitrixet + y].Z] = 0;  
                }
            }*/
        }

        public bool solve()
        {
            int a = 0;
            int b = 0;
            int tongsophantu = 10;
            int[] daysotam = null;


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (v3[i, j].Z == 0)             // duyet nhung o trong' hoac. o chua phu hop 
                    {
                        int[] M = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                        // xoa nhung so' trong M ma` trung` voi'cac hang , cot trong v3
                        for (int k = 0; k < 9; k++)
                        {
                            
                            M[(int)v3[i, k].Z] = 0;
                        }

                        for (int l = 0; l < 9; l++)
                        {

                            M[(int)v3[l, j].Z] = 0;
                        }
                        // xoa nhung so trong M ma` trung` voi mien vi tri dang xet trong v3
                        checkfield(M, i, j);

                        // dem so phan tu chua su dung
                        int sophantuchuasudung = 0;

                        for (int h = 1; h < 10; h++)
                        {
                            if (M[h] != 0)
                            {
                                sophantuchuasudung++;
                            }
                        }
                        // thong so de xet them trung hop khac xem co phu hop hon ko
                        if (sophantuchuasudung < tongsophantu)
                        {
                            tongsophantu = sophantuchuasudung;
                            daysotam = M;
                            a = i;
                            b = j;
                        }
                    }
                }
            }
            // khi ko con so nao trong v3 == 0
            if (tongsophantu == 10)
            {
                return true;
            }
            //ko co phan tu de su dung
            if (tongsophantu == 0)
            {
                return false;
            }

            //thu voi cac phuong an' khac' de tim cai phu hop nhat

            for (int n = 1; n < 10; n++)
            {
                if (daysotam[n] != 0)
                {
                    v3[a, b].Z = (float)daysotam[n];

                    if (solve())
                    {
                        return true;
                    }
                }
            }

            //xoa vi tri neu ko phu hop
            v3[a, b].Z = 0;
            return false;
        }
        public void showsolve()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Mapsolve[i, j] = v3[i, j].Z;
                }
            }
        }

        public void printArr()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write("  ");
                    Console.Write(v3[i, j].Z);

                }
                Console.WriteLine();
            }
        }
        
    }
}
