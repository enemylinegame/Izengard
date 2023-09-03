using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.Game;
using Controllers.Pool;
using Models.BaseUnit;
using UnityEngine;


namespace Code.TowerShot
{
	public class TowerShotController : IOnController, IOnUpdate, IOnStart, IDisposable
{
	#region Fealds
	private TowerShotConfig _config;
	private TowerShotBehavior _view;
	private BaseBulletFactory _baseBulletFactory;
	private BuildingFactory _buildingFactory;

	private List<Damageable> Enemys = new List<Damageable>();
	private GameObject _prefab;
	private Vector3 _offset;
	private Damageable _target;
	
	private int _index;
	private float _curFireRate;
	private bool IsTargert = true;

	#endregion
	#region Unity Methods
	public TowerShotController(ConfigsHolder configs, BuildingFactory buildingFactory)
	{
		_config = configs.TowerShotConfig;
		_prefab = configs.PrefabsHolder.Bullet;
		_buildingFactory = buildingFactory;
		_curFireRate = configs.TowerShotConfig.FireRate;
		
	}
	public void OnUpdate(float deltaTime)
	{
        if (_view == null)
		{
            _view = _buildingFactory.TowerShot;
            if (_view != null)
            {
                _view.Trigger += OnTriggerEnter;
                _offset = _view.TurretTrigger.center;
            }
        }
		

		for(int index = 0; index< Enemys.Count; index++)
		{
			Damageable currentUnit = Enemys[index];

            if (currentUnit.IsDead == true)
			{
				RemoveFromList(currentUnit);
				IsTargert = true;

                continue;
            }
			
			if (IsTargert == true) _target = Enemys[index];
			if(_target.transform != null) NearestObject(_target.transform);
		}

	}
	public void OnStart()
	{
		_baseBulletFactory = new BaseBulletFactory(new GameObjectPoolController(_config.CountBullet, _prefab));
	}
	public void Dispose()
	{
		_baseBulletFactory?.Dispose();
		_view.Trigger -= OnTriggerEnter;
	}

	#endregion
	#region Methods

		private void OnTriggerEnter(Collider other) //Это именно просто метод для экшона
		{
			Enemys.Add(other.GetComponent<Damageable>());
		}
		private void NearestObject(Transform target) // поиск ближайшего объекта
		{
            float dist = Mathf.Infinity;
            float currentDist = Vector3.Distance(_view.transform.position + _offset, target.transform.position);
            if (currentDist < dist && IsTargert == true)
            {
                dist = currentDist;
                IsTargert = false;
            }
            if (IsTargert == false)
            {
                Shot(target.transform);
            }
        }
		private bool Search(Transform target) //разворот башни на цель
		{
			if (_config.RayOffset < 0) _config.RayOffset = 0;
		
			float dist = Vector3.Distance(_view.transform.position + _offset, target.position);
			Vector3 lookPos = target.position - _view.TurretRotation.position;
			Debug.DrawRay(_view.TurretRotation.position,_view.Center.forward * (_view.TurretTrigger.contactOffset + _config.RayOffset));
			Vector3 rotation = Quaternion.Lerp(_view.TurretRotation.rotation, Quaternion.LookRotation(lookPos), 1.2f * Time.deltaTime).eulerAngles;
		
			_view.TurretRotation.rotation = Quaternion.LookRotation(lookPos);
			_view.TurretRotation.eulerAngles = rotation;
		
			if(dist > _view.TurretTrigger.contactOffset + _config.RayOffset)
			{
				target = null;
				return false;
			}

			if(Enemys.Count == 0)
			{
				target = null; 
				return false;
			}

			var tt = Quaternion.Dot(_view.TurretRotation.transform.rotation, Quaternion.LookRotation(lookPos));

			if (Math.Abs(tt) > 0.98f) return true;

            //if (IsRaycastHit(_view.Center)) return true;
            return false;
	}
	private bool IsRaycastHit(Transform point) //райкаст луч
	{
		RaycastHit hit;
		var bitmask = ~(1 << 0);
		Ray ray = new Ray(point.position, point.forward);
		if (Physics.Raycast(ray, out hit, _view.TurretTrigger.contactOffset + _config.RayOffset, bitmask))
		{
			if (hit.collider.CompareTag("Enemy"))
			{
				return true;
			}
		}

		return false;
	}
	private void Shot(Transform target) // выстрел
	{
		if (!Search(target)) return;

		_curFireRate += Time.deltaTime;
		if (_curFireRate > _config.FireRate)
		{
			Transform point = GetPoint();
			_curFireRate = 0;

			var go = _baseBulletFactory.CreateBullet(point.position);
			go.GetComponent<Bullet>().SetBullet(point.forward, target);
		}
	}

	private void RemoveFromList(Damageable target)
	{
		Enemys.Remove(target);
	}
	//private void Choice()
	//{
	//	_curFireRate = _config.FireRate;

	//	if (_target != null)
	//	{
	//		_view.TurretRotation.rotation = Quaternion.LookRotation(_target.position);
	//	}

	//	if (Quaternion.Angle(_view.TurretRotation.rotation, _defaultRot) == 0)
	//	{
	//		_view.TurretRotation.rotation = _defaultRot;
	//	}
	//}
	private Transform GetPoint() //точки выстрела
	{
		if (_index == _view.BulletPoint.Length - 1) _index = 0;
		else _index++;
		return _view.BulletPoint[_index];
	}

	#endregion
}
}

