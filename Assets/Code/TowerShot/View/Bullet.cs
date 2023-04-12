using UnityEngine;

namespace Code.TowerShot
{
	[RequireComponent(typeof(Rigidbody))]
	public class Bullet : MonoBehaviour
	{

		[SerializeField] private TowerShotConfig _towerShotConfig;
		private Transform _target;
		private bool _isAutoAuidance = false;

		public void SetBullet(Vector3 direction, Transform target)
		{
			Rigidbody body = GetComponent<Rigidbody>();
			body.useGravity = false;
			body.velocity = direction * _towerShotConfig.BulletSpeed;
			transform.forward = direction;
			_target = target;
			_isAutoAuidance = true;
		}

		void OnCollisionEnter(Collision other)
		{
			if (!other.collider.isTrigger)
			{
				if (other.collider.CompareTag("Enemy"))
				{
					other.collider.GetComponent<Damageable>().MakeDamage(_towerShotConfig.Damage);
				}
				_isAutoAuidance = false;
				gameObject.SetActive(false);
			}
		}

		void Update()
		{
			if (_isAutoAuidance) SearchEnemy();
		}
		void SearchEnemy()
		{
			Vector3 lookPos = _target.position - transform.position;
			Vector3 rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookPos), 4 * Time.deltaTime).eulerAngles;

			transform.rotation = Quaternion.LookRotation(lookPos);
			transform.eulerAngles = rotation;
		}
	}

	

}


