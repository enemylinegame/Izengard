using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	private GeneratorLevelController _generatorLevelController;

	private List<GameObject> Enemys = new List<GameObject>();
	private GameObject _prefab;
	private Transform _target;
	private Vector3 _offset;
	private int _index;
	private float _curFireRate;
	private Quaternion _defaultRot = Quaternion.identity;

	#endregion
	#region Unity Methods
	public TowerShotController(TowerShotConfig config, GeneratorLevelController lvlController, GameObject Prefab)
	{
		_config = config;
		_prefab = Prefab;
		_generatorLevelController = lvlController;
		_curFireRate = config.FireRate;
		
	}
	public void OnUpdate(float deltaTime)
	{
		if (_view != null)
		{
			NearestObject();
			if (_target != null) Shot();
		}
		else
		{
			_view = _generatorLevelController.TowerShot;
			if (_view != null)
			{
				_view.Trigger += OnTriggerEnter;
				_offset = _view.TurretTrigger.center;
			}
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
		Enemys.Add(other.gameObject);
		other.GetComponent<Damageable>().DeathAction += RemoveList;
		Debug.Log("врагов осталось: " + Enemys.Count);
	}
	private void NearestObject() // поиск ближайшего объекта
	{
		foreach (var enemy in Enemys)
		{
			float dist = Mathf.Infinity;
			float currentDist = Vector3.Distance(_view.transform.position + _offset, enemy.transform.position);

			if (currentDist < dist)
			{
				dist = currentDist;
				_target = enemy.transform;

			}
		}
	}
	private bool Search() //разворот башни на цель
	{
		if (_config.RayOffset < 0) _config.RayOffset = 0;
		
		float dist = Vector3.Distance(_view.transform.position + _offset, _target.position);
		Vector3 lookPos = _target.position - _view.TurretRotation.position;
		Debug.DrawRay(_view.TurretRotation.position,_view.Center.forward * (_view.TurretTrigger.contactOffset + _config.RayOffset));
		Vector3 rotation = Quaternion.Lerp(_view.TurretRotation.rotation, Quaternion.LookRotation(lookPos), 0.5f * Time.deltaTime).eulerAngles;
		
		_view.TurretRotation.rotation = Quaternion.LookRotation(lookPos);
		_view.TurretRotation.eulerAngles = rotation;
		
		if(dist > _view.TurretTrigger.contactOffset + _config.RayOffset)
		{
			_target = null;
			return false;
		}

		if (IsRaycastHit(_view.Center)) return true;
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
	private void Shot() // выстрел
	{
		if (!Search()) return;

		_curFireRate += Time.deltaTime;
		if (_curFireRate > _config.FireRate)
		{
			Transform point = GetPoint();
			_curFireRate = 0;

			var go = _baseBulletFactory.CreateBullet(point.position);
			go.GetComponent<Bullet>().SetBullet(point.forward, _target);
		}
	}

	private void RemoveList()
	{
		Enemys.Remove(_target.gameObject);
		Debug.Log("врагов осталось: " + Enemys.Count);
		_target.gameObject.GetComponent<Damageable>().DeathAction -= RemoveList;
	}
	private void Choice()
	{
		_curFireRate = _config.FireRate;

		if (_target != null)
		{
			_view.TurretRotation.rotation = Quaternion.LookRotation(_target.position);
		}

		if (Quaternion.Angle(_view.TurretRotation.rotation, _defaultRot) == 0)
		{
			_view.TurretRotation.rotation = _defaultRot;
		}
	}
	private Transform GetPoint() //точки выстрела
	{
		if (_index == _view.BulletPoint.Length - 1) _index = 0;
		else _index++;
		return _view.BulletPoint[_index];
	}

	#endregion
}
}

