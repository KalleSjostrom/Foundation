using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.Reflection;
using System.Security.Cryptography;

namespace Foundation {
	public class Saver {
	
		private string _filename;
			
		public Saver(string filename) 
		{
			_filename = filename;
		}
		
		public void Save<T>(T data) where T : ISerializable
		{
			Saver.Save<T>(data, _filename);
		}
		public T Load<T>() where T : ISerializable, new()
		{
			return Saver.Load<T>(_filename);
		}
		
		public static void Save<T>(T data, string filename) where T : ISerializable
		{
			using (Stream stream = File.Open(filename, FileMode.Create)) {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(stream, data);
				stream.Close();
			}
		}
		
		public static T Load<T>(string filename) where T : ISerializable, new()
		{
			T data = default(T);
			if (!File.Exists(filename))
				return data;

			using (Stream stream = File.Open(filename, FileMode.Open)) {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				try {
					data = (T) binaryFormatter.Deserialize(stream);
				} catch (Exception e) {
					Debug.LogWarning(e);
					return new T();
				}
				stream.Close();
			}
			return data;
		}
	}
}