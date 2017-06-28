using System;
using System.Drawing;

namespace Minesweeper
{
	class Cell
	{
		public bool isHidden { get; set; }
		public bool isFlagged { get; set; }
		public bool isQuestion { get; set; }
		
		private bool _isMine;
		public bool isMine
		{
			get
			{
				return this._isMine;
			}
			set
			{
				if(value)
				{
					this._isMine = true;
					this._mineValue = 9;
				}
				else
				{
					this._isMine = false;
					this._mineValue = 0;
				}
			}
		}
		
		private int _mineValue;
		public int MineValue 
		{
			get
			{
				return this._mineValue;
			}
			set
			{
				if(this.isMine)
				{
					this._mineValue = 9;
				}
				else
				{
					this._mineValue = value;
				}
			}
		}
		
		public Cell() : this(false) {}
		public Cell(bool mine)
		{
			isHidden = true;
			isFlagged = false;
			isQuestion = false;
			isMine = mine;
			MineValue = 0;
		}
		
		public Color getColor()
		{
			switch(MineValue)
			{
				case 1  : { return Color.Blue; }
				case 2  : { return Color.Green; }
				case 3  : { return Color.Red; }
				case 4  : { return Color.Purple; }
				case 5  : { return Color.Black; }
				case 6  : { return Color.Maroon; }
				case 7  : { return Color.Gray; }
				case 8  : { return Color.Turquoise; }
				case 9  : { return Color.DarkSlateGray;}
				default : { return Color.White; }
			}
		}
	}
}