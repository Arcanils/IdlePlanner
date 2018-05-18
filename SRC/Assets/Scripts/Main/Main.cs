using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public GameObject PrefabTile;
	public Map.GenerateMap MapGenerator;

	// Use this for initialization
	void Start () {
		var map = MapGenerator.GenerateStatic();

		foreach (var item in map)
		{
			var position = new Vector3(item.Key.x, item.Key.y);
			var instance = GameObject.Instantiate(PrefabTile, position, Quaternion.identity);
			var sr = instance.GetComponent<SpriteRenderer>();
			sr.sprite = item.Value.Data.Text;
			sr.color = item.Value.Data.Color;
		}
	}
}
