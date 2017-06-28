using System;

namespace Minesweeper
{
	public class InvalidLocationException : Exception
	{
		public InvalidLocationException() : this("Invalid Location") {}
		public InvalidLocationException(string message) : base(message) {}
	}
}