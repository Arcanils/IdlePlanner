using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntityController
{
	private IEntityModel _actionModel;
	private readonly AIBehaviour _ai;

	public EntityController(IEntityModel actionModel)
	{
		_actionModel = actionModel;
		_ai = AIBehaviour.GenerateStaticAI();
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
		switch (condition)
		{
			case ECondition.ON_TARGET:
				return true;
			case ECondition.NEXT_TO:
				return true;
			case ECondition.HAS_PATH_TO_TARGET:
				return true;
			case ECondition.NO_PATH:
				return true;
			case ECondition.TRUE:
				return true;
			default:
				throw new System.Exception("Unexpected Case");
		}

		return false;
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

	public AIGambitLine(ECondition condtion, ETargetCondition target, EActionCondition action)
	{
		Condition = condtion;
		Target = target;
		Action = action;
	}
}

public class AIBehaviour
{
	public List<AIGambitLine> Gambits;

	public AIBehaviour(List<AIGambitLine> gambits)
	{
		Gambits = gambits;
	}

	public static AIBehaviour GenerateStaticAI()
	{
		var gambits = new List<AIGambitLine>
		{
			new AIGambitLine(ECondition.HAS_PATH_TO_TARGET, ETargetCondition.RESOURCE, EActionCondition.COLLECT),
			new AIGambitLine(ECondition.NO_PATH, ETargetCondition.NONE, EActionCondition.FIND_NEW_PATH),
			new AIGambitLine(ECondition.TRUE, ETargetCondition.NONE, EActionCondition.MOVE_FORWARD),
		};

		return new AIBehaviour(gambits);
	}
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
	HAS_PATH_TO_TARGET,
	NO_PATH,
	TRUE,
}

public enum ETargetCondition
{
	RESOURCE,
	OBSTACLE,
	GROUND,
	NONE,
}