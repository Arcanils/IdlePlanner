using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntityController
{
	private IEntityModel _actionModel;
	private IVisionModel _visionModel;
	private readonly AIBehaviour _ai;

	private List<Point> _currentPath;

	public EntityController(IEntityModel actionModel, IVisionModel visionModel)
	{
		_actionModel = actionModel;
		_visionModel = visionModel;
		_ai = AIBehaviour.GenerateStaticAI();
	}

	public void Tick()
	{
		for (int i = 0; i < _ai.Gambits.Count; i++)
		{
			var gambit = _ai.Gambits[i];
			if (EvaluateCondtion(gambit.Condition, gambit.Targets, out _currentPath))
			{
				ExecuteAction(_actionModel, gambit.Action);
				break;
			}
		}

		ExecuteAction(_actionModel, EActionCondition.BACK_TO_BASE);
	}

	private bool EvaluateCondtion(ECondition condition, TypeTile[] targets, out List<Point> pathToReturn)
	{
		pathToReturn = null;
		switch (condition)
		{
			case ECondition.ON_TARGET:
				return IsCorrectTile(targets, _visionModel.GetCurrentTile().Data.Type);
			case ECondition.NEXT_TO:
				return GetPathToTargets(targets, out pathToReturn, 1);
			case ECondition.HAS_PATH_TO_TARGET:
				return GetPathToTargets(targets, out pathToReturn, 10);
			case ECondition.NO_PATH:
				return false;
			case ECondition.TRUE:
				return true;
			default:
				throw new System.Exception("Unexpected Case");
		}
	}

	private bool GetPathToTargets(TypeTile[] targets, out List<Point> pathToReturn, int range)
	{
		pathToReturn = _visionModel.GetPathTo(targets, range);
		return pathToReturn != null;
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

	private static bool IsCorrectTile(TypeTile[] targets, TypeTile tileToCompare)
	{
		if (targets == null)
			return false;

		for (int i = targets.Length - 1; i >= 0; --i)
		{
			if (tileToCompare.Equals(targets[i]))
				return true;
		}

		return false;
	}
}

public struct AIGambitLine
{
	public ECondition Condition;
	public TypeTile[] Targets;
	public EActionCondition Action;

	public AIGambitLine(ECondition condtion, TypeTile[] targets, EActionCondition action)
	{
		Condition = condtion;
		Targets = targets;
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
			new AIGambitLine(ECondition.HAS_PATH_TO_TARGET, new TypeTile[]{new TypeTile(ETile.RESOURCE, 1) }, EActionCondition.COLLECT),
			new AIGambitLine(ECondition.NO_PATH, null, EActionCondition.FIND_NEW_PATH),
			new AIGambitLine(ECondition.TRUE, null, EActionCondition.MOVE_FORWARD),
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