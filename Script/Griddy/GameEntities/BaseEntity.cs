using UnityEngine;

using System;
using System.Collections.Generic;

using Foundation;

namespace Foundation {
	public abstract class BaseEntity : MonoBehaviour, IActivatable 
	{
		public int Id { get { return GridAux.PositionToId(transform.position); } }
		private bool _active = true;
	
		public virtual void Init() {}
		
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