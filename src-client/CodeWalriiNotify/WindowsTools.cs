using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CodeWalriiNotify
{
	public static class WindowsTools
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(UnmanagedType.Bool)]
		static extern bool SetDllDirectory(string lpPathName);

		[DllImport("User32.dll")]
		public static extern int MessageBox(IntPtr h, string m, string c, uint type);

		public static bool CheckWindowsGtk()
		{
			string location = null;
			Version version = null;
			var minVersion = new Version(2, 12, 22);
			using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Xamarin\GtkSharp\InstallFolder")) {
				if (key != null)
					location = key.GetValue(null) as string;
			}
			using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Xamarin\GtkSharp\Version")) {
				if (key != null)
					Version.TryParse(key.GetValue(null) as string, out version);
			}

			if (version == null || version < minVersion || location == null || !File.Exists(Path.Combine(location, "bin", "libgtk-win32-2.0-0.dll"))) {
				Console.WriteLine("Did not find required GTK# installation");
				const string url = "http://monodevelop.com/Download";
				const string title = "Critical Error";
				const string message = "The program did not find the required version of GTK#.\nPlease click OK to open the download page,\nwhere you can download and install the latest version.";

				if (MsgBox(message, title, MessageBoxButtons.MB_OKCANCEL, MessageBoxIcon.MB_ICONERROR) == NotificationResult.ID_OK)
					Process.Start(url);

				return false;
			}
			//Console.WriteLine("Found GTK# version " + version);
			var path = Path.Combine(location, @"bin");
			//Console.WriteLine("SetDllDirectory(\"{0}\") ", path);
			try {
				if (SetDllDirectory(path)) {
					return true;
				}
			} catch (EntryPointNotFoundException) {
			}
			// this shouldn't happen unless something is weird in Windows
			MsgBox("Unable to set GTK+ dll directory", "You'r windoze is wired!", MessageBoxButtons.MB_OK, MessageBoxIcon.MB_ICONERROR);
			return true;
		}

		public enum NotificationResult
		{
			// MessageBox Notification Result Constants
			ID_ABORT = 0x3,
			ID_CANCEL = 0x2,
			ID_CONTINUE = 0xB,
			ID_IGNORE = 0x5,
			ID_NO = 0x7,
			ID_OK = 0x1,
			ID_RETRY = 0x4,
			ID_TRYAGAIN = 0xA,
			ID_YES = 0x6
		}

		public enum MessageBoxButtons : uint
		{
			// Buttons
			MB_ABORTRETRYIGNORE = 0x2,
			MB_CANCELTRYCONTINUE = 0x6,
			MB_HELP = 0x4000,
			MB_OK = 0x0,
			MB_OKCANCEL = 0x1,
			MB_RETRYCANCEL = 0x5,
			MB_YESNO = 0x4,
			MB_YESNOCANCEL = 0x3,
		}

		public enum MessageBoxIcon : uint
		{
			// Icons
			None = 0x0,
			MB_ICONEXCLAMATION = 0x30,
			MB_ICONWARNING = 0x30,
			MB_ICONINFORMATION = 0x40,
			MB_ICONASTERISK = 0x40,
			MB_ICONQUESTION = 0x20,
			MB_ICONSTOP = 0x10,
			MB_ICONERROR = 0x10,
			MB_ICONHAND = 0x10,
		}

		public enum MessageBoxDefaultButton : uint
		{
			// Default Buttons
			MB_DEFBUTTON1 = 0x0,
			MB_DEFBUTTON2 = 0x100,
			MB_DEFBUTTON3 = 0x200,
			MB_DEFBUTTON4 = 0x300,
		}

		// Methods
		public static NotificationResult MsgBox(string message)
		{
			return MsgBox(message, string.Empty);
		}

		public static NotificationResult MsgBox(string message, string caption)
		{
			return MsgBox(message, caption, MessageBoxButtons.MB_OK);
		}

		public static NotificationResult MsgBox(string message, string caption, MessageBoxButtons options)
		{
			return MsgBox(message, caption, options, MessageBoxIcon.None);
		}

		public static NotificationResult MsgBox(string message, string caption, MessageBoxButtons options, MessageBoxIcon icon)
		{
			return MsgBox(message, caption, options, icon, MessageBoxDefaultButton.MB_DEFBUTTON1);
		}

		public static NotificationResult MsgBox(string message, string caption, MessageBoxButtons options, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			return (NotificationResult)MessageBox((IntPtr)0, message, caption, (uint)options + (uint)icon + (uint)defaultButton);
		}
	}
}

