using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {
	public interface IState 
	{
		void OnEnter(params object[] args);
		void Update();
		void OnExit();
	}
	
	public class StateMachine {
	
		private IState _currentState;
		private Dictionary<int, IState> _allStates;
		
		public IState CurrentState { get { return _currentState; } }
		
		public StateMachine() {}
		
		public void Setup(int startState, Dictionary<int, IState> allStates)
		{
			_currentState = allStates[startState];
			_currentState.OnEnter();
			_allStates = allStates;
		}
	
		public void ChangeState(int newState, params object[] args)
		{
			IState previous = _currentState;
			previous.OnExit();
			
			IState next = _allStates[newState];
			next.OnEnter(args);
			
			_currentState = next;
		}
		
		public void Update()
		{
			_currentState.Update();
		}
	}
}