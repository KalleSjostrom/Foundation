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
		
		/*IEnumerator IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}*/
		
		public void OnBeforeSerialize() {
			_keys.Clear();
			_values.Clear();
			
			foreach(var kvp in _dictionary) {
				_keys.Add(kvp.Key);
				_values.Add(kvp.Value);
			}
		}
		public void OnAfterDeserialize() {
			_dictionary.Clear();
			for (int i=0; i!= Math.Min(_keys.Count,_values.Count); i++)
				_dictionary.Add(_keys[i],_values[i]);
		}
	}
}