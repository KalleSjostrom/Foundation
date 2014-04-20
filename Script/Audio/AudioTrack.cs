using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {
	public class AudioTrack : MonoBehaviour
	{
		public List<AudioClip> intros;
		public List<AudioClip> clips;
		public List<AudioClip> outros;
		
		public float volume = 1;
		public float fadeInDuration = 0.5f;
		public float fadeOutDuration = 0.5f;
		
		public bool keepPlaying = false;
		
		public Vector2 nrLoopsRange;
		public Vector2 randomTimeRange;
		
		private AudioSource _introSource;
		private AudioSource _mainSource;
		private AudioSource _outroSource;
		
		private float _targetVolume;
		
		void Start() {
		}
	
		public void Setup ()
		{
			_introSource = gameObject.AddComponent<AudioSource>();
			_mainSource = gameObject.AddComponent<AudioSource>();
			_outroSource = gameObject.AddComponent<AudioSource>();
		}
		
		public float Play()
		{		
			// Debug.Log("Starting play");
			float delay = 0;
			if (intros.Count > 0) {
				int index = Random.Range(0, intros.Count);
				AudioClip iclip = intros[index];
				_introSource.clip = iclip;
				_introSource.Play();
				_introSource.volume = 0;
				_mainSource.volume = volume;
				delay = iclip.length;
				
				StartCoroutine(FadeIn(0, fadeInDuration, _introSource));
			} else {
				_mainSource.volume = 0;
				StartCoroutine(FadeIn(0, fadeInDuration, _mainSource));
			}
			
			float duration = 0;
			if (clips.Count > 0) {
				AudioClip clip = clips[Random.Range(0, clips.Count)];
				_mainSource.clip = clip;
				_mainSource.PlayDelayed(delay);
				_mainSource.loop = true;
			
				int nrLoops = Random.Range((int) nrLoopsRange.x, (int) nrLoopsRange.y);
				if (nrLoops > 0) {
					duration = (nrLoops * clip.length) + delay;
					if (!keepPlaying)
						StartCoroutine(TurnOffLooping(duration - 1, _mainSource)); // Safe with one second.
				} else {
					duration = Random.Range(randomTimeRange.x, randomTimeRange.y) + delay;
				}
			} else {
				duration = Random.Range(randomTimeRange.x, randomTimeRange.y) + delay;
			}
			
			if (outros.Count > 0) {
				AudioClip oclip = outros[Random.Range(0, outros.Count)];
				_outroSource.clip = oclip;
				_outroSource.PlayDelayed(duration);
				_outroSource.volume = volume;
				duration += oclip.length;
				StartCoroutine(FadeOut(duration - fadeOutDuration, fadeOutDuration, _outroSource));
			} else if (!keepPlaying) {
				StartCoroutine(FadeOut(duration - fadeOutDuration, fadeOutDuration, _mainSource));
			}
			
			if (keepPlaying) {
				AudioClip mainClip = _mainSource.clip;
				
				intros.Clear();
				clips.Clear();
				outros.Clear();
				
				if ((nrLoopsRange.x > 0 || nrLoopsRange.y > 0) && mainClip != null) {
					randomTimeRange.x = (nrLoopsRange.x * mainClip.length);
					randomTimeRange.y = (nrLoopsRange.y * mainClip.length);
				}	
			}
			
			return duration;
		}
		
		public void FadeOut(float fadeDuration) {
			StartCoroutine(FadeOut(0, fadeDuration, _introSource));
			StartCoroutine(FadeOut(0, fadeDuration, _mainSource));
			StartCoroutine(FadeOut(0, fadeDuration, _outroSource));
		}
		
		private IEnumerator TurnOffLooping(float delay, AudioSource source) {
			yield return new WaitForSeconds(delay);
			source.loop = false;
		}
		
		private IEnumerator FadeIn(float delay, float fadeDuration, AudioSource source) {
			return Fade(delay, fadeDuration, source, volume);
		}
		private IEnumerator FadeOut(float delay, float fadeDuration, AudioSource source) {
			return Fade(delay, fadeDuration, source, 0);
		}
		
		private IEnumerator Fade(float delay, float fadeDuration, AudioSource source, float target) {
			yield return new WaitForSeconds(delay);
			
			float startVolume = source.volume;
			_targetVolume = target;
			float timer = 0;
			// Debug.Log("Start fade " + source.volume + " " + target);
			while (timer < fadeDuration) {
				float t = timer / fadeDuration;
				if (_targetVolume != target)
					break;
					
				source.volume = Mathf.Lerp(startVolume, target, t);
				timer += Time.deltaTime;
				yield return 0;
			}
			
			// Debug.Log("Fade done " + _targetVolume + " " + target + " " + (_targetVolume == target));
			if (_targetVolume == target) {
				source.volume = target;
			}
		}
	}
}
