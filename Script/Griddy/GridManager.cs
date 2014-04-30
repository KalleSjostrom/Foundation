using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Foundation {
	public class GridAux {
		public static int PositionToId(Vector3 position) 
		{
			int x, y;
			PositionToIndices(position, out x, out y);
			return x + y * 2^16;
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
	public class GridManager<T> where T : BaseTile
	{
		private T[,] _grid;
		private List<T> _tilesForQueries;
		private Iterator _iterator;
	
		public GridManager() {}
	
		public void CreateGrid(int sizeX, int sizeY)
		{
			_grid = new T[sizeX, sizeY];
			_tilesForQueries = new List<T>(sizeX * sizeY);
		}
		
		public void AddTile(T tile) {
			int x, y;
			GridAux.PositionToIndices(tile.transform.position, out x, out y);
			Log.Assert(InRange(x, y), "GridManager", "Can't add tile outside of grid! (x={0}, y={1}, size x={2}, size y={3})", x, y, _grid.GetLength(0), _grid.GetLength(1));
			_grid[x, y] = tile;
		}
		public void RemoveTile(T tile) {
			int x, y;
			GridAux.PositionToIndices(tile.transform.position, out x, out y);
			_grid[x, y] = null;
		}
	
		public bool InRange(Vector3 position) {
			int x, y;
			GridAux.PositionToIndices(position, out x, out y);
			return InRange(x, y);
		}
		public bool InRange(int xIndex, int yIndex) 
		{
			return xIndex < _grid.GetLength(0) && xIndex >= 0 &&
				   yIndex < _grid.GetLength(1) && yIndex >= 0;
		}
		
		public T GetTile(int address) 
		{
			Log.Assert(_grid.GetLength(0) > 0, "GridManager", "Can't get tile of a zero-sized grid!");
			int x = address % _grid.GetLength(0);
			int y = address / _grid.GetLength(0);
			return GetTile(x, y);
		}
	
		public T GetTile(Vector3 position)
		{
			int x, y;
			GridAux.PositionToIndices(position, out x, out y);
			return GetTile(x, y);
		}
		public T GetTile(int xIndex, int yIndex)
		{
			if (InRange(xIndex, yIndex))
				return _grid[xIndex, yIndex];
	
			return null;
		}
	
		protected void RemoveTile(int xIndex, int yIndex)
		{
			_grid[xIndex, yIndex] = default(T);
		}
		
		public int GetLength(int dimension)
		{
			return _grid.GetLength(dimension);
		}
		
		public IEnumerable<T> GetTilesAround(Vector3 position)
		{
			int x, y;
			GridAux.PositionToIndices(position, out x, out y);
			
			T t = GetTile(x, y);
			if (t) yield return t;
			
			t = GetTile(x-1, y);
			if (t) yield return t;
			
			t = GetTile(x, y-1);
			if (t) yield return t;
			
			t = GetTile(x-1, y-1);
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
		
		public void GetAll<T2>(out List<T> tiles) where T2 : T
		{
			_tilesForQueries.Clear();
			
			Iterator it = GetIterator();
			while (it.HasNext())
			{
				T t = it.Next();
				if (t != null && t is T2)
				{
					_tilesForQueries.Add(t);	
				}
			}
			
			tiles = _tilesForQueries;
		}
	
		public Iterator GetIterator()
		{
			if (_iterator == null)
			{
				_iterator = new Iterator();
				_iterator.Initialize(_grid);
			}
			
			_iterator.Reset();
			return _iterator;
		}
		
		public class Iterator
		{
			private int _cursorX = 0;
			private int _cursorY = 0;
			
			private int _sizeX;
			private int _sizeY;
			
			private T[,] _grid;
			
			public void Initialize(T[,] grid)
			{
				_grid = grid;
				_sizeX = _grid.GetLength(0);
				_sizeY = _grid.GetLength(1);
			}
			
			public void Reset()
			{
				_cursorX = 0;
				_cursorY = 0;
			}
			
			public T Next()
			{
				_cursorX++;
				if (_cursorX >= _sizeX)
				{
					_cursorX = 0;
					_cursorY++;
					if (_cursorY >= _sizeY)
						return default(T);
				}
				return _grid[_cursorX, _cursorY];
			}
			
			public bool HasNext()
			{
				return _cursorY < _grid.GetLength(1);
			}
		}
	}
}
