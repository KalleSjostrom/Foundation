using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Foundation {
	public class GridAux {
		private static int MAX_SIZE = 2^16;
		public static int PositionToId(Vector3 position) 
		{
			int x, y;
			PositionToIndices(position, out x, out y);
			return x + y * MAX_SIZE;
		}

		public static object IdToPosition(int id)
		{
			int x = id % MAX_SIZE;
			int y = id / MAX_SIZE;
#if Z_UP
			return new Vector3(x, y, 0);
#else
			return new Vector3(x, 0, y);
#endif
		}

		public static int IndicesToId(int x, int y) 
		{
			return x + y * MAX_SIZE;
		}
		
		public static void PositionToIndices(Vector3 position, out int xIndex, out int yIndex) 
		{
			xIndex = Mathf.RoundToInt(position.x);
#if Z_UP
			yIndex = Mathf.RoundToInt(position.y);
#else
			yIndex = Mathf.RoundToInt(position.z);
#endif
		}
		
		public static Vector2 PositionToIndices(Vector3 position)
		{
			int x, y;
			GridAux.PositionToIndices(position, out x, out y);
			return new Vector2(x, y);
		}
	}
	public class GridManager<T> where T : MonoBehaviour
	{
		private Dictionary<int, T> _grid;
		private List<T> _tilesForQueries;
	
		public GridManager() {}
	
		public void CreateGrid(Bounds levelBounds)
		{
			Vector3 size = levelBounds.size;
			int x, y;
			GridAux.PositionToIndices(size, out x, out y);
			x++; y++;
			_grid = new Dictionary<int, T>();
			_tilesForQueries = new List<T>(x * y);
		}
		
		public void AddTile(T tile) 
		{
			int id = GridAux.PositionToId(tile.transform.position);
			Log.Assert(!_grid.ContainsKey(id), "GridManager", "Can't add two tiles to same place! (tile={0}, position={1})", tile, tile.transform.position);
			_grid.Add(id, tile);
		}
		public void RemoveTile(T tile) 
		{
			int id = GridAux.PositionToId(tile.transform.position);
			Log.Assert(_grid.ContainsKey(id), "GridManager", "Couldn't find tile to remove! (tile={0}, position={1})", tile, tile.transform.position);
			_grid.Remove(id);
		}
	
		public T GetTile(int id) 
		{
			Log.Assert(_grid.ContainsKey(id), "GridManager", "Couldn't find tile! (id={0}, position={1})", id, GridAux.IdToPosition(id));
			return _grid[id];
		}
		public T GetTile(Vector3 position)
		{
			return GetTile(GridAux.PositionToId(position));
		}
		
		public T TryGetTile(int id)
		{
			T tile;
			if (_grid.TryGetValue(id, out tile))
				return tile;
			else
				return default(T);
		}
		public T TryGetTile(Vector3 position) {
			return TryGetTile(GridAux.PositionToId(position));
		}
		public T TryGetTile(int x, int y)
		{
			return TryGetTile(GridAux.IndicesToId(x, y));
		}
		
		public IEnumerable<T> GetTilesAround(Vector3 position)
		{
			int x, y;
			GridAux.PositionToIndices(position, out x, out y);
			
			T t = TryGetTile(x, y);
			if (t) yield return t;
			
			t = TryGetTile(x-1, y);
			if (t) yield return t;
			
			t = TryGetTile(x, y-1);
			if (t) yield return t;
			
			t = TryGetTile(x-1, y-1);
			if (t) yield return t;
		}
		
		public bool AreNeighbors(T tileA, T tileB, out Direction oDir)
		{
			Vector3 delta = tileA.transform.position - tileA.transform.position;
			int x = Mathf.RoundToInt(delta.x);
			int y = Mathf.RoundToInt(delta.y);
			
			oDir = Direction.Right;
			
			if (Mathf.Abs(x) + Mathf.Abs(y) != 1)
				return false;
			
			bool areNeighbors = true;
			if ((x < 0 || x > 0) && y == 0)
				oDir = x < 0 ? Direction.Left : Direction.Right;
			else if ((y < 0 || y > 0) && x == 0)
				oDir = y < 0 ? Direction.Down : Direction.Up;
			else
				areNeighbors = false;
			
			return areNeighbors;
		}
		
		public IEnumerable<T> AllTiles() 
		{
			foreach (var pair in _grid)
			{
				yield return pair.Value;
			}
		}
		
		public void GetAll<T2>(out List<T> tiles) where T2 : T
		{
			_tilesForQueries.Clear();
			foreach (var pair in _grid)
			{
				T tile = pair.Value;
				if (tile is T2)
					_tilesForQueries.Add(tile);
			}
			tiles = _tilesForQueries;
		}
	}
}
