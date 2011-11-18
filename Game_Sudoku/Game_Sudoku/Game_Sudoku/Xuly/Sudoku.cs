using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_Sudoku.Xuly
{
	class Sudoku
	{
		Random m_rand = new Random();
		public static Vector3[,] v3;
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
		
		
		Map.Level m_level;

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

		public Sudoku()
		{
			v3 = new Vector3[9, 9];
			// tao ra 3 so ngau nhien trong 3 mien TopLeft, Middle, BottomRight
			CreateSubRegions();
			// tao ra ma tran co o trong
			m_level = new Map.Level();


			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					v3[i, j].Z = m_Sudoku[i, j];
				}
			}

			// Giai o so da cho ben tren
			if (Solve())
			{
				// Gan cac gia tri tu v3 sang cho m_Sudoku
				ShowSolve();
			}
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					m_resultMatrix[i,j] = m_Sudoku[i,j];
				}
			}
			
			// gan cac phan tu ngau nhien trong m_emptyMatrix bang 0
			// cho cac so ngau nhien bang 0
			Merge();
			
		 
		}

	

		public void CheckField(int[] m, int i, int j)
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
			
		}

		// Thêm phần checkmap ban đâu vào 
		public bool checkmap()
		{
            
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (v3[i,j].Z != 0)
					{
						for (int l = 0; l < 9; l++)
						{
                            if ((l != j) && (v3[i,l].Z == v3[i, j].Z))
							{
								//                                 Console.Write("sai hang");
								//                                                               Console.WriteLine();
								return false;
							}

                            if ((l != i) && (v3[l,j].Z == v3[i,j].Z))
							{
								//                                 Console.Write("sai cot");
								//                                                             Console.WriteLine();
								return false;
							}
						}

						int squareIndex = m_subSquare[i, j];   // mien xac dinh theo index
						for (int x = 0; x < 9; x++)
						{
							for (int y = 0; y < 9; y++)
							{
								if (m_subSquare[x, y] == squareIndex)
								{
                                    if (((x != i) || (y != j)) && (v3[x,y].Z == v3[i,j].Z))
									{
										//                                         Console.Write("sai mien");
										//                                                                                     Console.WriteLine();
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
        //
        public bool checkketqua()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_Sudoku[i,j]==0)
                    {
                        return false;
                    }
                }

            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_Sudoku[i,j] != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if ((l != j) && (m_Sudoku[i, l ]==m_Sudoku[i,j]))
                            {
                                //Console.Write("sai hang");
                                //Console.WriteLine();
                                return false;
                            }

                            if ((l != i) && (m_Sudoku[l,j] == m_Sudoku[i,j]))
                            {
                                //Console.Write("sai cot");
                                //Console.WriteLine();
                                return false;
                            }
                        }

                        int squareIndex = m_subSquare[i, j];   // mien xac dinh theo index
                        for (int x = 0; x < 9; x++)
                        {
                            for (int y = 0; y < 9; y++)
                            {
                                if (m_subSquare[x, y] == squareIndex)
                                {
                                    if (((x != i) || (y != j)) && (m_Sudoku[x,y] ==m_Sudoku[i,j]))
                                    {
                                        //Console.Write("sai mien");
                                        //Console.WriteLine();
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
        //
		   // ket thuc sua

		public bool Solve()
		{
            
			int a = 0;
			int b = 0;
			int tongSoPhanTu = 10;
			int[] daySoTam = null;

			if (!checkmap())
			{
				return false;
			}

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
						CheckField(M, i, j);

						// dem so phan tu chua su dung
						int soPhanTuChuaSuDung = 0;

						for (int h = 1; h < 10; h++)
						{
							if (M[h] != 0)
							{
								soPhanTuChuaSuDung++;
							}
						}
						// thong so de xet them trung hop khac xem co phu hop hon ko
						if (soPhanTuChuaSuDung < tongSoPhanTu)
						{
							tongSoPhanTu = soPhanTuChuaSuDung;
							daySoTam = M;
							a = i;
							b = j;
						}
					}
				}
			}
			// khi ko con so nao trong v3 == 0
			if (tongSoPhanTu == 10)
			{
				return true;
			}
			//ko co phan tu de su dung
			if (tongSoPhanTu == 0)
			{
				return false;
			}

			//thu voi cac phuong an' khac' de tim cai phu hop nhat

			for (int n = 1; n < 10; n++)
			{
				if (daySoTam[n] != 0)
				{
					v3[a, b].Z = (float)daySoTam[n];

					if (Solve())
					{
						return true;
					}
				}
			}

			//xoa vi tri neu ko phu hop
			v3[a, b].Z = 0;
			return false;
		}
		public void ShowSolve()
		{
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					m_Sudoku[i, j] = (int)v3[i, j].Z;
					
				}
			}
		}

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
			switch(n)
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
		private void GenerateRegion(int xStart,int xEnd,int yStart,int yEnd)
		{
			int x = m_rand.Next(xStart,xEnd);
			int y = m_rand.Next(yStart,yEnd);
			int no = m_rand.Next(1,10);
			m_Sudoku[x,y] = no;
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
					if (i == 0 && j==0)
					{
						x = GetCase(i);
						y = GetCase(j);
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
	}
}
