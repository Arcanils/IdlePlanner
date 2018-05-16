using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityModel
{
	void Collect();
	void MoveTo(Vector2Int newPosition);
	void Scan();
	void Back();
	void Spawn(Vector2Int startPosition);
}

public class EntityModel : IEntityModel
{
	public enum EStateModel
	{
		SPAWN,
		MOVE,
		SCAN,
		COLLECT,
		BACK,
		OTHERS,
	}

	public int RangeVision = 1;
	public int RangeScanVision = 3;
	public int CapacityStorage = 5;
	public int CurrentAmountFuel = 50;

	public Vector2Int Position { get; private set; }
	public Vector2Int PrevPosition { get; private set; }
	public int AmountCollected { get; private set; }
	public EStateModel State { get; private set; }
	public int DurationState { get; private set; }

	private ETile[,] _visionMap;
	private readonly EntityView _pawn;
	private readonly IMapData _iMapData;

	public EntityModel(IMapData iMapData, Vector2Int startPosition)
	{
		_iMapData = iMapData;
		_visionMap = iMapData.GetMap();
		Spawn(startPosition);
	}


	void IEntityModel.Collect()
	{
		if (AmountCollected >= CapacityStorage)
			return;

		if (_iMapData.Collect(Position))
		{
			++AmountCollected;
			_iMapData.UpdateMap(ref _visionMap, Position);
		}

		SwitchState(EStateModel.COLLECT);
	}

	void IEntityModel.MoveTo(Vector2Int newPosition)
	{
		if (CurrentAmountFuel <= 0)
			return;
		PrevPosition = Position;
		Position = newPosition;
		--CurrentAmountFuel;
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeVision, Position));

		SwitchState(EStateModel.MOVE);
	}
	void IEntityModel.Scan()
	{
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeScanVision, Position));

		SwitchState(EStateModel.SCAN);
	}

	void IEntityModel.Back()
	{
		SwitchState(EStateModel.BACK);
	}

	public void Spawn(Vector2Int startPosition)
	{
		Position = startPosition;
		PrevPosition = startPosition;
		SwitchState(EStateModel.SPAWN);
	}

	private void SwitchState(EStateModel newState)
	{
		if (State == newState)
		{
			++DurationState;
			return;
		}

		DurationState = 0;
		State = newState;
	}

	private static Vector2Int[] GetPositionToUpdate(int rangeScan, Vector2Int position)
	{
		var listPosToUpdate = new List<Vector2Int>();
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
