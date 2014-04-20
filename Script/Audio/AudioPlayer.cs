using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Foundation {
	public class AudioPlayer : MonoBehaviour 
	{
		private AudioSource _audio;
	
		public void Start() {
			_audio = this.GetComponent<AudioSource>();
		}
	
		public void FadeOut(float duration) {
			StartCoroutine(Fade(duration));
		}
	
		private IEnumerator Fade(float duration) {
			float timer = 0;
			while (timer < duration) {
				float t = 1 - (timer / duration);
				_audio.volume = t;
				timer += Time.deltaTime;
				yield return 0;
			}
		}
	}
}
