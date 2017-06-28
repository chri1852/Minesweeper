using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
	public class GameStatisticsManager
	{
		private GameStatisticData Easy;
		private GameStatisticData Medium;
		private GameStatisticData Hard;
		private BinaryWriter FileWriter;
		private BinaryReader FileReader;
		private Form StatisticsMenu;
		private Form NameMenu;
		
		public GameStatisticsManager()
		{
			Easy = new GameStatisticData("Easy");
			Medium = new GameStatisticData("Medium");
			Hard = new GameStatisticData("Hard");
			
			LoadGameStatsFromFile();
		}
		
		private void LoadGameStatsFromFile()
		{
			if(File.Exists(GameResources.GetGameStatsFile()))
			{
				using(FileReader = new BinaryReader(File.Open(GameResources.GetGameStatsFile(), FileMode.Open)))
				{
					for(int i = 0; i < 9; i++)
					{
						if(i >= 0 && i <= 2)
						{
							Easy.LoadScore(FileReader.ReadInt32(), FileReader.ReadString());
						}
						else if(i >= 3 && i <= 5)
						{
							Medium.LoadScore(FileReader.ReadInt32(), FileReader.ReadString());
						}
						else if(i >= 6 && i <= 8)
						{
							Hard.LoadScore(FileReader.ReadInt32(), FileReader.ReadString());
						}
					}
				}
			}
			else
			{
				WriteGameStatsToFile();
				LoadGameStatsFromFile();
			}
		}
		
		private void WriteGameStatsToFile()
		{
			if(File.Exists(GameResources.GetGameStatsFile()))
			{
				File.Delete(GameResources.GetGameStatsFile());
				using(FileWriter = new BinaryWriter(File.Open(GameResources.GetGameStatsFile(), FileMode.Create)))
				{
					// Easy
					foreach(Tuple<int, string> item in Easy)
					{
						FileWriter.Write(item.Item1);
						FileWriter.Write(item.Item2);
					}

					// Medium
					foreach(Tuple<int, string> item in Medium)
					{
						FileWriter.Write(item.Item1);
						FileWriter.Write(item.Item2);
					}
					
					// Hard
					foreach(Tuple<int, string> item in Hard)
					{
						FileWriter.Write(item.Item1);
						FileWriter.Write(item.Item2);
					}
				}
			}
			else
			{
				Directory.CreateDirectory(GameResources.GetResourceDirectory());
				using(FileWriter = new BinaryWriter(File.Open(GameResources.GetGameStatsFile(), FileMode.Create)))
				{
					//Easy
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					//Medium
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					//Hard
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
					FileWriter.Write(9999);
					FileWriter.Write("NOPLAYER....9999");
				}
			}
		}
		
		private void BuildStatisticsMenu()
		{
			StatisticsMenu = new Form();
			StatisticsMenu.MaximizeBox = false;
			StatisticsMenu.FormBorderStyle = FormBorderStyle.FixedDialog;
			StatisticsMenu.Name = "StatisticsMenu";
			StatisticsMenu.Text = "Minesweeper";
			StatisticsMenu.ClientSize = new Size(200, 315);
			StatisticsMenu.KeyPreview = true;
			StatisticsMenu.KeyPress += new KeyPressEventHandler(StatisticsMenuClose);
			StatisticsMenu.Icon = GameResources.GetIconImage();
			
			Label StatsLabel = new Label();
			StatsLabel.Font = new Font(StatsLabel.Font.FontFamily, Single.Parse("14"), FontStyle.Bold);
			StatsLabel.Text = "High Scores";
			StatsLabel.TextAlign = ContentAlignment.MiddleCenter;
			StatsLabel.Size = new Size(190, 30);
			StatsLabel.Location = new Point(5, 5);
			StatisticsMenu.Controls.Add(StatsLabel);
			
			Label StatsLabelLine = new Label();
			StatsLabelLine.Size = new Size(190, 2);
			StatsLabelLine.Location = new Point(5, 37);
			StatsLabelLine.BorderStyle = BorderStyle.FixedSingle;
			StatisticsMenu.Controls.Add(StatsLabelLine);
			
			// Easy Stats
			Label EasyLabel = new Label();
			EasyLabel.Text = "Easy";
			EasyLabel.Font = new Font(EasyLabel.Font.FontFamily, Single.Parse("10"), FontStyle.Bold);
			EasyLabel.TextAlign = ContentAlignment.MiddleLeft;
			EasyLabel.Size = new Size(190, 20);
			EasyLabel.Location = new Point(5, 42);
			StatisticsMenu.Controls.Add(EasyLabel);
			AddScoreLabels(62,1);
			
			// Medium Stats
			Label MediumLabel = new Label();
			MediumLabel.Text = "Medium";
			MediumLabel.Font = new Font(MediumLabel.Font.FontFamily, Single.Parse("10"), FontStyle.Bold);
			MediumLabel.TextAlign = ContentAlignment.MiddleLeft;
			MediumLabel.Size = new Size(190, 20);
			MediumLabel.Location = new Point(5, 121);
			StatisticsMenu.Controls.Add(MediumLabel);
			AddScoreLabels(141,2);
			
			// Hard Stats
			Label HardLabel = new Label();
			HardLabel.Text = "Hard";
			HardLabel.Font = new Font(HardLabel.Font.FontFamily, Single.Parse("10"), FontStyle.Bold);
			HardLabel.TextAlign = ContentAlignment.MiddleLeft;
			HardLabel.Size = new Size(190, 20);
			HardLabel.Location = new Point(5, 200);
			StatisticsMenu.Controls.Add(HardLabel);
			AddScoreLabels(220,3);
			
			Label StatsLabelLineTwo = new Label();
			StatsLabelLineTwo.Size = new Size(190, 2);
			StatsLabelLineTwo.Location = new Point(5, 279);
			StatsLabelLineTwo.BorderStyle = BorderStyle.FixedSingle;
			StatisticsMenu.Controls.Add(StatsLabelLineTwo);
			
			// The Okay Button
			NoFocusButton OkayButton = new NoFocusButton();
			OkayButton.Text = "Ok";
			OkayButton.Name = "OkayButton";
			OkayButton.Size = new Size(90, 25);
			OkayButton.Location = new Point(55, 285);
			OkayButton.Font = new Font(OkayButton.Font.FontFamily, Single.Parse("10"));
			OkayButton.Click += StatisticsMenuClose;
			OkayButton.FlatStyle = FlatStyle.Flat;
			StatisticsMenu.Controls.Add(OkayButton);
			
		}
		
		public string GetUserName()
		{
			GetUserNameForm();
			NameMenu.ShowDialog();
			string name = NameMenu.Controls.Find("NameTextBox", true)[0].Text;
			return name;
		}
		
		private void GetUserNameForm()
		{
			NameMenu = new Form();
			NameMenu.MaximizeBox = false;
			NameMenu.FormBorderStyle = FormBorderStyle.FixedDialog;
			NameMenu.Name = "NameMenu";
			NameMenu.Text = "Minesweeper";
			NameMenu.ClientSize = new Size(200, 100);
			NameMenu.KeyPreview = true;
			NameMenu.KeyPress += new KeyPressEventHandler(NameMenuKeys);
			NameMenu.Icon = GameResources.GetIconImage();
			
			Label HSLabel = new Label();
			HSLabel.Font = new Font(HSLabel.Font.FontFamily, Single.Parse("14"), FontStyle.Bold);
			HSLabel.Text = "New High Score!";
			HSLabel.TextAlign = ContentAlignment.MiddleCenter;
			HSLabel.Size = new Size(190, 30);
			HSLabel.Location = new Point(5, 5);
			NameMenu.Controls.Add(HSLabel);
			
			Label NameLabel = new Label();
			NameLabel.Text = "Name";
			NameLabel.TextAlign = ContentAlignment.MiddleLeft;
			NameLabel.Size = new Size(40, 25);
			NameLabel.Location = new Point(5, 35);
			NameMenu.Controls.Add(NameLabel);
			
			TextBox NameTextBox = new TextBox();
			NameTextBox.Name = "NameTextBox";
			NameTextBox.Text = Environment.UserName;
			NameTextBox.Location = new Point(50, 38);
			NameTextBox.Size = new Size(145,25);
			NameTextBox.MaxLength = 10;
			NameMenu.Controls.Add(NameTextBox);
			
			// The Okay Button
			NoFocusButton OkayButton = new NoFocusButton();
			OkayButton.Text = "Ok";
			OkayButton.Name = "OkayButton";
			OkayButton.Size = new Size(90, 25);
			OkayButton.Location = new Point(55, 68);
			OkayButton.Font = new Font(OkayButton.Font.FontFamily, Single.Parse("10"));
			OkayButton.Click += NameMenuClose;
			OkayButton.FlatStyle = FlatStyle.Flat;
			NameMenu.Controls.Add(OkayButton);
		}
		
		private void NameMenuClose(object sender, EventArgs e)
		{
			NameMenu.Close();
		}
		
		private void NameMenuKeys(object sender, KeyPressEventArgs e)
		{
			// If Enter
			if(e.KeyChar == (char)13)
			{
				NameMenu.Close();
			}
		}
		
		private void AddScoreLabels(int yStart, int diff)
		{
			GameStatisticData gScores;
			switch(diff)
			{
				case 1  : {
							 gScores = Easy;
							 break;
						  }
				case 2  : {
							 gScores = Medium;
							 break;
						  }
				case 3  : {
							 gScores = Hard;
							 break;
						  }
				default : {
							 gScores = Easy;
							 break;
						  }
			}
			foreach(Tuple<int, string> item in gScores)
			{
				Label ScoreLabel = new Label();
				ScoreLabel.Text = item.Item2;
				ScoreLabel.Font = new Font("Lucida Console", 10);
				ScoreLabel.TextAlign = ContentAlignment.MiddleCenter;
				ScoreLabel.Size = new Size(190, 20);
				ScoreLabel.Location = new Point(5, yStart);
				StatisticsMenu.Controls.Add(ScoreLabel);
				yStart += 18;
			}
		}
		
		public void StatisticsMenuRun()
		{
			BuildStatisticsMenu();
			StatisticsMenu.ShowDialog();
		}
		
		private void StatisticsMenuClose(object sender, EventArgs e)
		{
			StatisticsMenu.Close();
		}
		
		public void LoadEasyValue(int score, string msg)
		{
			Easy.LoadScore(score, msg);
		}
		
		public void LoadMediumValue(int score, string msg)
		{
			Medium.LoadScore(score, msg);
		}
		
		public void LoadHardValue(int score, string msg)
		{
			Hard.LoadScore(score, msg);
		}
		
		public bool IsEasyHighScore(int score)
		{
			return Easy.IsHighScore(score);
		}
		
		public bool IsMediumHighScore(int score)
		{
			return Medium.IsHighScore(score);
		}
		
		public bool IsHardHighScore(int score)
		{
			return Hard.IsHighScore(score);
		}
		
		public void SaveStats()
		{
			WriteGameStatsToFile();
		}
		
		
		public void TestingWriteAndRead()
		{
			//WriteGameStatsToFile();
			LoadGameStatsFromFile();
		}
		
		public void TestWriteState()
		{
			Console.WriteLine("**  EASY  **");
			foreach(Tuple<int, string> item in Easy)
			{
				Console.WriteLine("{0} : {1}", item.Item1, item.Item2);
			}
			Console.WriteLine("\n");
			
			Console.WriteLine("**  Medium  **");
			foreach(Tuple<int, string> item in Medium)
			{
				Console.WriteLine("{0} : {1}", item.Item1, item.Item2);
			}
			Console.WriteLine("\n");
			
			Console.WriteLine("**  Hard  **");
			foreach(Tuple<int, string> item in Hard)
			{
				Console.WriteLine("{0} : {1}", item.Item1, item.Item2);
			}
			Console.WriteLine("\n");
		
		}
		
	}
}