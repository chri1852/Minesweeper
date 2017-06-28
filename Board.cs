using System;
using System.Drawing;
using System.Collections.Generic;

namespace Minesweeper
{
	class Board
	{
		public readonly int Height;
		public readonly int Width;
		public readonly int NumberOfMines;
		public bool isGameOver;
		private bool isFirstMove;
		private Random RandomGenerator;
		private Cell[,] GameBoard;
		public event EventHandler<GameOverEventArgs> gOverEvent;
		
		public Board() : this(9, 9, 10) {}
		public Board(int height, int width) : this(height, width, 10) {}
		public Board(int height, int width, int numMines)
		{
			if(validDimension(height))
			{
				Height = height;
			}
			else
			{
				Height = 9;
			}
			
			if(validDimension(width))
			{
				Width = width;
			}
			else
			{
				Width = 9;
			}

			if(validNumMines(numMines))
			{
				NumberOfMines = numMines;
			}
			else
			{
				NumberOfMines = 10;
			}

			RandomGenerator = new Random();
			GameBoard = new Cell[Height,Width];
			isGameOver = false;
			isFirstMove = true;
			
			// generate the game board cells
			for(int hgt = 0; hgt < Height; hgt++)
			{
				for(int wth = 0; wth < Width; wth++)
				{
					GameBoard[hgt,wth] = new Cell();
				}
			}
				
		}
		
		public void initializeBoard()
		{
			AddMines();
			AddValues();
		}
		
		private void AddMines()
		{
			// Create the mines
			int placedMines = 0;
			while(placedMines < NumberOfMines)
			{
				int x = RandomGenerator.Next(0, Width);
				int y = RandomGenerator.Next(0, Height);
				
				if(!GameBoard[y,x].isMine)
				{
					GameBoard[y,x].isMine = true;
					placedMines++;
				}
			}
		}
		
		private void AddValues()
		{
			// Calculate the value for the cells
			for(int hgt = 0; hgt < Height; hgt++)
			{
				for(int wth = 0; wth < Width; wth++)
				{
					if(!GameBoard[hgt,wth].isMine)
					{
						GameBoard[hgt,wth].MineValue = numberOfSurroundingMines(wth,hgt);
					}
				}
			}
		}
		
		// functions to check the validity of the the propertys
		
