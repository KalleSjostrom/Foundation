using UnityEngine;
using System;
using System.Collections.Generic;

namespace Foundation {
	[Flags]
	public enum Layer {
		Ground = 1 << 0,
		Air = 1 << 1,
	}
	
	public abstract class BaseTile : BaseEntity
	{
		private Dictionary<Layer, BaseUnit> _occupyingUnits;
		private List<BaseUnit> _previousUnits;
	
		public override void Init(GridManager gridManager) {
			base.Init(gridManager);
			_occupyingUnits = new Dictionary<Layer, BaseUnit>();
			_previousUnits = new List<BaseUnit>();
		}
		
		public static void TeleportTo(BaseUnit unit, BaseTile sourceTile, BaseTile destinationTile) {
			Vector3 position = destinationTile.transform.position;
			position.y = unit.transform.position.y;
			unit.transform.position = position;
	
			HandleOccupy(unit, sourceTile, destinationTile);
			HandleArrive(unit, sourceTile, destinationTile);
		}
		public static void HandleOccupy(BaseUnit unit, BaseTile sourceTile, BaseTile destinationTile) {
			if (sourceTile != null)
				sourceTile.Unoccupy(unit);
			if (destinationTile != null)
				destinationTile.Occupy(unit);
		}
		public static void HandleArrive(BaseUnit unit, BaseTile sourceTile, BaseTile destinationTile) {
			if (sourceTile != null)
				sourceTile.Leave(unit, destinationTile);
			if (destinationTile != null)
				destinationTile.Arrive(unit, sourceTile);
		}
	
		private void Occupy(BaseUnit unit) {
			int mask = unit.LayerMask;
			foreach (Layer l in Enum.GetValues(typeof(Layer))) {
				int layerMask = (int)l;
				if ((layerMask & mask) == layerMask) {
					BaseUnit u;
					if (_occupyingUnits.TryGetValue(l, out u))
						_previousUnits.Add(u);
	
					_occupyingUnits[l] = unit;
				}
			}
			unit.OccupiedTile = this;
		}
	
		private void Unoccupy(BaseUnit unit) {
			int mask = unit.LayerMask;
			foreach (Layer l in Enum.GetValues(typeof(Layer))) {
				BaseUnit u;
				if (_occupyingUnits.TryGetValue(l, out u)) {
					int layerMask = (int)l;
					if ((layerMask & mask) == layerMask && unit == u)
						_occupyingUnits.Remove(l);
				}
			}
		}
	
		private void Arrive(BaseUnit unit, BaseTile sourceTile) {
			unit.OnArrived(this, _previousUnits);
			foreach (BaseUnit u in _previousUnits)
				u.OnArrivedToMe(unit);
	
			_previousUnits.Clear();
			OnArrived(unit, sourceTile);
		}
	
		private void Leave(BaseUnit unit, BaseTile destinationTile) {
			unit.OnLeaved(this);
			OnLeaved(unit, destinationTile);
		}
		
		public virtual bool CanWalkOn(BaseUnit unit) {
			foreach (BaseUnit u in OccupyingUnits(unit)) {
				if (u != unit && !u.CanWalkOn(unit.gameObject.tag))
					return false;
			}
			return true;
		}
		public BaseUnit GetOccupyingUnitOnLayer(Layer iLayer) {
			BaseUnit unit;
			_occupyingUnits.TryGetValue(iLayer, out unit);
			return unit;
		}
	
		protected abstract void OnLeaved(BaseUnit unit, BaseTile sourceTile);
		protected abstract void OnArrived(BaseUnit unit, BaseTile destinationTile);
	
		private static Array mLayers = Enum.GetValues(typeof(Layer));
		public IEnumerable<BaseUnit> OccupyingUnits(BaseUnit unit) {
			foreach (Layer l in mLayers) {
				BaseUnit u;
				if (_occupyingUnits.TryGetValue(l, out u)) {
					int layerMask = (int)l;
					if ((layerMask & unit.LayerMask) == layerMask)
						yield return u;
				}
			}
		}
	}
}