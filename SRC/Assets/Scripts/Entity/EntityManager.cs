using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEntityTick
{
	void Tick();
	void TickMultiple(int nTick);
}

public interface IEntityManagerAction
{
	void AddEntity();
	void RemoveEntity(int index);
	void RemoveAllEntity();
	void SetActiveEntitiesView(bool enable);
}

public class EntityManager : IEntityManagerAction, IEntityTick
{
	private class EntityMVC
	{
		public readonly EntityModel Model;
		public readonly EntityController Controller;
		public EntityView View { get; private set; }

		public EntityMVC(EntityModel model, EntityController controller)
		{
			Model = model;
			Controller = controller;
		}


		public void SetupView(EntityView view)
		{
			View = view;
		}

		public void RemoveView()
		{
			View = null;
		}
	}
	private IFactoryRobot _factory;
	private Point _startPoint;
	private List<EntityMVC> _listEntity;
	private bool _viewEnable;

	public EntityManager(IFactoryRobot factory, Point startPoint)
	{
		_factory = factory;
		_startPoint = startPoint;
		_listEntity = new List<EntityMVC>(32);
	}

	void IEntityManagerAction.AddEntity()
	{
		EntityModel model;
		EntityController controller;
		_factory.CreateModelControllerRobot(_startPoint, out model, out controller);
		var entity = new EntityMVC(model, controller);
		_listEntity.Add(entity);

		if (_viewEnable)
		{
			SetActiveView(entity, true);
		}

	}

	void IEntityManagerAction.RemoveEntity(int index)
	{
		var entity = _listEntity[index];
		if (entity.View != null)
			Object.Destroy(entity.View);

		_listEntity.RemoveAt(index);
	}

	void IEntityManagerAction.RemoveAllEntity()
	{
		var script = ((IEntityManagerAction)this);
		for (int i = _listEntity.Count - 1; i >= 0; --i)
		{
			script.RemoveEntity(i);
		}
	}

	void IEntityManagerAction.SetActiveEntitiesView(bool enable)
	{
		if (enable == _viewEnable)
			return;

		_viewEnable = enable;

		for (int i = _listEntity.Count - 1; i >= 0; --i)
		{
			SetActiveView(_listEntity[i], enable);
		}
	}


	public void Tick()
	{
		for (int i = 0, iLength = _listEntity.Count; i < iLength; i++)
		{
			_listEntity[i].Controller.Tick();
		}

		if (!_viewEnable)
			return;

		for (int i = 0, iLength = _listEntity.Count; i < iLength; i++)
		{
			_listEntity[i].View.Tick();
		}
	}

	public void TickMultiple(int nTick)
	{
		for (int j = 0; j < nTick; j++)
		{
			for (int i = 0, iLength = _listEntity.Count; i < iLength; i++)
			{
				_listEntity[i].Controller.Tick();
			}
		}

		if (!_viewEnable)
			return;

		for (int i = 0, iLength = _listEntity.Count; i < iLength; i++)
		{
			_listEntity[i].View.Tick();
		}
	}

	private void SetActiveView(EntityMVC entity, bool enable)
	{
		if ((entity.View != null && enable) || (entity.View == null && !enable))
			return;

		if (!enable)
		{
			entity.RemoveView();
			return;
		}

		EntityView view;
		_factory.CreateViewRobot(entity.Model, out view);
		entity.SetupView(view);
	}
}
