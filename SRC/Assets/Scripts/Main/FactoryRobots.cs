using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryRobots : MonoBehaviour {

	public GameObject PrefabPawn;

	private IActionOnMap _iMapData;

	public void Init(IActionOnMap iMapData)
	{
		_iMapData = iMapData;
	}

	public void CreateEntity(Point positionCell)
	{
		var logic = CreateEntityLogic(positionCell);
		var controller = CreateController(logic);
		var pawn = CreatePawn();
	}

	public EntityView CreatePawn()
	{
		return GameObject.Instantiate(PrefabPawn).GetComponent<EntityView>();
	}

	public EntityModel CreateEntityLogic(Point positionCell)
	{
		return new EntityModel(_iMapData, positionCell);
	}

	public EntityController CreateController(EntityModel model)
	{
		return new EntityController(model);
	}

}
