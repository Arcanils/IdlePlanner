using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrawMap
{
	void UpdateMap(Point pos, TileMapData tileToUpdate);
}

public class Main : MonoBehaviour, IDrawMap
{

	public GameObject PrefabTile;
	public Map.GenerateMap MapGenerator;

	[SerializeField]
	private FactoryRobots _factory;
	private GlobalMap _map;
	private IEntityManagerAction _entityManagerAction;
	private IEntityTick _entityManagerTick;
	private Dictionary<Point, GameObject> _uiMap;

	// Use this for initialization
	void Start ()
	{
		var startPoint = new Point(2, 2);

		var entityManager = new EntityManager(_factory, startPoint);
		_entityManagerAction = entityManager;
		_entityManagerTick = entityManager;
		TileFlyweight defaultTile;
		var mapData = MapGenerator.GenerateStatic(out defaultTile);
		_map = new GlobalMap(this, mapData, defaultTile);
		_factory.Init(_map);
		_uiMap = new Dictionary<Point, GameObject>();
		foreach (var item in mapData)
		{
			_uiMap[item.Key] = CreateTile(item.Key, item.Value.Data);
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

	public void UpdateMap(Point pos, TileMapData tileToUpdate)
	{
		Destroy(_uiMap[pos]);
		_uiMap[pos] = CreateTile(pos, tileToUpdate.Data);
	}

	private GameObject CreateTile(Point position, TileFlyweight tile)
	{
		var position3d = new Vector3(position.x, position.y);
		var instance = GameObject.Instantiate(PrefabTile, position3d, Quaternion.identity);
		var sr = instance.GetComponent<SpriteRenderer>();
		sr.sprite = tile.Text;
		sr.color = tile.Color;

		return instance;
	}
}

// Refresh map after collect
// Draw robot
