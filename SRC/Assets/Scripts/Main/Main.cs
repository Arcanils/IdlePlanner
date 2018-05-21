using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public GameObject PrefabTile;
	public Map.GenerateMap MapGenerator;


	[SerializeField]
	private FactoryRobots _factory;
	private IEntityManagerAction _entityManagerAction;
	private IEntityTick _entityManagerTick;

	// Use this for initialization
	void Start ()
	{
		var startPoint = new Point(2, 2);

		var entityManager = new EntityManager(_factory, startPoint);
		_entityManagerAction = entityManager;
		_entityManagerTick = entityManager;
		var map = MapGenerator.GenerateStatic();

		foreach (var item in map)
		{
			var position = new Vector3(item.Key.x, item.Key.y);
			var instance = GameObject.Instantiate(PrefabTile, position, Quaternion.identity);
			var sr = instance.GetComponent<SpriteRenderer>();
			sr.sprite = item.Value.Data.Text;
			sr.color = item.Value.Data.Color;
		}

		StartCoroutine(TestLogicEnum());
	}

	public IEnumerator TestLogicEnum()
	{
		_entityManagerAction.AddEntity();
		while (true)
		{
			_entityManagerTick.Tick();
			yield return new WaitForSeconds(0.5f);
		}
	}

}
