using System;
using UnityEngine;

namespace Foundation {
	sealed class Node : IComparable<Node> {
	
	    public Vector2 Indices {get; private set;}
	    public Node Parent {get; private set;}
	    public int Cost {get; private set;}
	    public int Heuristic {get; private set;}
	
	    public Node(Vector2 indices, Node parent, int cost, int heuristic) 
		{
	        Indices = indices;
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