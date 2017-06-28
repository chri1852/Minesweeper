using System;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Minesweeper
{
	public static class GameResources
	{
		
		public static string GetGameStatsFile()
		{
			return GetResourceDirectory() + "Save.msv";
		}
		
		public static string GetResourceDirectory()
		{
			string rDirectory = @"C:\users\";
			rDirectory += Environment.UserName;
			rDirectory += @"\AppData\Local\Apps\Minesweeper\";
			return rDirectory;
		}
		
		public static Icon GetIconImage()
		{
			Assembly thisExe = Assembly.GetExecutingAssembly();
			Stream file = thisExe.GetManifestResourceStream("MineIcon.Ico");
			return new Icon(file);
		}
		
		public static Icon GetIconImageSized(int x, int y)
		{
			Size nSize = new Size(x, y);
			Assembly thisExe = Assembly.GetExecutingAssembly();
			Stream file = thisExe.GetManifestResourceStream("MineIcon.Ico");
			return new Icon(file, nSize);
		}
		
	}
}