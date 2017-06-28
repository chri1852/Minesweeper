using System;

namespace Minesweeper
{
	public class OptionsMenuData
	{
		public int Width;
		public int Height;
		public int NumberOfMines;
		
		public OptionsMenuData(int w, int h, int n)
		{
			Width = w;
			Height = h;
			NumberOfMines = n;
		}
		
		public bool isEasy()
		{
			if(Width == 9 && Height == 9 && NumberOfMines == 10)
			{
				return true;
			}
			
			return false;
		}
		
		public bool isMedium()
		{
			if(Width == 16 && Height == 16 && NumberOfMines == 40)
			{
				return true;
			}
			
			return false;
		}
		
		public bool isHard()
		{
			if(Width == 30 && Height == 16 && NumberOfMines == 99)
			{
				return true;
			}
			
			return false;
		}
	}
}