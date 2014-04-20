using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {
	public class AudioPlayerAdvanced : MonoBehaviour, IAudioPlayer 
	{
		public List<TrackInfo> tracks;
			
		void Start() 
		{
			Log.Assert(tracks.Count > 0, "AudioPlayerAdvanced", "There is no tracks specified!");
			
			foreach (TrackInfo info in tracks) {
				AudioTrack track = ComponentAux.Instansiate<AudioTrack>(info.audioTrackPrefab.gameObject, this.gameObject);
				info.track = track;
				track.Setup();
			}
			
			NextStep(tracks[0]); // Always start with the first track
		}
		
		public void NextStep(TrackInfo trackInfo) 
		{
			float duration = trackInfo.track.Play();
			TrackInfo nextTrack = PickATrack(trackInfo);
			duration += nextTrack.startTime;
			Log.Debug("AudioPlayerAdvanced", "Starting track, {0} seconds until next.", duration);
			StartCoroutine(MakeNextDecisionIn(duration, nextTrack));
		}
		
		public IEnumerator MakeNextDecisionIn(float delay, TrackInfo nextTrack) 
		{
			yield return new WaitForSeconds(delay);
			NextStep(nextTrack);
		}
		
		public TrackInfo PickATrack(TrackInfo excludedInfo) 
		{
			float r = Random.value;
			float p = 0;
			float totalWeight = 0;
			foreach (TrackInfo info in tracks) {
				if (info != excludedInfo)
					totalWeight += info.weight;
			}
			
			if (totalWeight == 0)
				return PickATrack(null);
			
			foreach (TrackInfo info in tracks) {
				if (info == excludedInfo)
					continue;
					
				float probability = info.weight / totalWeight;
				p += probability;
				if (r < p) {
					return info;
				}
			}
			Log.Assert(false, "AudioPlayerAdvanced", "Found no track to play!?");
			return null;
		}
		
		public void FadeOut(float fadeDuration)
		{
			foreach (TrackInfo info in tracks) {
				info.track.FadeOut(fadeDuration);
			}
		}
	}
	
	[System.Serializable]
	public class TrackInfo
	{
		public AudioTrack audioTrackPrefab;
		public float weight = 1;
		public float startTime = 0;
		
		public AudioTrack track { get; set; }
	}
}