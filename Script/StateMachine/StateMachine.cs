using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {
	public interface BaseState 
	{
		void OnEnter(params object[] args);
		void Update();
		void OnExit();
	}
	
	public class StateMachine {
	
		private BaseState _currentState;
		private Dictionary<int, BaseState> _allStates;
		
		public StateMachine() {}
		
		public void Setup(int startState, Dictionary<int, BaseState> allStates)
		{
			_currentState = allStates[startState];
			_currentState.OnEnter();
			_allStates = allStates;
		}
	
		public void ChangeState(int newState, params object[] args)
		{
			BaseState previous = _currentState;
			previous.OnExit();
			
			BaseState next = _allStates[newState];
			next.OnEnter(args);
			
			_currentState = next;
		}
		
		public void Update()
		{
			_currentState.Update();
		}
	}
}