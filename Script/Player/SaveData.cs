using UnityEngine;
using System.Runtime.Serialization;

namespace Foundation {
	[System.Serializable]
	public class SaveData : ISerializable {
	
		public int level;
		
		public SaveData() {}
		
		public SaveData(SerializationInfo info, StreamingContext context) 
		{
			level = (int)info.GetValue("level", typeof(int));
		}
	
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("level", level);
		}		
	}
}