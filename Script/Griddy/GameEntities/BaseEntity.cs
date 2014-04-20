using UnityEngine;
using System;
using System.Collections.Generic;
using Foundation;

namespace Foundation {
	public abstract class BaseEntity : MonoBehaviour, IActivatable 
	{
		protected GridManager GridManager { get { return _gridManager; } }
		private GridManager _gridManager;
		
		private bool _active = true;
	
		public virtual void Init(GridManager gridManager) {
			_gridManager = gridManager;
		}
		
		public void SetActive(bool active) {
			_active = active;
			GetComponent<MeshRenderer>().enabled = active;
			if (active)
				OnActivated();
			else
				OnDeactivated();
		}
	
		public bool IsActive() {
			return _active;
		}
	
		protected virtual void OnActivated() {}
		protected virtual void OnDeactivated() {}
	}
}