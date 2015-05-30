using System;
using Gtk;
using System.IO;
using System.Diagnostics;

namespace CodeWalriiNotify
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// Are we on Windows?
			if (Environment.OSVersion.VersionString.ToLower().Contains("windows")) {
				// Set everything up for GTK on Windows
				if (!WindowsTools.CheckWindowsGtk())
					return;
			}

			// Initialize the application
			Application.Init();

			// Create the main window class
			var win = new MainWindow();
			// Show the main window
			win.Show();

			// RUN IT!
			Application.Run();
		}
	}
}
