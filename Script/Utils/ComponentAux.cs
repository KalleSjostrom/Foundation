using UnityEngine;
using System.Collections;
using System;

namespace Foundation {
	public class ComponentAux
	{
		public static T Create<T>(String name) where T : UnityEngine.Component
		{
			GameObject go = new GameObject(name, typeof(T));
			return go.GetComponent<T>();
		}
		
		public static T Create<T>(String name, GameObject parent) where T : UnityEngine.Component
		{
			GameObject go = new GameObject(name, typeof(T));
			go.transform.parent = parent.transform;
			return go.GetComponent<T>();
		}
		
		public static T CreateAndAdd<T>(String name) where T : UnityEngine.Component
		{
			GameObject go = new GameObject(name, typeof(T));
			return go.AddComponent<T>();
		}
		
		public static T CreateAndAdd<T>(String name, GameObject parent) where T : UnityEngine.Component
		{
			GameObject go = new GameObject(name, typeof(T));
			go.transform.parent = parent.transform;
			return go.AddComponent<T>();
		}
		
		public static T Instansiate<T>(GameObject prefab) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			return go.GetComponent<T>();
		}
		public static T Instansiate<T>(GameObject prefab, GameObject parent) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.parent = parent.transform;
			return go.GetComponent<T>();
		}
		
		public static T InstansiateAndAdd<T>(GameObject prefab) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			return go.AddComponent<T>();
		}
		public static T InstansiateAndAdd<T>(GameObject prefab, GameObject parent) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.parent = parent.transform;
			return go.AddComponent<T>();
		}
		
		public static T Find<T>(String name) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Find(name) as GameObject;
			return go.GetComponent<T>();
		}
		public static T Find<T>(String name, GameObject parent) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Find(name) as GameObject;
			go.transform.parent = parent.transform;
			return go.GetComponent<T>();
		}
		
		public static T FindAndAdd<T>(String name) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Find(name) as GameObject;
			return go.AddComponent<T>();
		}
		public static T FindAndAdd<T>(String name, GameObject parent) where T : UnityEngine.Component
		{
			GameObject go = GameObject.Find(name) as GameObject;
			go.transform.parent = parent.transform;
			return go.AddComponent<T>();
		}
	}
}