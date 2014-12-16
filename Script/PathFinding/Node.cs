using System;
using UnityEngine;

namespace Foundation {
	sealed class Node : IComparable<Node> {
	
	    public Vector3 Position {get; private set;}
	    public Node Parent {get; private set;}
	    public int Cost {get; private set;}
	    public int Heuristic {get; private set;}
	
	    public Node(Vector3 position, Node parent, int cost, int heuristic) 
		{
			Position = position;
	        Parent = parent;
	        Cost = cost;
	        Heuristic = heuristic;
	    }
	
	    public int CompareTo(Node node)
		{
	        return GetTotalCost() < node.GetTotalCost() ? -1 : 1;
	    }
	
	    public int GetTotalCost() 
		{
	        return Cost + Heuristic;
	    }
	}
}