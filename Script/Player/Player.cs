using UnityEngine;
using System.Collections;

namespace Foundation {
	public class Player : MonoBehaviour
	{
		public SaveData saveData;
		
		public int Level { get { return saveData.level; } set { saveData.level = value; } }
		private SaverEncrypted _saver;
	
		void Awake() 
		{
			DontDestroyOnLoad(gameObject);
			_saver = new SaverEncrypted("save01.sav");
		}
	
		public void Save() 
		{
			_saver.Save<SaveData>(saveData);
		}
		
		public static Player CreatePlayer() 
		{
			bool hasPlayer = GameObject.Find("Player") != null;
			Player player;
			if (hasPlayer) {
				player = ComponentAux.Find<Player>("Player");
			} else {
				player = ComponentAux.Create<Player>("Player");
				player.saveData = player._saver.Load<SaveData>();
			}
			
			return player;
		}
	}
}
