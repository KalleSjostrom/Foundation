using System.Collections;
using System;
using UnityEngine;

namespace Foundation {
	public static class Log
	{
		private static string format(string category, string format, params object[] args)
		{
			string s = String.Format(format, args);
			s = String.Format("[{0}] [{1}] {2}", DateTime.Now.ToString("HH:mm:ss.fff"), category, s);
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
		
		public static void Assert(bool condition, string format, params object[] args)
		{
			if (!condition) {
				string s = String.Format(format, args);
				throw new Exception(String.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss.fff"), s));
			}
		}
		
		public static void ScreenText(string text) {
			GameObject go = GameObject.FindGameObjectWithTag("DebugHud");
			DebugScreenText dtd = go.GetComponent<DebugScreenText>();
			dtd.ScreenText(text);
		}
	}
}