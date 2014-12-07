using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugScreenText : MonoBehaviour {

	private List<string> strings = new List<string>();

	public void ScreenText(string text) {
		strings.Add(text);
	}
	
	void Update() {
		strings.Clear();
	}
	
	void OnGUI() {
		for (int i = 0; i < strings.Count; ++i) {
			string s = strings[i];
			GUI.Label(new Rect(10, 10 + i * 13, 1000, 20), s);
		}
	}
}
