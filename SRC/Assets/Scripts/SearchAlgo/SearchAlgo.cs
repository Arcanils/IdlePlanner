using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
	public static class SearchAlgo
	{
		public static List<Point> DijkstraPath(Dictionary<Point, TileMapData> map, Point start, TypeTile[] typesWanted, int maxLength)
		{
			// openSet.Add(start);
			var closedSet = new HashSet<Point>();
			var openSet = new List<Point>
			{
				start
			}; // Sorted by heurestic
			var cameFrom = new Dictionary<Point, Point>();
			var gScore = new Dictionary<Point, int>
			{
				{ start, 0 }
			};


			var neighbors = new List<Point>(4);

			while (openSet.Count != 0)
			{
				var indexCurrent = GetIndexMinValue(gScore, openSet);
				var current = openSet[indexCurrent];

				if (gScore[current] > maxLength)
					return null;

				if (IsTargetNode(map[current], typesWanted))
					return RecontructPath(cameFrom, current);


				openSet.RemoveAt(indexCurrent);
				closedSet.Add(current);

				neighbors.Clear();
				GetSidesNodes(map, current, ref neighbors);

				for (int i = neighbors.Count - 1; i >= 0; --i)
				{
					var sideNode = neighbors[i];
					if (closedSet.Contains(sideNode))
						continue;

					if (!openSet.Contains(sideNode))
					{
						openSet.Add(sideNode);
						gScore[sideNode] = gScore[current] + GetDistance(map[current], map[sideNode]);
						cameFrom[sideNode] = current;
						continue;
					}

					var sideNodeCost = gScore[sideNode];
					var dist = GetDistance(map[current], map[sideNode]);

					if (sideNodeCost <= gScore[current] + dist)
						continue;

					cameFrom[sideNode] = current;
					gScore[sideNode] = sideNodeCost;


				}
			}

			return null;
		}


		private static bool IsTargetNode(TileMapData node, TypeTile[] types)
		{
			for (int i = types.Length - 1; i >= 0; --i)
			{
				if (node.Data.Type.Equals(types[i]))
					return true;
			}

			return false;
		}

		public static List<Point> GetPathAStar<T>(Dictionary<Point, T> map, Point start, Point goal, int maxLength)
		{
			// openSet.Add(start);
			var closedSet = new HashSet<Point>();
			var openSet = new List<Point>
			{
				start
			}; // Sorted by heurestic
			var cameFrom = new Dictionary<Point, Point>();
			var gScore = new Dictionary<Point, int>
			{
				{ start, 0 }
			};

			var fScore = new Dictionary<Point, int>
			{
				{ start, HeuristicCostEstimate(start, goal) }
			};


			var neighbors = new List<Point>(4);

			while (openSet.Count != 0)
			{
				var indexCurrent = GetIndexMinValue(fScore, openSet);
				var current = openSet[indexCurrent];

				if (current == goal)
					return RecontructPath(cameFrom, current);


				openSet.RemoveAt(indexCurrent);
				closedSet.Add(current);

				neighbors.Clear();
				GetSidesNodes(map, current, ref neighbors);

				for (int i = neighbors.Count - 1; i >= 0; --i)
				{
					var sideNode = neighbors[i];
					if (closedSet.Contains(sideNode))
						continue;

					if (!openSet.Contains(sideNode))
					{
						openSet.Add(sideNode);
						//var costNode = HeuristicCostEstimate(nodeTest, goal);
						//AddElementInSortedList(ref openSet, nodeTest, fScore, costNode);
					}

					int tentativeGScore;
					if (gScore.TryGetValue(sideNode, out tentativeGScore))
					{
						var dist = GetDistance(current, sideNode); // Juste distance

						if (dist == -1 || tentativeGScore + dist >= gScore[current])
						{
							continue;
						}
					}
					else
						continue;

					cameFrom[sideNode] = current;
					gScore[sideNode] = tentativeGScore;
					fScore[sideNode] = gScore[sideNode] + HeuristicCostEstimate(sideNode, goal);
				}
			}

			return null;
		}

		private static int GetIndexMinValue(Dictionary<Point, int> dicoValue, List<Point> listPoint)
		{
			var indexCurrent = listPoint.Count - 1;
			var min = int.MaxValue;
			dicoValue.TryGetValue(listPoint[indexCurrent], out min);

			for (int i = indexCurrent - 1, valFScore = 0; i >= 0; --i)
			{
				if (dicoValue.TryGetValue(listPoint[i], out valFScore))
				{
					if (min > valFScore)
					{
						min = valFScore;
						indexCurrent = i;
					}
				}
			}

			return indexCurrent;
		}

		private static int GetDistance(Point start, Point goal)
		{
			return 1;
		}
		private static int GetDistance(TileMapData start, TileMapData goal)
		{
			return (start.Data.Type.Tile == ETile.OBSTACLE || goal.Data.Type.Tile == ETile.OBSTACLE) ? 10000 : 1;
		}

		private static void AddElementInSortedList(ref List<Point> listPoints, Point newPoint, Dictionary<Point, int> pointsValue, int newPointValue)
		{
			for (int j = 0, jLength = listPoints.Count; j < jLength; ++j)
			{
				int cost;
				var pointJ = listPoints[j];
				if (pointsValue.TryGetValue(pointJ, out cost) && cost >= newPointValue)
				{
					listPoints.Insert(j, newPoint);
					return;
				}
			}

			listPoints.Add(newPoint);
		}

		private static void GetSidesNodes<T>(Dictionary<Point, T> map, Point position, ref List<Point> listNodeToAppend)
		{
			var sideNode = position;

			sideNode.Set(position.x + 1, position.y);
			if (map.ContainsKey(sideNode))
				listNodeToAppend.Add(sideNode);
			sideNode.Set(position.x - 1, position.y);
			if (map.ContainsKey(sideNode))
				listNodeToAppend.Add(sideNode);
			sideNode.Set(position.x, position.y + 1);
			if (map.ContainsKey(sideNode))
				listNodeToAppend.Add(sideNode);
			sideNode.Set(position.x, position.y - 1);
			if (map.ContainsKey(sideNode))
				listNodeToAppend.Add(sideNode);
		}

		private static int HeuristicCostEstimate(Point start, Point goal)
		{
			return Mathf.Abs(start.x - goal.x) + Mathf.Abs(start.y - goal.y);
		}

		public static List<Point> RecontructPath(Dictionary<Point, Point> cameFrom, Point current)
		{
			var path = new List<Point>();
			path.Add(current);
			while (cameFrom.ContainsKey(current))
			{
				current = cameFrom[current];
				path.Add(current);
			}

			path.RemoveAt(path.Count - 1);
			path.Reverse();

			return path;
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

}