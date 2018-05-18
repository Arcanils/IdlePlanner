using Map;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{

	public interface IActionOnMap
	{
		HashSet<Point> GetMap();
		void UpdateMap(ref HashSet<Point> visionMap, params Point[] tilesToUpdate);
		bool Collect(Point position);
	}

	public class GlobalMap : IActionOnMap
	{
		private Dictionary<Point, TileMapData> _map;

		bool IActionOnMap.Collect(Point position)
		{
			throw new System.NotImplementedException();
		}

		HashSet<Point> IActionOnMap.GetMap()
		{
			throw new System.NotImplementedException();
		}

		void IActionOnMap.UpdateMap(ref HashSet<Point> visionMap, params Point[] tilesToUpdate)
		{
			throw new System.NotImplementedException();
		}
	}
}
