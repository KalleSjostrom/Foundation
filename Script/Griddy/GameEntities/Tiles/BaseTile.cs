using UnityEngine;
using System;
using System.Collections.Generic;

namespace Foundation {
	public class BaseTile : ScriptableObject
	{
		public Vector3 Position;
		public int Id { get { return GridAux.PositionToId(Position); } }
		public BaseSoldier OccupyingUnit { get { return _occupyingUnit; } }
		[SerializeField]
		private BaseSoldier _occupyingUnit;
		
		public static void TeleportTo(BaseSoldier unit, BaseTile sourceTile, BaseTile destinationTile) 
		{
			if (destinationTile)
				unit.transform.position = destinationTile.Position;
	
			HandleOccupy(unit, sourceTile, destinationTile);
			HandleArrive(unit, sourceTile, destinationTile);
		}
		
		public static void HandleOccupy(BaseSoldier unit, BaseTile sourceTile, BaseTile destinationTile) 
		{
			if (sourceTile != null)
				sourceTile.Unoccupy(unit);
			if (destinationTile != null)
				destinationTile.Occupy(unit);
		}
		
		public static void HandleArrive(BaseSoldier unit, BaseTile sourceTile, BaseTile destinationTile) 
		{
			if (sourceTile != null)
				sourceTile.Leave(unit, destinationTile);
			if (destinationTile != null)
				destinationTile.Arrive(unit, sourceTile);
		}
	
		private void Occupy(BaseSoldier unit) 
		{
			_occupyingUnit = unit;
			unit.OccupiedTile = this;
		}
	
		private void Unoccupy(BaseSoldier unit) 
		{
			_occupyingUnit = null;
			unit.OccupiedTile = null;
		}
	
		private void Arrive(BaseSoldier unit, BaseTile sourceTile) {
			unit.OnArrived(this);
			OnArrived(unit, sourceTile);
		}
	
		private void Leave(BaseSoldier unit, BaseTile destinationTile) {
			unit.OnLeaved(this);
			OnLeaved(unit, destinationTile);
		}
	
		protected void OnLeaved(BaseSoldier unit, BaseTile sourceTile) {}
		protected void OnArrived(BaseSoldier unit, BaseTile destinationTile) {}
	}
}