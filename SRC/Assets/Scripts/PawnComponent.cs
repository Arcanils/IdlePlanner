using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityView : MonoBehaviour {

	private EntityModel _data;

	public void Init(EntityModel data)
	{
		_data = data;
	}

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

	private ETile[,] _visionMap;
	private IMapData _iMapData;
	private EntityView _pawn;

	public EntityModel(IMapData iMapData, Vector2Int startPosition)
	{
		_iMapData = iMapData;
		_visionMap = iMapData.GetMap();
		Position = startPosition;
		PrevPosition = startPosition;
		State = EStateModel.SPAWN;
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

		State = EStateModel.COLLECT;
	}

	void IEntityModel.MoveTo(Vector2Int newPosition)
	{
		if (CurrentAmountFuel <= 0)
			return;
		PrevPosition = Position;
		Position = newPosition;
		--CurrentAmountFuel;
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeVision, Position));

		State = EStateModel.MOVE;
	}
	void IEntityModel.Scan()
	{
		_iMapData.UpdateMap(ref _visionMap, GetPositionToUpdate(RangeScanVision, Position));

		State = EStateModel.SCAN;
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
	ETile[,] GetMap();
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
	MOVE_FORWARD,
	BACK,
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

public class EntityController
{
	private IEntityModel _actionModel;
	private AIBehaviour _ai;

	public EntityController(IEntityModel actionModel)
	{
		_actionModel = actionModel;
	}

	public void Tick()
	{
		for (int i = 0; i < _ai.Gambits.Count; i++)
		{
			var gambit = _ai.Gambits[i];
			if (EvaluateCondtion(_actionModel, gambit.Condition, gambit.Target))
			{
				ExecuteAction(_actionModel, gambit.Action);
				break;
			}
		}

		ExecuteAction(_actionModel, EActionCondition.BACK);
	}

	private static bool EvaluateCondtion(IEntityModel model, ECondition condition, ETargetCondition target)
	{
		return true;
	}

	private static void ExecuteAction(IEntityModel model, EActionCondition action)
	{
		switch (action)
		{
			case EActionCondition.COLLECT:
				model.Collect();
				break;
			case EActionCondition.SCAN:
				model.Scan();
				break;
			case EActionCondition.FIND_NEW_PATH:
				//A* new path
				break;
			case EActionCondition.MOVE_FORWARD:
				// Move forward
				break;
			case EActionCondition.BACK:
				// Finish
				break;
			default:
				break;
		}
	}
}

public struct AIGambitLine
{
	public ECondition Condition;
	public ETargetCondition Target;
	public EActionCondition Action;
}

public class AIBehaviour
{
	public List<AIGambitLine> Gambits;
}

public interface IEntityModel
{
	void Collect();
	void MoveTo(Vector2Int newPosition);
	void Scan();
}