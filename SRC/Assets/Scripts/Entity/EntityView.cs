using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityView : MonoBehaviour {

	private EntityModel _data;

	private Transform _trans;

	public void Init(EntityModel data)
	{
		_trans = transform;
		_data = data;
	}

	public void Tick()
	{
		Move();
	}

	public void Move()
	{
		var pos = _data.Position;
		_trans.position = new Vector3(pos.x, pos.y);
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

