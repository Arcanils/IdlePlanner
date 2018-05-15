using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryRobots : MonoBehaviour {

	public GameObject PrefabPawn;

	private IMapData _iMapData;

	public void Init(IMapData iMapData)
	{
		_iMapData = iMapData;
	}

	public void CreateEntity(Vector2Int positionCell)
	{
		var logic = CreateEntityLogic(positionCell);
		var controller = CreateController(logic);
		var pawn = CreatePawn();
	}

	public EntityView CreatePawn()
	{
		return GameObject.Instantiate(PrefabPawn).GetComponent<EntityView>();
	}

	public EntityModel CreateEntityLogic(Vector2Int positionCell)
	{
		return new EntityModel(_iMapData, positionCell);
	}

	public EntityController CreateController(EntityModel model)
	{
		return new EntityController(model);
	}

}
