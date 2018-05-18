using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
	[CreateAssetMenu(fileName = "MapGenerator", menuName = "Map/MapGenerator")]
	public class GenerateMap : ScriptableObject
	{
		public TileFlyweight[] ListTiles;
		public DataGenerateMap[] Data;

		public Dictionary<Point, TileMapData> GenerateStatic()
		{
			var map = new Dictionary<Point, TileMapData>();
			var data = Data[0];
			var fwBorder = GetTileFlyweight(data.Border);
			for (int x = 0, xLength = data.SizeMapX; x < xLength; x++)
			{
				map[new Point(x, 0)] = new TileMapData(fwBorder);
				map[new Point(x, data.SizeMapY - 1)] = new TileMapData(fwBorder);
			}

			for (int x = 1, xLength = data.SizeMapY - 1; x < xLength; x++)
			{
				map[new Point(0, x)] = new TileMapData(fwBorder);
				map[new Point(data.SizeMapX - 1, x)] = new TileMapData(fwBorder);
			}

			var fwEmpty = GetTileFlyweight(data.Ground);

			for (int x = 1, xLength = data.SizeMapX - 1; x < xLength; x++)
			{
				for (int y = 1, yLength = data.SizeMapY - 1; y < yLength; y++)
				{
					map[new Point(x, y)] = new TileMapData(fwEmpty);
				}
			}


			for (int i = 0; i < data.SpawnTile.Length; i++)
			{
				var tile = data.SpawnTile[i];
				var fwTile = GetTileFlyweight(tile.Type);
				var xOrigin = UnityEngine.Random.Range(0f, 100f);
				var yOrigin = UnityEngine.Random.Range(0f, 100f);
				var lengthLuck = (1 - tile.SpawnValue);
				for (int x = data.SizeMapX - 2; x >= 1; x--)
				{
					for (int y = data.SizeMapY - 2; y >= 1; y--)
					{
						var position = new Point(x, y);

						if (map[position].Data != fwEmpty)
							continue;

						var xCoord = xOrigin + (x * tile.ScaleNoise ) / data.SizeMapX;
						var yCoord = yOrigin + (y * tile.ScaleNoise) / data.SizeMapY;

						var noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

						if (noiseValue > tile.SpawnValue)
						{
							map[position] = new TileMapData(fwTile, Mathf.RoundToInt(5 * ((noiseValue - tile.SpawnValue) / lengthLuck)));
						}
					}
				}
			}



			return map;
		}

		private TileFlyweight GetTileFlyweight(TypeTile type)
		{
			return ListTiles.FirstOrDefault(tile => tile.Type.Equals(type));
		}
	}
}
