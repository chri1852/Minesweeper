using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Resources;

namespace Minesweeper
{
	class GameGUI
	{
		private Form GameForm;
		private int pixelHeight;
		private int pixelWidth;
		private Board GameData;
		private GameTimer ScoreTimer;
		private OptionsMenu OPsMenu;
		private AboutMenu AMenu;
		private GameStatisticsManager GSManager;
		
		public GameGUI() : this(9,9) {}
		public GameGUI(int cellHeight, int cellWidth)
		{
			pixelHeight = (25*cellHeight)+85;
			pixelWidth = (25*cellWidth)+10;
			GameData = new Board(cellHeight, cellWidth);
			GameData.initializeBoard();
			GameData.gOverEvent += GameData_GameOverEvent;
			
			GameForm = new Form();
			GameForm.MaximizeBox = false;
			GameForm.KeyPreview = true;
			GameForm.FormBorderStyle = FormBorderStyle.FixedDialog;
			GameForm.Name = "GameForm";
			GameForm.Text = "Minesweeper";
			GameForm.ClientSize = new Size(pixelWidth, pixelHeight);
			GameForm.KeyPress += new KeyPressEventHandler(MainFormKeyPressAction);
			GameForm.Icon = GameResources.GetIconImage();
			GameForm.FormClosed += GameForm_Closing;
			
			// Button not shown on screen, used to steal the focus away from other things
			Button focusStealingButton = new Button();
			focusStealingButton.Size = new Size(25, 25);
			focusStealingButton.Location = new Point(-100, -100);
			focusStealingButton.Name = "focusStealingButton";
			GameForm.Controls.Add(focusStealingButton);
			
			ScoreTimer = new GameTimer();
			ScoreTimer.gTimerTick += ScoreTimer_gTimerTick;
			
			OPsMenu = new OptionsMenu();
			OPsMenu.OptionsReturn += OptionsMenu_SetOptions;
			
			GSManager = new GameStatisticsManager();
			
			AMenu = new AboutMenu();

			AddToolBar();
		}
		
		private void GameForm_Closing(object sender, EventArgs e)
		{
			GSManager.SaveStats();
		}
		
		private void ScoreTimer_gTimerTick(object sender, GameTimerEventArgs e)
		{
			GameForm.Controls.Find("TimerTextBox", true)[0].Text = e.CounterValue.ToString("D" + 4);
		}
		
		public void StartForm()
		{
			AddHeaderObjects();
			setFormBoard();
			GameForm.ShowDialog();
		}
		
		private void GameData_GameOverEvent(object sender, GameOverEventArgs e)
		{
			ScoreTimer.GameTimerStop();
			if(!e.isWinner)
			{
				RevealAllMines();
				GameForm.Controls.Find("ResetButton", true)[0].Text = "X";
				GameForm.Controls.Find("ResetButton", true)[0].ForeColor = Color.Red;
			}
			else
			{
				int score = Int32.Parse(GameForm.Controls.Find("TimerTextBox",true)[0].Text);
				GameForm.Controls.Find("ResetButton", true)[0].Text = "W";
				GameForm.Controls.Find("ResetButton", true)[0].ForeColor = Color.Green;
				if(GameData.isEasy() && GSManager.IsEasyHighScore(score))
				{
					GSManager.LoadEasyValue(score, GSManager.GetUserName());
				}
				else if(GameData.isMedium() && GSManager.IsMediumHighScore(score))
				{
					GSManager.LoadMediumValue(score, GSManager.GetUserName());
				}
				else if(GameData.isHard() && GSManager.IsHardHighScore(score))
				{
					GSManager.LoadHardValue(score, GSManager.GetUserName());
				}
			}

			ScoreTimer.GameTimerStop();
		}
		
