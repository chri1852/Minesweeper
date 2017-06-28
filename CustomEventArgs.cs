using System;

namespace Minesweeper
{
	class GameTimerEventArgs : EventArgs
	{
		public int CounterValue { get; set; }
	}
	
	class GameOverEventArgs : EventArgs
	{
		public bool isGameOver { get; set; }
		public bool isWinner   { get; set; }
	}
	
	class OptionMenuEventArgs : EventArgs
	{
		public OptionsMenuData Data { get; set; }
	}
}