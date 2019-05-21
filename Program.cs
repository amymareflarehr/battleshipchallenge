using System;

namespace BattleshipStateTracker
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("Hello World");
			
			var board = BattleshipStateTracker.Board.Create(10, 10);
			
			Console.WriteLine(board.Width);
			Console.WriteLine(board.Height);
			
			//turn these into unit tests with asserts...
			
			//test board is made of 0s
			Console.WriteLine(board.AllSquares[3][5]);
			
			Console.WriteLine("INFO: test small battleship horizontally at 0,0 returns true");
			Console.WriteLine(" Test passing?: " + board.TestPlaceBattleship(2, 0, 0, true, true));
			
			Console.WriteLine("INFO: test small battleship horizontally at 9,9 returns false");
			Console.WriteLine(" Test passing?: " + board.TestPlaceBattleship(2, 9, 9, false, false));
			
			Console.WriteLine("INFO: test small battleship vertically at 9,9 returns false");
			Console.WriteLine(" Test passing?: " + board.TestPlaceBattleship(2, 9, 9, true, false));
			
			Console.WriteLine("INFO: test small battleship horizontally at 8,9 returns true");
			Console.WriteLine(" Test passing?: " + board.TestPlaceBattleship(2, 8, 9, true, true));
			
			Console.WriteLine("INFO: test small battleship vertically at 9,8 returns false - already a battleship there");
			Console.WriteLine(" Test passing?: " + board.TestPlaceBattleship(2, 9, 8, false, false));
			
			Console.WriteLine("INFO: And is it game over?");
			Console.WriteLine(board.CheckGameOver().ToString());
			
			
			var newBoard = BattleshipStateTracker.Board.Create(10, 10);
			
			Console.WriteLine("INFO: test small battleship vertically at 9,8 returns true");
			Console.WriteLine(" Test passing?: " + newBoard.TestPlaceBattleship(2, 9, 8, false, true));
			
			Console.WriteLine("INFO: attack 9,9 returns true");
			Console.WriteLine(" Test passing?: " + (newBoard.Attack(9,9) == true));
			
			Console.WriteLine("INFO: attack 8,8 returns false");
			Console.WriteLine(" Test passing?: " + (newBoard.Attack(8,8) == false));
			
			Console.WriteLine("INFO: now lets sink that baby");
			Console.WriteLine(" Test passing?: " + (newBoard.Attack(9,8) == true));
			
			Console.WriteLine("INFO: And is it game over?");
			Console.WriteLine(newBoard.CheckGameOver().ToString());
		}
	}

	public class Vessel
	{
		private int _length;
		
		public int Length
		{
			get { return _length; }
		}
		
		public Vessel (int length)
		{
			this._length = length;
		}
	}
	
	public class Board
	{
		private int _width;
		private int _height;
		
		public int Width
		{
			get { return _width; }
		}
		
		public int Height
		{
			get { return _height; }
		}

		public int[][] AllSquares {get;set;}
		
		public Board(int width, int height) 
		{
			this._width = width;
			this._height = height;
			
			int[][] allSquares = new int[height][];
			for (int i = 0;i<height;i++)
			{
				int[] horizontalSquares = new int[width];
				for (int j = 0;j<width;j++)
				{
					horizontalSquares[j] = 0;
				}
				allSquares[i] = horizontalSquares;
			}
			this.AllSquares = allSquares;
		}
		
		public static Board Create(int width, int height)
		{
			return new Board(width, height);
		}
		
		public bool TestPlaceBattleship(int vesselLength, int xCoordinate, int yCoordinate, bool isHorizontal, bool expectedResult)
		{
			Vessel firstBattleship = new Vessel(vesselLength);
			
			Tuple<int,int> coordinates = new Tuple<int, int>(xCoordinate, yCoordinate);
			
			bool result = this.Place(firstBattleship, coordinates, isHorizontal);
			return result == expectedResult;
		}
		
		private bool Place(Vessel vessel, Tuple<int,int> startingCoordinate, bool isHorizontal)
		{
			int xCoordinate = startingCoordinate.Item1;
			int yCoordinate = startingCoordinate.Item2;
			
			if (xCoordinate > this._width || yCoordinate > this._height)
			{
				return false;
			}
			
			if (isHorizontal) 
			{
				if (xCoordinate + vessel.Length > this._width)
				{
					return false;
				}
				for (int i = 0;i < vessel.Length; i++)
				{
					if (AllSquares[xCoordinate + i][yCoordinate] > 0) 
					{
						//uh oh - something already here.
						return false;
					}
					AllSquares[xCoordinate + i][yCoordinate] = 1;
				}
			}
			else 
			{
				if (yCoordinate + vessel.Length > this._height)
				{
					return false;
				}
				
				for (int i = 0;i < vessel.Length; i++)
				{
					if (AllSquares[xCoordinate][yCoordinate + i] > 0) 
					{
				Console.WriteLine("hi?" + xCoordinate + yCoordinate.ToString());
						return false;
					}
					AllSquares[xCoordinate][yCoordinate + i] = 1;
				}
			}
			
			return true;
		}
		
		public bool Attack(int xCoordinate, int yCoordinate)
		{
			bool hit = false;
			
			if (xCoordinate < this._width && yCoordinate < this._height)
			{
				if (AllSquares[xCoordinate][yCoordinate] == 1) 
				{
					AllSquares[xCoordinate][yCoordinate] = 2;
					hit = true;
				}
			}
			
			return hit;
		}
		
		public bool CheckGameOver ()
		{
			for (int i = 0;i < this._height;i++)
			{
				int[] horizontalSquares = new int[this._width];
				for (int j = 0;j < this._width;j++)
				{
					if (AllSquares[i][j] == 1)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}