		private void RevealAllMines()
		{
			for(int hgt = 0; hgt < GameData.Height; hgt++)
			{
				for(int wth = 0; wth < GameData.Width; wth++)
				{
					if(GameData.GetIsMine(wth, hgt) && GameData.GetCellHidden(wth, hgt))
					{
						Label btn = GameForm.Controls.Find(string.Format("mineButtonX{0}Y{1}", wth, hgt), true)[0] as Label;
						GameForm.Controls.Remove(btn);
					}
				}
			}
		}
		
		private void AddHeaderObjects()
		{
			// Total width 170 +10 for side buffer
			int offset = (pixelWidth-158)/2;
			TextBox TimerTextBox = new TextBox();
			TimerTextBox.Size = new Size(53, 45);
			TimerTextBox.Location = new Point(offset, 38);
			TimerTextBox.BackColor = Color.Black;
			TimerTextBox.ForeColor = ColorTranslator.FromHtml("#39FF14");
			TimerTextBox.Name = "TimerTextBox";
			TimerTextBox.Font = new Font("Consolas", 18, FontStyle.Bold);
			TimerTextBox.Text = "9999";
			TimerTextBox.ReadOnly = true;
			TimerTextBox.BorderStyle = BorderStyle.None;
			GameForm.Controls.Add(TimerTextBox);
			
			TextBox ScoreTextBox = new TextBox();
			ScoreTextBox.Size = new Size(40, 45);
			ScoreTextBox.Location = new Point(offset + 118, 38);
			ScoreTextBox.BackColor = Color.Black;
			ScoreTextBox.ForeColor = ColorTranslator.FromHtml("#39FF14");
			ScoreTextBox.Name = "ScoreTextBox";
			ScoreTextBox.Font = new Font("Consolas", 18, FontStyle.Bold);
			ScoreTextBox.Text = "999";
			ScoreTextBox.ReadOnly = true;
			ScoreTextBox.BorderStyle = BorderStyle.None;
			GameForm.Controls.Add(ScoreTextBox);
			
			NoFocusButton ResetButton = new NoFocusButton();
			ResetButton.Size = new Size(45, 35);
			ResetButton.Location = new Point(offset + 63, 35);
			ResetButton.Name = "ResetButton";
			ResetButton.TextAlign = ContentAlignment.MiddleCenter;
			ResetButton.Font = new Font(ResetButton.Font.FontFamily, Single.Parse("18"), FontStyle.Bold);
			ResetButton.Click += DefaultBoardReset;
			ResetButton.FlatStyle = FlatStyle.Flat;
			GameForm.Controls.Add(ResetButton);
		}
	
		
		private void AddToolBar()
		{	
			ToolStripMenuItem gameMenuItem = new ToolStripMenuItem();
			gameMenuItem.Text = "Game";
			gameMenuItem.Name = "gameMenuItem";
			gameMenuItem.DropDownItems.Add("&New");
			gameMenuItem.DropDownItems.Add("&About");
			gameMenuItem.DropDownItems.Add(new ToolStripSeparator());
			gameMenuItem.DropDownItems.Add("&Options");
			gameMenuItem.DropDownItems.Add("&Statistics");
			gameMenuItem.DropDownItems.Add(new ToolStripSeparator());
			gameMenuItem.DropDownItems.Add("&Exit");
			gameMenuItem.DropDown.ItemClicked += new ToolStripItemClickedEventHandler(GameMenu_ItemClicked);
			
			//ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem();
			//aboutMenuItem.Text = "About";
			//aboutMenuItem.Name = "helpMenuItem";
			//aboutMenuItem.DropDownItems.Add("About");
			//aboutMenuItem.DropDown.ItemClicked += new ToolStripItemClickedEventHandler(GameMenu_ItemClicked);
			
			MenuStrip mainMenuStrip = new MenuStrip();
			mainMenuStrip.Text = "Menu";
			mainMenuStrip.Items.Add(gameMenuItem);
			//mainMenuStrip.Items.Add(helpMenuItem);
			mainMenuStrip.Height = 25;
			
			GameForm.MainMenuStrip = mainMenuStrip;
			GameForm.Controls.Add(mainMenuStrip);
			
		}
		
