using System;
using System.Windows.Forms;

namespace Minesweeper
{
	class GameTimer
	{
		private System.Windows.Forms.Timer gTimer;
		public int gTimerCount;
		public event EventHandler<GameTimerEventArgs> gTimerTick;
		
		public GameTimer()
		{
			gTimer = new System.Windows.Forms.Timer();
			gTimer.Tick += GameTimerTick;
			gTimer.Interval = 1000;
			gTimerCount = 0;
		}
		
		public void GameTimerStart()
		{
			gTimerCount = 0;
			gTimer.Start();	
		}
		
		public void GameTimerStop()
		{
			gTimer.Stop();
		}
		
		private void GameTimerTick(object sender, EventArgs e)
		{
			gTimerCount++;
			
			GameTimerEventArgs tEA = new GameTimerEventArgs();
			tEA.CounterValue = gTimerCount;
			OnGameTimerTick(tEA);
			if(gTimerCount == 9999)
			{
				gTimer.Stop();
			}
		}
		
		protected virtual void OnGameTimerTick(GameTimerEventArgs e)
		{
			EventHandler<GameTimerEventArgs> handler = gTimerTick;
			if(handler != null)
			{
				handler(this, e);
			}
		}
		
		
	}
}