using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Foundation {
	public class GridManager
	{
		private BaseTile[,] _grid;
		private List<BaseTile> _tilesForQueries;
		private Iterator _iterator;
	
		public GridManager() {}
	
		public void CreateGrid(int sizeX, int sizeY)
		{
			_grid = new BaseTile[sizeX, sizeY];
			_tilesForQueries = new List<BaseTile>(sizeX * sizeY);
		}
	
		public void PositionToIndices(Vector3 position, out int xIndex, out int yIndex) {
			xIndex = Mathf.RoundToInt(position.x);
			yIndex = Mathf.RoundToInt(position.z);
		}
	
		public void AddTile(BaseTile tile) {
			int x, y;
			PositionToIndices(tile.transform.position, out x, out y);
			_grid[x, y] = tile;
		}
		public void RemoveTile(BaseTile tile) {
			int x, y;
			PositionToIndices(tile.transform.position, out x, out y);
			_grid[x, y] = null;
		}
	
		public bool InRange(Vector3 position) {
			int x, y;
			PositionToIndices(position, out x, out y);
			return InRange(x, y);
		}
		public bool InRange(int xIndex, int yIndex) 
		{
			return xIndex < _grid.GetLength(0) && xIndex >= 0 &&
				   yIndex < _grid.GetLength(1) && yIndex >= 0;
		}
	
		public BaseTile GetTile(Vector3 position)
		{
			int x, y;
			PositionToIndices(position, out x, out y);
			return GetTile(x, y);
		}
		public BaseTile GetTile(int xIndex, int yIndex)
		{
			if (InRange(xIndex, yIndex))
				return _grid[xIndex, yIndex];
	
			return null;
		}
	
		public Vector2 PositionToIndices(Vector3 position)
		{
			return new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
		}
	
		// Gets the tile at the given indices
		protected void RemoveTile(int xIndex, int yIndex)
		{
			_grid[xIndex, yIndex] = default(BaseTile);
		}
		
		public int GetLength(int dimension)
		{
			return _grid.GetLength(dimension);
		}
		
		public void GetAll<T>(out List<BaseTile> tiles) where T : BaseTile
		{
			_tilesForQueries.Clear();
			
			Iterator it = GetIterator();
			while (it.HasNext())
			{
				BaseTile t = it.Next();
				if (t != null && t is T)
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
			
			private BaseTile[,] _grid;
			
			public void Initialize(BaseTile[,] grid)
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
			
			public BaseTile Next()
			{
				_cursorX++;
				if (_cursorX >= _sizeX)
				{
					_cursorX = 0;
					_cursorY++;
					if (_cursorY >= _sizeY)
						return default(BaseTile);
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
