// #define FOUNDATION_DEBUG_PATHFINDER

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Foundation {
	[Serializable]
	public class PathFinder
	{
		private static Vector3[] DIRECTIONS = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
	
		private IPriorityQueue<Node> _priorityQueue;
		private HashSet<int> _closedSet = new HashSet<int>();
		private GridManager _gridTiles;
	
		public PathFinder(GridManager gridTiles) 
		{
			_gridTiles = gridTiles;
		}
		
		public int GetCostTo(Vector3 startPosition, Vector3 endPosition, BaseSoldier unit) {
			Node n = GoTo(startPosition, endPosition, unit);
			int cost = n.GetTotalCost();
			return cost;
		}
	
		public Vector2[] GetPathTo(Vector3 startPosition, Vector3 endPosition, BaseSoldier unit, out int cost)
		{
			Node n = GoTo(startPosition, endPosition, unit);
			cost = n.GetTotalCost();
	
			bool foundTarget = n.Position == endPosition;
			if (!foundTarget) 
				return null;
			
			List<Node> nodes = new List<Node>();
			Node node = n; // Starts at the target and backtrace through parents back to beginning.
			while (node != null)
			{
				nodes.Add(node);
				node = node.Parent;
			}
	
			// Construct a path where each entry is one move in some direction.
			Vector2[] path = new Vector2[nodes.Count-1];
			int counter = 0;
			for (int i = nodes.Count-1; i > 0; i--) {
				path[counter] = nodes[i-1].Position - nodes[i].Position; // The direction is the difference in .
				counter++;
			}
	
			return path;
		}
		
		private Node GoTo(Vector3 start, Vector3 target, BaseSoldier unit)
		{
#if (FOUNDATION_DEBUG_PATHFINDER)
			Log.Debug("PathFinder", "Go to (start={0}, target={1})", start, target);
#endif
			int h1 = GetManhattanDistance(start, target);
			
			_closedSet = new HashSet<int>();
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
		
		private void Expand(Node parent, ref Vector3 target, BaseSoldier unit)
		{
#if (FOUNDATION_DEBUG_PATHFINDER)
			Log.Debug("PathFinder", "Expand node at position={0}", parent.Position);
#endif
			
			bool okToAdd = Close(parent);
			
			if (!okToAdd) 
				return;
			
			for (int i = 0; i < 4; ++i) {
				Vector3 position = parent.Position + DIRECTIONS[i];
				AddNode(position, target, parent, unit);
			}
		}
		
		private void AddNode(Vector3 position, Vector3 target, Node parent, BaseSoldier unit) 
		{
			int address = GridAux.PositionToId(position);
#if (FOUNDATION_DEBUG_PATHFINDER)
			Log.Debug("PathFinder", "address={0}, position={1}", address, position);
#endif
	
			if (!_closedSet.Contains(address))
			{
				BaseTile t = _gridTiles.TryGetTile(position);
				if (t != null && unit.CanWalkPast(t)) 
				{
					int cost = (parent == null ? 0 : parent.Cost) + t.GetCost(unit);
					int heuristic = GetManhattanDistance(position, target);
#if (FOUNDATION_DEBUG_PATHFINDER)
					Log.Debug("PathFinder", "adding node. (position={0}, heuristic={1}, cost={2})", position, heuristic, cost);
#endif
					
					_priorityQueue.Enqueue(new Node(position, parent, cost, heuristic));
				} 
				else 
				{
					Close(position);
				}
			}
		}
		
		private static int GetManhattanDistance(Vector2 a, Vector2 b) {
			return (int) Mathf.Abs(a.x - b.x) + (int) Mathf.Abs(a.y - b.y);
		}
		private bool Close(Node n) {
			return Close(n.Position);
		}
		private bool Close(Vector3 position) {
			int id = GridAux.PositionToId(position);
			return _closedSet.Add(id);
		}
	}
}
