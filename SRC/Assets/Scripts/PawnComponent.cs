using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnComponent : MonoBehaviour {
	
	public void Move()
	{

	}

	public void Scan()
	{

	}

	public void Collect()
	{

	}

	public void ThrowResources()
	{

	}
}

public class EntityData
{
	public int RangeVision = 1;
	public int RangeScanVision = 3;
	public int CapacityStorage = 5;
	public int CurrentAmountFuel = 50;

	public Vector2Int Position { get; private set; }
	public Vector2Int PrevPosition { get; private set; }
	public int AmountCollected { get; private set; }

	private ETile[,] _visionMap;
	private IMapData _iMapData;

	public EntityData(IMapData iMapData, ETile[,] visionMap, Vector2Int startPosition)
	{

	}

	public void Collect()
	{
		if (AmountCollected >= CapacityStorage)
			return;

		if (_iMapData.Collect(Position))
		{
			++AmountCollected;
			_iMapData.UpdateMap(ref _visionMap, Position);
		}

	}

	public void MoveTo(Vector2Int newPosition)
	{
		if (CurrentAmountFuel <= 0)
			return;
		PrevPosition = Position;
		Position = newPosition;
		--CurrentAmountFuel;
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeVision, Position));
	}
	public void Scan()
	{
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeScanVision, Position));
	}

	private static Vector2Int[] GetPositionToUpdate(int rangeScan, Vector2Int position)
	{
		List<Vector2Int> listPosToUpdate = new List<Vector2Int>();
		for (int i = -rangeScan; i < rangeScan; i++)
		{
			var jLength = (rangeScan - Mathf.Abs(i)) + 1;
			for (int j = jLength / 2; j < rangeScan; j++)
			{
				listPosToUpdate.Add(new Vector2Int(i + position.x, j + position.y));
			}
		}
		return listPosToUpdate.ToArray();
	}
}

public interface IMapData
{
	void UpdateMap(ref ETile[,] visionMap, params Vector2Int[] tilesToUpdate);
	bool Collect(Vector2Int position);
}


public enum ETile
{
	UNKNOWN,
	DUST,
	GOLD,
	ROCK,
}

public enum EActionCondition
{
	COLLECT,
	SCAN,
	FIND_NEW_PATH,
}

public enum ECondition
{
	ON_TARGET,
	IN_FRONT_OF_TARGET,
	HAS_VISION_ON_TARGET,
}

public enum ETargetCondition
{
	RESOURCES,
	ROCK,
	DUST,
}
public struct TileMap
{
	public ETile Type;
	public int Amount;
}
