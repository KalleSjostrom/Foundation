using UnityEngine;
using System.Collections;

namespace Foundation {
	public class HudFps : MonoBehaviour 
	{
		public  float updateInterval = 0.5f;
		
		private float _accum = 0;
		private int _frames = 0;
		private float _timeleft;
		
		void Start()
		{
			if(!guiText)
			{
				Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
				enabled = false;
				return;
			}
			_timeleft = updateInterval;
		}
		
		void Update()
		{
			_timeleft -= Time.deltaTime;
			_accum += Time.timeScale/Time.deltaTime;
			++_frames;
			
			if(_timeleft <= 0.0)
			{
				float fps = _accum/_frames;
				string format = System.String.Format("FPS:{0:F2} ({1:F2})",fps, 1/Time.smoothDeltaTime);
				guiText.text = format;
				
				if (fps < 30)
					guiText.material.color = Color.yellow;
				else 
					if (fps < 10)
						guiText.material.color = Color.red;
				else
					guiText.material.color = Color.green;
					
				_timeleft = updateInterval;
				_accum = 0.0F;
				_frames = 0;
			}
		}
	}
}