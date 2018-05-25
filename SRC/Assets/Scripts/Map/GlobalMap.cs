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
		void UpdateMap(ref Dictionary<Point, TileMapData> visionMap, Point position, int range);
		bool Collect(Point position);
	}

	public class GlobalMap : IActionOnMap
	{
		private Dictionary<Point, TileMapData> _map;
		private Dictionary<Point, TileMapData> _vision;
		private TileFlyweight _defaultTile;
		private IDrawMap _drawMap;
		public GlobalMap(IDrawMap drawMap, Dictionary<Point, TileMapData> map, TileFlyweight defaultTile)
		{
			_drawMap = drawMap;
			_map = map;
			_defaultTile = defaultTile;
			_vision = new Dictionary<Point, TileMapData>();
		}

		bool IActionOnMap.Collect(Point position)
		{
			if (!_map.ContainsKey(position))
				return false;

			var item = _map[position];

			Debug.Log(item.Data.Type.Tile + " " + item.AmountResources);

			if (item.Data.Type.Tile != ETile.RESOURCE || item.AmountResources <= 0)
				return false;

			if (--item.AmountResources <= 0)
			{
				_map[position].Data = _defaultTile;
				_drawMap.UpdateMap(position, _map[position]);
				Debug.LogWarning("END OF RESOURCES");
			}
			return true;
		}

		Dictionary<Point, TileMapData> IActionOnMap.GetMap()
		{
			return _vision;
		}

		void IActionOnMap.UpdateMap(ref Dictionary<Point, TileMapData> visionMap, Point position, int range)
		{
			var tilesToUpdate = GetPositionToUpdate(range, position);
			for (int i = tilesToUpdate.Length - 1; i >= 0; --i)
			{
				var tile = tilesToUpdate[i];
				if (!_map.ContainsKey(tile))
					continue;

				visionMap[tile] = _map[tile];
			}
			++range;
			for (int i = -range; i <= range; i++)
			{
				var jLength = ((range - Mathf.Abs(i)) + 1) / 2;
				var tile1 = new Point(i + position.x, -jLength + position.y);
				var tile2 = new Point(i + position.x, +jLength + position.y);
				if (_map.ContainsKey(tile1) && !(visionMap.ContainsKey(tile1)))
				{
					//SetTileUnknown
				}
				if (_map.ContainsKey(tile2) && !(visionMap.ContainsKey(tile2)))
				{
					//SetTileUnknown
				}
			}
		}

		private static Point[] GetPositionToUpdate(int rangeScan, Point position)
		{
			var listPosToUpdate = new List<Point>();
			for (int i = -rangeScan; i <= rangeScan; i++)
			{
				var jLength = (rangeScan - Mathf.Abs(i)) + 1;
				var jLength2 = jLength / 2;
				for (int j = -jLength2; j <= jLength2; j++)
				{
					listPosToUpdate.Add(new Point(i + position.x, j + position.y));
				}
			}
			return listPosToUpdate.ToArray();
		}
	}
}
