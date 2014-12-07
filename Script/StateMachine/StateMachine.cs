using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {

	public class StateAux {
		public static void AddState<T>(StateDictionary states, int state, params object[] args) where T : IState {
			T instance = ScriptableObject.CreateInstance<T>();
			instance.Initialize(args);
			states.Add(state, instance);
		}
	}

	[System.Serializable]
	public class StateDictionary : Dict<int, IState>{}
	
	public abstract class IState : ScriptableObject
	{
		public abstract void Initialize(params object[] args);
		
		public abstract void OnEnter(params object[] args);
		public abstract void Update();
		public abstract void OnExit();
	}

	[System.Serializable]
	public class StateMachine {
		private IState _currentState;
		[SerializeField]
		private StateDictionary _allStates;
		public IState CurrentState { get { return _currentState; } }
		
		public void Setup(int startState, StateDictionary allStates) {
			_currentState = allStates[startState];
			_currentState.OnEnter();
			_allStates = allStates;
		}
	
		public void ChangeState(int newState, params object[] args) {
			IState previous = _currentState;
			previous.OnExit();
			IState next = _allStates[newState];
			next.OnEnter(args);
			_currentState = next;
		}
		
		public void Update() {
			_currentState.Update();
		}
	}
}