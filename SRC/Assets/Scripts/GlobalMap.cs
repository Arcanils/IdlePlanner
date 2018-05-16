using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{

	public class GlobalMap : IMapData
	{
		private TileMapData[,] _map;




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
		public readonly ETile Type;
		public readonly int SubType; // Resources / Obstacle / Ground
		public Sprite Text;
	}

	public class TileMapData
	{
		public TileFlyweight Data;
		public int AmountResources;
	}
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
