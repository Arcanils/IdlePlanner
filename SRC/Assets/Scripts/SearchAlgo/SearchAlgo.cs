using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAlgo
{
	public struct Node
	{
		public Point point;
		public int Cost;
		public int Heuristic;
	}

	public struct Point
	{
		public int x, y;
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public 

	public Queue<Vector2Int> GetPathToTarget<T>(Dictionary<Vector2Int, T> map, Vector2Int start, Vector2Int goal, int maxLength)
	{
		var startNode = new Node();
		var goalNode = new Node();
		var openSet = new List<Node>(); // Sorted by heurestic
		// openSet.Add(start);
		var closedSet = new List<Node>();
		var cameFrom = new Dictionary<Node, Node>();
		var gScore = new Dictionary<Node, int>();
		gScore[startNode] = 0;

		var fScore = new Dictionary<Node, int>();
		fScore[startNode] = HeuristicCostEstimate(start, goal);

		var neighbors = new List<Node>(4);
		var position = new Vector2Int();
	
		while (openSet.Count != 0)
		{
			var current = openSet[0];

			if (current == goalNode)
				return null; // Return le path construit


			openSet.RemoveAt(0);
			closedSet.Add(current);

			neighbors = GetSidesNodes();


			for (int i = neighbors.Count - 1; i >= 0; --i)
			{
				var nodeTest = neighbors[i];
				if (!cameFrom.ContainsKey(nodeTest))
				{
					openSet.Add(nodeTest);
				}

				var tentativeGScore = gScore[current] + GetDistance(current, nodeTest);
				if (tentativeGScore >= gScore[current])
				{
					continue;
				}

				cameFrom[nodeTest] = current;
				gScore[nodeTest] = tentativeGScore;
				fScore[nodeTest] = gScore[nodeTest] + HeuristicCostEstimate(nodeTest, goalNode);
			}
		}

		return null;
	}
	/*
	function reconstruct_path(cameFrom, current)

	total_path := [current]
    while current in cameFrom.Keys:
        current := cameFrom[current]
		total_path.append(current)
    return total_path
		*/

	//https://en.wikipedia.org/wiki/A*_search_algorithm
}
