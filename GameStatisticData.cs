using System;
using System.Collections;
using System.Collections.Generic;

namespace Minesweeper
{
	public class GameStatisticData : IEnumerable
	{
		private List<Tuple<int, string>> Scores;
		private string _Difficulty;
		public string Difficulty
		{
			get
			{
				return _Difficulty;
			}
			set
			{
				if(value != "Easy" && value != "Medium" && value != "Hard")
				{
					_Difficulty = "Easy";
				}
				else
				{
					_Difficulty = value;
				}
			}
		}

		public GameStatisticData() : this("Easy") {}
		public GameStatisticData(string diff)
		{
			Difficulty = diff;
			Scores = new List<Tuple<int, string>>();
		}
		
		public void LoadScore(int score, string name)
		{
			String scoreString = score.ToString();
			String nameValue = name.PadRight(10, ' ').Substring(0,10).TrimEnd().PadRight(16 - scoreString.Length, '.');
			if(score == 0)
			{
				nameValue += ".";
			}
			else
			{
				nameValue += score.ToString();
			}
			
			Scores.Add(new Tuple<int, string>(score, nameValue));
			SortScoreList();
		}
		
		public bool IsHighScore(int score)
		{
			if(score < Scores[2].Item1)
			{
				return true;
			}
			return false;
		}
		
		public IEnumerator GetEnumerator()
		{
			foreach(Tuple<int, string> item in Scores)
			{
				yield return item;
			}
		}
		
		
		public void WriteDiff()
		{
			Console.WriteLine("Diff: {0}", this.Difficulty);
		}
		
		private void SortScoreList()
		{
			Scores.Sort((x,y)=> x.Item1.CompareTo(y.Item1));
			
			if(Scores.Count > 3)
			{
				Scores.RemoveAt(3);
			}
		}
	
	}
}