		private void DefaultBoardReset(object sender, EventArgs e)
		{
			ResetBoard(GameData.Height, GameData.Width, GameData.NumberOfMines);
		}
		
		private void GameMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			switch(e.ClickedItem.Text)
			{
				case "&New"  		:   {
											DefaultBoardReset(this, EventArgs.Empty);
											break;
										}
										
				case "&Options"		:	{
											OPsMenu.RunOptionsMenu(GameData.Height, GameData.Width, GameData.NumberOfMines);
											break;
										}
										
				case "&Statistics"	:	{
											GSManager.StatisticsMenuRun();
											break;
										}
										
				case "&Exit"			:   {
											GameForm.Close();
											break;
										}
										
				case "&About"		:	{
											AMenu.AboutMenuRun();
											break;
										}
										
				default				:	{
											break;
										}
			}
		}
		
		// Sets the key presses
		private void MainFormKeyPressAction(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case (char)'n': {
									DefaultBoardReset(this, EventArgs.Empty);
									break;
								}
								
				case (char)'o': {
									OPsMenu.RunOptionsMenu(GameData.Height, GameData.Width, GameData.NumberOfMines);	
									break;
								}
								
				case (char)'s': {
									GSManager.StatisticsMenuRun();
									break;
								}
								
				case (char)'e': {
									GameForm.Close();
									break;
								}
								
				case (char)'a': {
									AMenu.AboutMenuRun();
									break;
								}
								
				case (char)27 : {
									GameForm.Close();
									break;
								}
			}
		}
		
		private void OptionsMenu_SetOptions(object sender, OptionMenuEventArgs e)
		{
			if(((OptionsMenuData)e.Data).Height != GameData.Height || ((OptionsMenuData)e.Data).Width != GameData.Width || ((OptionsMenuData)e.Data).NumberOfMines != GameData.NumberOfMines)
			{
				ResetBoard(((OptionsMenuData)e.Data).Height, ((OptionsMenuData)e.Data).Width, ((OptionsMenuData)e.Data).NumberOfMines);
			}
		}
		
		// Sets the mine labels and buttons
		private void setFormBoard()
		{	
			for(int hgt = 0; hgt < GameData.Height; hgt++)
			{
				for(int wth = 0; wth < GameData.Width; wth++)
				{
					addMineButton(wth, hgt);
					addMineLabel(wth, hgt);
				}
			}
			GameForm.Controls.Find("ScoreTextBox", true)[0].Text = ((int)GameData.NumberOfMines).ToString("D" + 3);
			GameForm.Controls.Find("TimerTextBox", true)[0].Text = "0000";
			GameForm.Controls.Find("ResetButton", true)[0].Text = "O";
			GameForm.Controls.Find("ResetButton", true)[0].ForeColor = Color.Blue;
			ScoreTimer.GameTimerStart();
		}
		
		// adds a mine label at x,y underneath the buttons
		private void addMineLabel(int x, int y)
		{		
			Label mineLabel = new Label();
			mineLabel.Name = string.Format("mineLabelX{0}Y{1}", x, y);
			mineLabel.Font = new Font(mineLabel.Font, FontStyle.Bold);
			mineLabel.BorderStyle = BorderStyle.FixedSingle;
			mineLabel.TextAlign = ContentAlignment.MiddleCenter;
			mineLabel.Size = new Size(25,25);
			mineLabel.Location = new Point((25*x)+5, (25*y)+80);
			mineLabel.Tag = new Tuple<int, int>(x, y);
			mineLabel.DoubleClick += NumberDoubleClick;

			GameForm.Controls.Add(mineLabel);
		}
		
		private void setMineLabelValues()
		{
			for(int y = 0; y < GameData.Height; y++)
			{
				for(int x = 0; x < GameData.Width; x++)
				{
					Label mineLabel = GameForm.Controls.Find(string.Format("mineLabelX{0}Y{1}", x, y), true)[0] as Label;
					if(GameData.GetCellValue(x, y) == 9)
					{
						mineLabel.Text = "X";
						mineLabel.ForeColor = GameData.GetCellColor(x,y);
						mineLabel.BackColor = Color.LightGray;
					}
					else if(GameData.GetCellValue(x, y) == 0)
					{
						mineLabel.Text = "";
					}
					else
					{
						mineLabel.Text = GameData.GetCellValue(x,y).ToString();
						mineLabel.ForeColor = GameData.GetCellColor(x,y);
					}
				}
			}
		}
		
		// adds the button on top of the label
		private void addMineButton(int x, int y)
		{
			Label mineButton = new Label();
			mineButton.Name = string.Format("mineButtonX{0}Y{1}", x, y);
			mineButton.TextAlign = ContentAlignment.MiddleCenter;
			mineButton.Size = new Size(25, 25);
			mineButton.Location = new Point((25*x)+5, (25*y)+80);
			mineButton.MouseDown += new MouseEventHandler(RemoveMineButton);
			mineButton.BackColor = Color.Blue;
			mineButton.Tag = new Tuple<int, int>(x, y);
			mineButton.BorderStyle = BorderStyle.FixedSingle;
			mineButton.ForeColor = Color.White;
			mineButton.Font = new Font(mineButton.Font, FontStyle.Bold);
			GameForm.Controls.Add(mineButton);
		}
		
		// Called when the under lying label is double clicked
		private void NumberDoubleClick(object sender, EventArgs e)
		{
			// Read the X and Y values
			Label lbl = sender as Label;
			int x = ((Tuple<int, int>)lbl.Tag).Item1;
			int y = ((Tuple<int, int>)lbl.Tag).Item2;
			
			if(!GameData.GetIsMine(x, y) && (GameData.GetFlaggedNearBy(x, y) == GameData.GetCellValue(x, y)))
			{
				UnHideTile(x+1, y+1);
				UnHideTile(x+1, y);
				UnHideTile(x+1, y-1);
				UnHideTile(x, y+1);
				UnHideTile(x, y-1);
				UnHideTile(x-1, y+1);
				UnHideTile(x-1, y);
				UnHideTile(x-1, y-1);
				
				GameData.TestGameOver();
			}
		}
		
		private void UnHideTile(int x, int y)
		{
			if(!GameData.GetCellFlagged(x, y) && GameData.SetAsUnHidden(x, y))
			{
				Label btn = GameForm.Controls.Find(string.Format("mineButtonX{0}Y{1}", x, y), true)[0] as Label;
				GameForm.Controls.Remove(btn);
				if(GameData.GetCellValue(x, y) == 0)
				{
					ClearSurroundingButtons(x, y);
				}
			}
		}
		
		private void ResetBoard(int height, int width, int numMin)
		{
			// Remove the old board
			for(int hgt = 0; hgt < GameData.Height; hgt++)
			{
				for(int wth = 0; wth < GameData.Width; wth++)
				{
					try
					{
						GameForm.Controls.Remove(GameForm.Controls.Find(string.Format("mineButtonX{0}Y{1}",wth,hgt), true)[0]);
					}
					catch
					{
						// Do Nothing
					}
					GameForm.Controls.Remove(GameForm.Controls.Find(string.Format("mineLabelX{0}Y{1}",wth,hgt), true)[0]);
					
				}
			}
			
			//Stop the score timer
			ScoreTimer.GameTimerStop();
			
			// Set the new
			GameData = new Board(height, width, numMin);
			GameData.initializeBoard();
			pixelHeight = (25*GameData.Height)+85;
			pixelWidth = (25*GameData.Width)+10;
			GameForm.ClientSize = new Size(pixelWidth, pixelHeight);
			GameData.gOverEvent += GameData_GameOverEvent;
			
			// Move the Header
			int offset = (pixelWidth-158)/2;
			GameForm.Controls.Find("TimerTextBox", true)[0].Location = new Point(offset, 38);
			GameForm.Controls.Find("ScoreTextBox", true)[0].Location = new Point(offset + 118, 38);
			GameForm.Controls.Find("ResetButton", true)[0].Location = new Point(offset + 63, 35);
			
			setFormBoard();
		}
		
		
		
		// called when a mineButton is clicked
		private void RemoveMineButton(object sender, MouseEventArgs e)
		{
			// If Game over return
			if(GameData.isGameOver)
			{
				return;
			}
			// Read the X and Y values
			Label btn = sender as Label;
			int x = ((Tuple<int, int>)btn.Tag).Item1;
			int y = ((Tuple<int, int>)btn.Tag).Item2;
			
			// when removing a tile
			if(e.Button == MouseButtons.Left && !GameData.GetCellFlagged(x, y))
			{
				if(GameData.CheckFirstMove(x, y))
				{
					setMineLabelValues();
				}
				
				GameForm.Controls.Remove(sender as Label);	
				GameData.SetCellHidded(x, y, false);
				if(GameData.GetCellValue(x, y) == 0)
				{
					ClearSurroundingButtons(x, y);
				}
				GameData.TestGameOver();
			}
			
			// when flagging
			if(e.Button == MouseButtons.Right)
			{
				if(!GameData.GetCellFlagged(x, y) && !GameData.GetCellQuestion(x, y) && (GameForm.Controls.Find("ScoreTextBox", true)[0].Text != "000"))
				{
					GameData.SetCellFlagged(x, y, true);
					GameData.SetCellQuestion(x, y, false);
					(sender as Label).Text = "F";
					int numFlagged = GameData.GetTotalFlaggedTiles();
					GameForm.Controls.Find("ScoreTextBox", true)[0].Text = ((int)GameData.NumberOfMines-numFlagged).ToString("D" + 3);
				}
				else if(!GameData.GetCellQuestion(x, y))
				{
					GameData.SetCellFlagged(x, y, false);
					GameData.SetCellQuestion(x, y, true);
					(sender as Label).Text = "?";
					int numFlagged = GameData.GetTotalFlaggedTiles();
					GameForm.Controls.Find("ScoreTextBox", true)[0].Text = ((int)GameData.NumberOfMines-numFlagged).ToString("D" + 3);
				}
				else if(!GameData.GetCellFlagged(x, y) && GameData.GetCellQuestion(x, y))
				{
					GameData.SetCellFlagged(x, y, false);
					GameData.SetCellQuestion(x, y, false);
					(sender as Label).Text = "";
				}
			}			
		}
		
		private void ClearSurroundingButtons(int x, int y)
		{
			ClearSurroundingButtonsHelper(x+1, y+1);
			ClearSurroundingButtonsHelper(x+1, y);
			ClearSurroundingButtonsHelper(x+1, y-1);
			ClearSurroundingButtonsHelper(x, y+1);
			ClearSurroundingButtonsHelper(x, y-1);
			ClearSurroundingButtonsHelper(x-1, y+1);
			ClearSurroundingButtonsHelper(x-1, y);
			ClearSurroundingButtonsHelper(x-1, y-1);
		}
		
		private void ClearSurroundingButtonsHelper(int x, int y)
		{
			if(GameData.SetAsUnHidden(x, y)){
				Label btn = GameForm.Controls.Find(string.Format("mineButtonX{0}Y{1}", x, y), true)[0] as Label;
				GameForm.Controls.Remove(btn);
				if(GameData.GetCellValue(x, y) == 0)
				{	
					ClearSurroundingButtons(x, y);
				}
			}
		}
		
	}
}