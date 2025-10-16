using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	[SerializeField] private int _spawnID = 1;
	[SerializeField] private float _maxY = -1.55f;
	[SerializeField] private float _minY = -4.5f;
	[SerializeField] private int _numberOfEnemies = 2;
	[SerializeField] private float _spawnTime = 1.0f;
	[SerializeField] private GameObject _prefabEnemy;

	private int _count;
	private int _currentEnemies;
	private Camera _camera;
	private Player _player;
	private List<GameObject> _enemies = new List<GameObject>();

	public static Action onStartSpawn;
	public static Action onAllKill;
	public static Action<int> onEnemyCount;

	private void Awake()
    {
		_camera = Camera.main;
		_currentEnemies = 0;
		_count = 0;
	}

	private void OnEnable()
	{
		ItemDamage.onDeathEnemy += UpdateCountEnemy; 
	}

	private void OnDisable()
	{
		ItemDamage.onDeathEnemy -= UpdateCountEnemy;
	}

	public void StartSpawn()
	{
		onStartSpawn?.Invoke();
	}

	public void AllKill()
	{
		onAllKill?.Invoke();
	}

	private void UpdateCountEnemy(ItemDamage enemy)
    {
		if (enemy.spawnID != _spawnID) return;

		GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		int countAliveEnemies = -1;

		foreach (GameObject item in aliveEnemies)
        {
			if (item.GetComponent<ItemDamage>().spawnID == _spawnID)
			{
				countAliveEnemies++;
			}
        }

		if (countAliveEnemies > 0)  
		{
			enemy.gameObject.tag = "Untagged";
			_count--;
			onEnemyCount?.Invoke(_count); 
		}
		
		if (countAliveEnemies == 0 && _currentEnemies >= _numberOfEnemies)
		{
			AllKill();
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			StartSpawn();
			_player = player;
			GetComponent<BoxCollider2D>().enabled = false;
			_camera.GetComponent<CameraFollow>().maxXAndY.x = transform.position.x + 8.9f;
			_currentEnemies = 0;
			_count = _numberOfEnemies;
			onEnemyCount?.Invoke(_count);
			CountEnemiesOnScreen();
			SpawnEnemy();
		}
	}

	private void CountEnemiesOnScreen()
    {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject enemy in enemies)
		{
			if (enemy.GetComponent<ItemDamage>().spawnID == _spawnID)
			{
				_enemies.Add(enemy);
				enemy.GetComponent<ItemDamage>().Player = _player;
				_currentEnemies++;
			}
		}
	}

	private void SpawnEnemy()
	{
		if (_currentEnemies < _numberOfEnemies)
		{
			//bool positionX = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
			bool positionX = true;
			Vector3 spawnPosition;
			spawnPosition.y = UnityEngine.Random.Range(_minY, _maxY);

			if (positionX)
			{
				spawnPosition = new Vector3(transform.position.x + 18, spawnPosition.y, 0);
			}
			else
			{
				spawnPosition = new Vector3(transform.position.x - 16, spawnPosition.y, 0);
			}

			GameObject enemy = Instantiate(_prefabEnemy, spawnPosition, Quaternion.identity);
			enemy.GetComponent<ItemDamage>().Player = _player;
			enemy.GetComponent<ItemDamage>().spawnID = _spawnID;
			_enemies.Add(enemy);
			_currentEnemies++;
		}

		foreach (var item in _enemies)
		{
			item.GetComponent<ItemDamage>().Friends = _enemies;
		}

		if (_currentEnemies < _numberOfEnemies)
		{
			Invoke("SpawnEnemy", _spawnTime); 
		}
	}
}
