using System.Collections;
using System;

namespace Foundation {
	public static class Log
	{
		private static string format(string category, string format, params object[] args)
		{
			string s = String.Format(format, args);
			s = String.Format("[{0}] [{1}] {2}", DateTime.Now.ToString(), category, s);
			return s;
		}
		
		public static void Debug(string category, string format, params object[] args) 
		{	
			UnityEngine.Debug.Log(Log.format(category, format, args));
		}
		
		public static void Info(string category, string format, params object[] args) 
		{
			// if (LogLevels[category] < INFO) return;
			UnityEngine.Debug.Log(Log.format(category, format, args));
		}

		public static void Warning(string category, string format, params object[] args) 
		{
			UnityEngine.Debug.LogWarning(Log.format(category, format, args));
		}
		
		public static void Assert(bool condition, string category, string format, params object[] args)
		{
			if (!condition) throw new Exception(Log.format(category, format, args));
		}
	}
}