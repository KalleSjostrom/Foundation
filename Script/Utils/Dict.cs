using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Foundation;
using System.Runtime.Serialization;

namespace Foundation {
	public abstract class Dict<TKey, TValue> : ISerializationCallbackReceiver {
		private List<TKey> _keys;
		private List<TValue> _values;
		private Dictionary<TKey, TValue>  _dictionary;
		
		public Dictionary<TKey, TValue>.ValueCollection Values { get { return _dictionary.Values; } }
		public TValue this[TKey key]
		{
			get { return _dictionary[key]; }
			set { _dictionary[key] = value; }
		}
		public Dict() {	
			_keys = new List<TKey>();
			_values = new List<TValue>();
			_dictionary = new Dictionary<TKey, TValue>();
		}
		public void Add(TKey key, TValue value) {
			_dictionary.Add(key, value);
		}
		
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (KeyValuePair<TKey, TValue> pair in _dictionary)
				yield return pair;
		}
		
		public bool ContainsKey(TKey key) {
			return _dictionary.ContainsKey(key);
		}
		
		public bool Remove(TKey key) {
			return _dictionary.Remove(key);
		}
		
		public void OnBeforeSerialize() {
			_keys.Clear();
			_values.Clear();
			
			foreach(var kvp in _dictionary) {
				_keys.Add(kvp.Key);
				_values.Add(kvp.Value);
			}
		}
		public void OnAfterDeserialize() {
#if DEBUG_DICT
			if (_keys.Count != _values.Count) {
				Debug.Log("Keys:");
				foreach (TKey k in _keys)
					Debug.Log(k);
					
				Debug.Log("Vals:");
				foreach (TValue v in _values)
					Debug.Log(v);
			}
#endif
			Log.Assert(_keys.Count == _values.Count, "Dictionary tried to deserialize but nr keys do no longer match nr values! (nr_keys={0}, nr_values={1})", _keys.Count, _values.Count);
			
			_dictionary.Clear();
			for (int i=0; i!= Math.Min(_keys.Count, _values.Count); i++)
				_dictionary.Add(_keys[i],_values[i]);
		}
	}
}