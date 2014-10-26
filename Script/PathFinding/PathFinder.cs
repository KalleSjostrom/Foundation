#define FOUNDATION_DEBUG_PATHFINDER

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Foundation {
	public class PathFinder<T> where T : BaseTile
	{
		private static Vector2[] DIRECTIONS = { Vector2.up, -Vector2.up, Vector2.right, -Vector2.right };
	
		private IPriorityQueue<Node> _priorityQueue;
		private HashSet<int> _closedSet = new HashSet<int>();
	
		private List<BaseTile> _tilesForQueries;
		private GridManager<T> _gridManager;
	
		public PathFinder(GridManager<T> gridManager) 
		{
			_gridManager = gridManager;
		}
	
		public Vector2[] GetPathTo(Vector3 startPosition, Vector3 endPosition, BaseUnit unit, out int cost)
		{
			Vector2 startIndices = GridAux.PositionToIndices(startPosition);
			Vector2 endIndices = GridAux.PositionToIndices(endPosition);
	
			Node n = GoTo(startIndices, endIndices, unit);
			cost = n.GetTotalCost();
	
			bool foundTarget = n.Indices == endIndices;
			if (!foundTarget) 
				return null;
			
			List<Vector2> indices = new List<Vector2>();
			Node node = n; // Starts at the target and backtrace through parents back to beginning.
			while (node != null)
			{
				indices.Add(node.Indices);
				node = node.Parent;
			}
	
			// Construct a path where each entry is one move in some direction.
			Vector2[] path = new Vector2[indices.Count-1];
			int counter = 0;
			for (int i = indices.Count-1; i > 0; i--) {
				path[counter] = indices[i-1] - indices[i]; // The direction is the difference in indices.
				counter++;
			}
	
			return path;
		}
	
		// Converts indices in the grid to a uniqe number
		private int GetAddress(Vector2 indices) {
			int x = (int)indices.x * 1000; // Implies that we cant have rows/columns with more than 1000 tiles.
			int y = (int)indices.y;
			return x + y;
		}
		
		private Node GoTo(Vector2 start, Vector2 target, BaseUnit unit)
		{
#if (FOUNDATION_DEBUG_PATHFINDER)
			Log.Debug("PathFinder", "Go to (start={0}, target={1})", start, target);
#endif
			int h1 = GetManhattanDistance(start, target);
			
			_priorityQueue = new BinaryHeap<Node>(25);
			_priorityQueue.Enqueue(new Node(start, null, 0, h1));
			
			Node bestGuess = null;
			int heuristic = int.MaxValue;
			while (heuristic > 0 && !_priorityQueue.IsEmpty()) 
			{
				bestGuess = _priorityQueue.Dequeue();
				heuristic = bestGuess.Heuristic;
#if (FOUNDATION_DEBUG_PATHFINDER)
				Log.Debug("PathFinder", "heruistic={0}, best total cost={1}", heuristic, bestGuess.GetTotalCost());
#endif
				if (heuristic == 0)
					break;
				
				Expand(bestGuess, ref target, unit);
#if (FOUNDATION_DEBUG_PATHFINDER)
				Log.Debug("PathFinder", "queue empty={0}", _priorityQueue.IsEmpty());
#endif
			}
			_closedSet.Clear();
			_priorityQueue.Clear();
			return bestGuess;
		}
		
		private static int GetManhattanDistance(Vector2 a, Vector2 b) 
		{
			return (int) Mathf.Abs(a.x - b.x) + (int) Mathf.Abs(a.y - b.y);
		}
		
		private void Expand(Node parent, ref Vector2 target, BaseUnit unit)
		{
			Vector2 currentIndices = parent.Indices;
#if (FOUNDATION_DEBUG_PATHFINDER)
			Log.Debug("PathFinder", "expand node at indices {0}", currentIndices);
#endif
			
			bool okToAdd = _closedSet.Add(GetAddress(currentIndices));
			
			if (!okToAdd) 
				return;
			
			for (Direction i = Direction.Up; i <= Direction.Left; i++) 
			{ 
				Vector2 nextNodeIndices = currentIndices + DIRECTIONS[(int)i];
				AddNode(nextNodeIndices, target, parent, unit);
			}
		}
		
		private void AddNode(Vector2 indices, Vector2 target, Node parent, BaseUnit unit) 
		{
			int address = GetAddress(indices);
#if (FOUNDATION_DEBUG_PATHFINDER)
			Log.Debug("PathFinder", "address={0}, indices={1}", address, indices);
#endif
	
			if (!_closedSet.Contains(address))
			{
				BaseTile t = _gridManager.GetTile(address);
				if (t != null && t.CanWalkOn(unit)) 
				{
					int cost = (parent == null ? 0 : parent.Cost) + 1; // TODO: Add different costs for different tiles
					int heuristic = GetManhattanDistance(indices, target);
#if (FOUNDATION_DEBUG_PATHFINDER)
					Log.Debug("PathFinder", "adding node. (indices={0}, heuristic={1}, cost={2})", indices, heuristic, cost);
#endif
					
					_priorityQueue.Enqueue(new Node(indices, parent, cost, heuristic));
				} 
				else 
				{
					_closedSet.Add(GetAddress(indices));
				}
			}
		}
	}
}
