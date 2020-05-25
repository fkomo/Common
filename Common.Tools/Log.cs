using System;
using System.Diagnostics;
using System.IO;

namespace Ujeby.Common.Tools
{
	public class Log
	{
		public delegate void ShowNotificationCallback(string text);
		public delegate void WriteLogLineCallback(string text);

		public static ShowNotificationCallback ShowNotification { get; set; } = null;
		public static WriteLogLineCallback LogLineAdded { get; set; } = null;

		private static string LogFile { get { return Path.Combine(LogFolder, LogFileName); } }
		public static string LogFolder { get; set; }
		public static string LogFileName { get; set; } = "ujeby.log";
		public static bool WriteToConsole { get; set; } = false;

		private static object logLock = new object();

		public static void WriteLine(string line)
		{
			try
			{
				line = $"{ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") } | { line }";

				lock (logLock)
				{
					File.AppendAllLines(LogFile, new string[] { line });
				}

				if (WriteToConsole)
					Console.WriteLine(line);

				if (Debugger.IsAttached)
					Debug.WriteLine(line);

				LogLineAdded?.Invoke(line);
			}
			catch (Exception ex)
			{
				ShowNotification?.Invoke(ex.ToString());
			}
		}
	}
}
