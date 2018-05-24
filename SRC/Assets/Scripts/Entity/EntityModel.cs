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
	void MoveForward();
}

public interface IVisionModel
{
	List<Point> GetPathTo(TypeTile[] targets, int range);
	bool IsBlocked();
	TileMapData GetCurrentTile();
}

public class EntityModel : IEntityModel, IVisionModel
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
	public int CapacityStorage = 500;
	public int CurrentAmountFuel = 50;

	public Point Direction { get; private set; }
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
			//_iMapData.UpdateMap(ref _visionMap, Position);
		}

		Debug.Log("COLLECT");
		SwitchState(EStateModel.COLLECT);
	}

	public void MoveTo(Point newPosition)
	{
		if (CurrentAmountFuel <= 0)
			return;
		PrevPosition = Position;
		Position = newPosition;
		Direction = Position - PrevPosition;
		--CurrentAmountFuel;
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeVision, Position));

		Debug.Log("MOVE to" + Position);
		SwitchState(EStateModel.MOVE);
	}

	void IEntityModel.MoveForward()
	{
		MoveTo(Position + Direction);
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
		Direction = new Point(0, 1);
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeVision, Position));
		SwitchState(EStateModel.SPAWN);
	}


	List<Point> IVisionModel.GetPathTo(TypeTile[] targets, int range)
	{
		return SearchAlgo.DijkstraPath(_visionMap, Position, targets, range);
	}

	bool IVisionModel.IsBlocked()
	{
		var newPos = Direction + Position;

		return !_visionMap.ContainsKey(newPos) || _visionMap[newPos].Data.Type.Tile == ETile.OBSTACLE;
	}

	TileMapData IVisionModel.GetCurrentTile()
	{
		return _visionMap[Position];
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
