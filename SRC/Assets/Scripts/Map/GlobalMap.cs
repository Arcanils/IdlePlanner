using Map;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{

	public interface IActionOnMap
	{
		Dictionary<Point, TileMapData> GetMap();
		void UpdateMap(ref Dictionary<Point, TileMapData> visionMap, params Point[] tilesToUpdate);
		bool Collect(Point position);
	}

	public class GlobalMap : IActionOnMap
	{
		private Dictionary<Point, TileMapData> _map;
		private Dictionary<Point, TileMapData> _vision;
		public GlobalMap(Dictionary<Point, TileMapData> map)
		{
			_map = map;
			_vision = new Dictionary<Point, TileMapData>();
		}

		bool IActionOnMap.Collect(Point position)
		{
			if (!_map.ContainsKey(position))
				return false;

			var item = _map[position];

			if (item.Data.Type.Tile != ETile.RESOURCE || item.AmountResources <= 0)
				return false;

			--item.AmountResources;
			return true;
		}

		Dictionary<Point, TileMapData> IActionOnMap.GetMap()
		{
			return _vision;
		}

		void IActionOnMap.UpdateMap(ref Dictionary<Point, TileMapData> visionMap, params Point[] tilesToUpdate)
		{
			for (int i = tilesToUpdate.Length - 1; i >= 0; --i)
			{
				var position = tilesToUpdate[i];
				if (!_map.ContainsKey(position))
					continue;

				visionMap[position] = _map[position];
			}
		}
	}
}
