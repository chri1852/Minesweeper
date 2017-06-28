using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Drawing;

namespace Minesweeper
{
	class program
	{
		public static void Main(string[] args)
		{
			GameGUI NewGame = new GameGUI();
			NewGame.StartForm();
		
			
			//TestStats();
			
			/*
			Assembly thisExe = Assembly.GetExecutingAssembly();
			string[] resources = thisExe.GetManifestResourceNames();
			// Build the string of resources.
			foreach (string resource in resources)
			{
				Console.WriteLine("RSC: {0}", resource + "\r\n");
			} */
			/*
			Image img = Image.FromFile("MineIcon.Ico");
			ResXResourceWriter rsxw = new ResXResourceWriter("MineResource.resx"); 
			rsxw.AddResource("MineIcon.Ico",img);
			rsxw.Close();
			*/
			Console.ReadLine(); 
			   
			
		}
		
		public static void TestStats()
		{
			Console.WriteLine("***** Testing the File Reader *****\n");
			
			GameStatisticsManager TestMgr = new GameStatisticsManager();
			
			TestMgr.TestingWriteAndRead();
			
			TestMgr.TestWriteState();
			
			TestMgr.LoadEasyValue(25, "YANGKONG");
			TestMgr.LoadEasyValue(56, "BRULSTE");
			TestMgr.LoadEasyValue(12, "CHRIALE");
			
			TestMgr.TestWriteState();
			
			TestMgr.LoadEasyValue(98, "BAGGBIL");
			TestMgr.LoadEasyValue(10, "BAGGFRO");
			
			TestMgr.TestWriteState();
			
			TestMgr.SaveStats();
			
			TestMgr.StatisticsMenuRun();
			
			Console.WriteLine("{0}", Environment.UserName);
			
			Console.ReadLine();
		}
	}
}