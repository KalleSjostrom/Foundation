using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Foundation {
	public abstract class BaseUnit : MonoBehaviour
	{
		public abstract int LayerMask { get; }
		public BaseTile OccupiedTile { get; set; }
	
	    public abstract bool CanWalkOn(BaseUnit unit); // Returns the CanWalkOver bool
	
		public virtual void OnLeaved(BaseTile tile) {}
		public virtual void OnCollided(BaseUnit unit) {}
		public virtual void OnArrived(BaseTile tile, List<BaseUnit> previousUnits) {}
		public virtual void OnArrivedToMe(BaseUnit unit) {}
	
		public virtual void DestroyUnit() {
			BaseTile.HandleOccupy(this, OccupiedTile, null); // Need to leave the grid before destroying!
			Destroy(gameObject);
		}
	}
}