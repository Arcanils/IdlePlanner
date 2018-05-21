using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityModel
{
	void Collect();
	void MoveTo(Point newPosition);
	void Scan();
	void Back();
	void Spawn(Point startPosition);
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

	public Point Position { get; private set; }
	public Point PrevPosition { get; private set; }
	public int AmountCollected { get; private set; }
	public EStateModel State { get; private set; }
	public int DurationState { get; private set; }

	private Dictionary<Point, TileMapData> _visionMap;
	private readonly EntityView _pawn;
	private readonly IActionOnMap _iMapData;

	public EntityModel(IActionOnMap iMapData, Point startPosition)
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

	void IEntityModel.MoveTo(Point newPosition)
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

	public void Spawn(Point startPosition)
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

	private static Point[] GetPositionToUpdate(int rangeScan, Point position)
	{
		var listPosToUpdate = new List<Point>();
		for (int i = -rangeScan; i < rangeScan; i++)
		{
			var jLength = (rangeScan - Mathf.Abs(i)) + 1;
			for (int j = jLength / 2; j < rangeScan; j++)
			{
				listPosToUpdate.Add(new Point(i + position.x, j + position.y));
			}
		}
		return listPosToUpdate.ToArray();
	}
}
