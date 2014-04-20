using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

namespace Foundation {
	public class SaverEncrypted {
			
		private const string ENCRYPTION_KEY = "*#4$%^.++q~!cfr0(_!#$@$!&#&#*&@(7cy9rn8r265&$@&*E^184t44tq2cr9o3r6329";
		private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
		
		private string _filename;
		
		private string _encryptionKey;
		private byte[] _iv;
			
		public SaverEncrypted(string filename, string encryptionKey = SaverEncrypted.ENCRYPTION_KEY, byte[] iv = null) 
		{
			_filename = filename;
			_encryptionKey = encryptionKey;
			_iv = iv == null ? SaverEncrypted.IV : iv;
		}
		
		public void Save<T>(T data) where T : ISerializable
		{
			SaverEncrypted.Save(data, _filename, _encryptionKey, _iv);
		}
		public T Load<T>() where T : ISerializable, new()
		{
			return SaverEncrypted.Load<T>(_filename, _encryptionKey, _iv);
		}
		
		public static void Save<T>(T data, string filename, string encryptionKey = SaverEncrypted.ENCRYPTION_KEY, byte[] iv = null) where T : ISerializable
		{
			if (iv == null)
				iv = SaverEncrypted.IV;
				
			using(Stream stream = File.Open(filename, FileMode.Create)) {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Binder = new VersionDeserializationBinder(); 
				
				byte[] byKey = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
				using(DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
					using(Stream cryptoStream = new CryptoStream(stream, des.CreateEncryptor(byKey, iv), CryptoStreamMode.Write))
					{
						using(cryptoStream)
							binaryFormatter.Serialize(cryptoStream, data);
						cryptoStream.Close();
					}
				}
				stream.Close();
			}
		}
		
		public static T Load<T>(string filename, string encryptionKey = SaverEncrypted.ENCRYPTION_KEY, byte[] iv = null) where T : ISerializable, new()
		{
			T data = default(T);
			if (!File.Exists(filename))
				return data;
				
			if (iv == null)
				iv = SaverEncrypted.IV;

			using(Stream stream = File.Open(filename, FileMode.Open)) {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Binder = new VersionDeserializationBinder(); 
				
				byte[] byKey = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
					using (Stream cryptoStream = new CryptoStream(stream, des.CreateDecryptor(byKey, iv), CryptoStreamMode.Read))
					{
						try {
							data = (T) binaryFormatter.Deserialize(cryptoStream);
						} catch (System.Exception e) {
							Log.Warning("SaverEncrypted", e.ToString());
							return new T();
						}
						cryptoStream.Close();
					}
				}
				stream.Close();
			}
			return data;
		}
	
		private sealed class VersionDeserializationBinder : SerializationBinder 
		{ 
		    public override Type BindToType(string assemblyName, string typeName)
		    { 
		        if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName)) 
		        { 
		            Type typeToDeserialize = null;
		            assemblyName = Assembly.GetExecutingAssembly().FullName;
		            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName)); // The following line of code returns the type. 
		            return typeToDeserialize; 
		        } 
		        return null; 
		    } 
		}
	}
}