using Map;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{

	public class GlobalMap : IMapData
	{
		private Dictionary<Point, TileMapData> _map;




		bool IMapData.Collect(Vector2Int position)
		{
			throw new System.NotImplementedException();
		}

		ETile[,] IMapData.GetMap()
		{
			throw new System.NotImplementedException();
		}

		void IMapData.UpdateMap(ref ETile[,] visionMap, params Vector2Int[] tilesToUpdate)
		{
			throw new System.NotImplementedException();
		}
	}

	public class TileFlyweight
	{
		public TypeTile Type;
		public Sprite Text;
	}

	public class TileMapData
	{
		public TileFlyweight Data;
		public int AmountResources;

		public TileMapData(TileFlyweight data, int amountResources = -1)
		{
			Data = data;
			AmountResources = amountResources;
		}
	}
}

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

					var xCoord = xOrigin + (x / data.SizeMapX) * tile.ScaleNoise;
					var yCoord = xOrigin + (x / data.SizeMapX) * tile.ScaleNoise;

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

public struct TypeTile : System.IEquatable<TypeTile>
{
	public ETile Tile;
	public int SubTile;

	public bool Equals(TypeTile other)
	{
		return this.Tile == other.Tile && this.SubTile == other.SubTile;
	}
}

public class DataGenerateMap : ScriptableObject
{
	public struct DataSpawnTile
	{
		public TypeTile Type;
		public float ScaleNoise;
		public float SpawnLuck;
		public float SpawnValue; // Avec le noisie
	}
	public int SizeMapX = 20;
	public int SizeMapY = 20;
	public TypeTile Ground;
	public TypeTile Border;
	public DataSpawnTile[] SpawnTile;
}
public interface IMapData
{
	ETile[,] GetMap();
	void UpdateMap(ref ETile[,] visionMap, params Vector2Int[] tilesToUpdate);
	bool Collect(Vector2Int position);
}


public enum ETile
{
	RESOURCE,
	OBSTACLE,
	GROUND,
}

public enum EResource
{
	GOLD,
	IRON,
	WATER,
	TREASURE,
}

public enum EObstacle
{
	ROCK,
	WALL,
	FIRE,
}

public enum EGround
{
	SAND,
	GRASS,
	MISC,
}

public struct Point
{
	public int x, y;
	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void Set(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Point))
		{
			return false;
		}

		var point = (Point)obj;
		return x == point.x &&
			   y == point.y;
	}

	public override int GetHashCode()
	{
		var hashCode = 1502939027;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + x.GetHashCode();
		hashCode = hashCode * -1521134295 + y.GetHashCode();
		return hashCode;
	}

	public static bool operator ==(Point lhs, Point rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y;
	}
	public static bool operator !=(Point lhs, Point rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y;
	}
}
