using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactoryRobot
{
	void CreateModelControllerRobot(Point startPoint, out EntityModel model, out EntityController controller);
	void CreateViewRobot(EntityModel modelToBind, out EntityView view);
}

[System.Serializable]
public class FactoryRobots : IFactoryRobot
{
	public GameObject PrefabPawn;

	private IActionOnMap _iMapData;

	public void Init(IActionOnMap iMapData)
	{
		_iMapData = iMapData;
	}

	private EntityModel CreateEntityLogic(Point positionCell)
	{
		return new EntityModel(_iMapData, positionCell);
	}

	private EntityController CreateController(EntityModel model)
	{
		return new EntityController(model);
	}

	void IFactoryRobot.CreateModelControllerRobot(Point startPoint, out EntityModel model, out EntityController controller)
	{
		model = CreateEntityLogic(startPoint);
		controller = CreateController(model);
	}

	void IFactoryRobot.CreateViewRobot(EntityModel modelToBind, out EntityView view)
	{
		view = Object.Instantiate(PrefabPawn).GetComponent<EntityView>();
		view.Init(modelToBind);
	}
}
