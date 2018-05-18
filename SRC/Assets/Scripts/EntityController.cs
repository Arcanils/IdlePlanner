using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

		ExecuteAction(_actionModel, EActionCondition.BACK_TO_BASE);
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
			case EActionCondition.BACK_TO_BASE:
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

public enum EActionCondition
{
	COLLECT,
	SCAN,
	FIND_NEW_PATH,
	MOVE_FORWARD,
	BACK_TO_BASE,
}

public enum ECondition
{
	ON_TARGET,
	NEXT_TO,
	HAS_VISION_ON_TARGET,
}

public enum ETargetCondition
{
	RESOURCE,
	OBSTACLE,
	GROUND,
}