		// Validates the height and width
		private bool validDimension(int dimension)
		{
			if((dimension >= 8) && (dimension <= 50))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		// Validates the number of mines
		private bool validNumMines(int numMines)
		{
			if((numMines >= 0) && (numMines <= ((Height*Width)-9)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		// Checks to see if a value is in bounds
		public bool isValidLocation(int x, int y)
		{
			if((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		
		// returns the number of mines surrounding a cells
		private int numberOfSurroundingMines(int x, int y)
		{
			int numSurroundingMines = 0;
			
			if(isValidLocation(x-1,y-1) && GameBoard[y-1,x-1].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x-1,y) && GameBoard[y,x-1].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x-1,y+1) && GameBoard[y+1,x-1].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x,y-1) && GameBoard[y-1,x].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x,y+1) && GameBoard[y+1,x].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x+1,y-1) && GameBoard[y-1,x+1].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x+1,y) && GameBoard[y,x+1].isMine)
			{
				numSurroundingMines++;
			}
			
			if(isValidLocation(x+1,y+1) && GameBoard[y+1,x+1].isMine)
			{
				numSurroundingMines++;
			}
			
			return numSurroundingMines;
		}
		
		public bool SetAsUnHidden(int x, int y)
		{
			if(isValidLocation(x, y) && (GameBoard[y,x].isHidden) && !(GameBoard[y,x].isFlagged))
			{
				GameBoard[y,x].isHidden = false;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public int GetCellValue(int x, int y)
		{
			if(isValidLocation(x, y))
			{
				return GameBoard[y,x].MineValue;
			}
			else
			{
				throw new InvalidLocationException(string.Format("Location X: {0}, Y: {0} is Invalid!", x, y));
			}
		}
		
		public Color GetCellColor(int x, int y)
		{
			if(isValidLocation(x, y))
			{
				return GameBoard[y,x].getColor();
			}
			else
			{
				throw new InvalidLocationException(string.Format("Location X: {0}, Y: {0} is Invalid!", x, y));
			}
		}
		
		public bool GetCellHidden(int x, int y)
		{
			if(isValidLocation(x, y))
			{
				return GameBoard[y,x].isHidden;
			}
			else
			{
				return false;
			}
		}
		
		public bool GetCellQuestion(int x, int y)
		{
			if(isValidLocation(x, y))
			{
				return GameBoard[y,x].isQuestion;
			}
			else
			{
				return false;
			}
		}
		
		public bool GetCellFlagged(int x, int y)
		{
			if(isValidLocation(x, y))
			{
				return GameBoard[y,x].isFlagged;
			}
			else
			{
				return false;
			}
		}
		
		public void SetCellHidded(int x, int y, bool hide)
		{
			if(isValidLocation(x, y))
			{
				GameBoard[y,x].isHidden = hide;
			}
			else
			{
				throw new InvalidLocationException(string.Format("Location X: {0}, Y: {0} is Invalid!", x, y));
			}
		}
		
		public void SetCellQuestion(int x, int y, bool question)
		{
			if(isValidLocation(x, y))
			{
				GameBoard[y,x].isQuestion = question;
			}
			else
			{
				throw new InvalidLocationException(string.Format("Location X: {0}, Y: {0} is Invalid!", x, y));
			}
		}
		
		public void SetCellFlagged(int x, int y, bool flag)
		{
			if(isValidLocation(x, y))
			{
				GameBoard[y,x].isFlagged = flag;
			}
			else
			{
				throw new InvalidLocationException(string.Format("Location X: {0}, Y: {0} is Invalid!", x, y));
			}
		}
		
		public bool GetIsMine(int x, int y)
		{
			if(isValidLocation(x, y))
			{
				return GameBoard[y,x].isMine;
			}
			else
			{
				return false;
			}
		}
		
		public int GetFlaggedNearBy(int x, int y)
		{
			int numFlaggedNear = 0;
			
			if(isValidLocation(x-1,y-1) && GameBoard[y-1,x-1].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x-1,y) && GameBoard[y,x-1].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x-1,y+1) && GameBoard[y+1,x-1].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x,y-1) && GameBoard[y-1,x].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x,y+1) && GameBoard[y+1,x].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x+1,y-1) && GameBoard[y-1,x+1].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x+1,y) && GameBoard[y,x+1].isFlagged)
			{
				numFlaggedNear++;
			}
			
			if(isValidLocation(x+1,y+1) && GameBoard[y+1,x+1].isFlagged)
			{
				numFlaggedNear++;
			}
			
			return numFlaggedNear;
		}
		
		
		public int GetTotalFlaggedTiles()
		{
			int FlaggedMinesCount = 0;
			for(int hgt = 0; hgt < Height; hgt++)
			{
				for(int wth = 0; wth < Width; wth++)
				{
					if(GameBoard[hgt,wth].isFlagged)
					{
						FlaggedMinesCount++;
					}
				}
			}
			return FlaggedMinesCount;
		}
		
		public bool TestGameOver()
		{
			CheckForGameOver();
			return isGameOver;
		}
		
		private void CheckForGameOver()
		{
			if(AreExposedMines())
			{
				isGameOver = true;
				GameOverEventArgs gOEA = new GameOverEventArgs();
				gOEA.isGameOver = true;
				gOEA.isWinner = false;
				OnGameOver(gOEA);
			}
			else
			if(onlyCorrectFlagsLeft())
			{
				isGameOver = true;
				GameOverEventArgs gOEA = new GameOverEventArgs();
				gOEA.isGameOver = true;
				gOEA.isWinner = true;
				OnGameOver(gOEA);
			}
			
			

		}
		
		private bool AreExposedMines()
		{
			for(int hgt = 0; hgt < Height; hgt++)
			{
				for(int wth = 0; wth < Width; wth++)
				{
					if(GameBoard[hgt,wth].isMine && !GameBoard[hgt,wth].isHidden)
					{
						return true;
					}
				}
			}
			return false;
		}
		
		private bool onlyCorrectFlagsLeft()
		{
			for(int hgt = 0; hgt < Height; hgt++)
			{
				for(int wth = 0; wth < Width; wth++)
				{
					if(GameBoard[hgt, wth].isHidden && !GameBoard[hgt, wth].isMine && !GameBoard[hgt, wth].isFlagged)
					{
						return false;
					}
				}
			}
			return true;
		}
		
		protected virtual void OnGameOver(GameOverEventArgs e)
		{
			EventHandler<GameOverEventArgs> handler = gOverEvent;
			if(handler != null)
			{
				handler(this, e);
			}
		}
		
		public bool CheckFirstMoveORG(int x, int y)
		{
			if(isFirstMove)
			{
				while(GameBoard[y, x].MineValue != 0)
				{
					// reset the GameBoard to blankCells
					for(int hgt = 0; hgt < Height; hgt++)
					{
						for(int wth = 0; wth < Width; wth++)
						{
							GameBoard[hgt,wth] = new Cell();
						}
					}
					initializeBoard();
				}
				isFirstMove = false;
				return true;
			}
			return false;
		}
		
		public bool CheckFirstMove(int x, int y)
		{
			if(isFirstMove)
			{
				while(GameBoard[y, x].MineValue != 0)
				{
					// Get All valid open spots
					List<Tuple<int, int>> openSpaces = new List<Tuple<int, int>>();
					for(int hgt = 0; hgt < Height; hgt++)
					{
						for(int wth = 0; wth < Width; wth++)
						{

							// if it is not adjacent
							if(GameBoard[hgt, wth].MineValue != 9 && !IsAdjacent(x, y, wth, hgt))
							{
								openSpaces.Add(new Tuple<int, int>(wth, hgt));
							}
						}
					}
					Tuple<int, int> nextVal = ReturnNineBlockMines(x, y);
					if(nextVal.Item1 == 9999 && nextVal.Item2 == 9999)
					{
						break;
					}
					Tuple<int, int> openSpot = openSpaces[RandomGenerator.Next(0, openSpaces.Count)];
					GameBoard[openSpot.Item2, openSpot.Item1].isMine = true;
					GameBoard[nextVal.Item2, nextVal.Item1].isMine = false;
					
					AddValues();
				}
				isFirstMove = false;
				return true;
			}
			return false;
		}
		
		private bool IsAdjacent(int x, int y, int locX, int locY)
		{
			if((x-locX == 1 && y-locY ==1) || (x-locX == 1 && y-locY ==0) || (x-locX == 1 && y-locY ==-1) || (x-locX == 0 && y-locY ==1) || (x-locX == 0 && y-locY ==-1) || (x-locX == -1 && y-locY ==1) || (x-locX == -1 && y-locY ==0) || (x-locX == -1 && y-locY ==-1))
			{
				return true;
			}
			
			return false;
		}
		
		private Tuple<int, int> ReturnNineBlockMines(int x, int y)
		{
			if(isValidLocation(x-1,y-1) && GameBoard[y-1,x-1].isMine)
			{
				return new Tuple<int, int>(x-1, y-1);
			}
			else if(isValidLocation(x-1,y) && GameBoard[y,x-1].isMine)
			{
				return new Tuple<int, int>(x-1, y);
			}
			else if(isValidLocation(x-1,y+1) && GameBoard[y+1,x-1].isMine)
			{
				return new Tuple<int, int>(x-1, y+1);
			}
			else if(isValidLocation(x,y-1) && GameBoard[y-1,x].isMine)
			{
				return new Tuple<int, int>(x, y-1);
			}
			else if(isValidLocation(x,y) && GameBoard[y,x].isMine)
			{
				return new Tuple<int, int>(x, y);
			}
			else if(isValidLocation(x,y+1) && GameBoard[y+1,x].isMine)
			{
				return new Tuple<int, int>(x, y+1);
			}
			else if(isValidLocation(x+1,y-1) && GameBoard[y-1,x+1].isMine)
			{
				return new Tuple<int, int>(x+1, y-1);
			}
			else if(isValidLocation(x+1,y) && GameBoard[y,x+1].isMine)
			{
				return new Tuple<int, int>(x+1, y);
			}
			else if(isValidLocation(x+1,y+1) && GameBoard[y+1,x+1].isMine)
			{
				return new Tuple<int, int>(x+1, y+1);
			}
			else
			{
				return new Tuple<int, int>(9999, 9999);
			}
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