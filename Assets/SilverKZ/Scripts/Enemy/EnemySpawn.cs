using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	[SerializeField] private float _maxY = 1.0f;
	[SerializeField] private float _minY = -4.5f;
	[SerializeField] private int _numberOfEnemies = 2;
	[SerializeField] private float _spawnTime = 1.0f;
	[SerializeField] private Enemy _prefabEnemy;

	private int _currentEnemies;
	private int _countAliveEnemies;
	private Camera _camera;
	private Player _player;
	private List<Enemy> _enemies = new List<Enemy>();

	public static Action onStartSpawn;
	public static Action onAllKill;

	private void Awake()
    {
		_camera = Camera.main;
		_currentEnemies = 0;
		_countAliveEnemies = _numberOfEnemies;
	}

	private void OnEnable()
	{
		Enemy.onDeathEnemy += UpdateCountEnemy; 
	}

	private void OnDisable()
	{
		Enemy.onDeathEnemy -= UpdateCountEnemy;
	}

	private void UpdateCountEnemy(int number)
    {
		if (_currentEnemies >= _numberOfEnemies)
		{
			_countAliveEnemies--;

			if (_countAliveEnemies == 0)
			{
				AllKill();
				Destroy(gameObject);
			}
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
			SpawnEnemy();
		}
	}
	
	private void SpawnEnemy()
	{
		//bool positionX = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
		bool positionX = true;
		Vector3 spawnPosition;
		spawnPosition.y = UnityEngine.Random.Range(_minY, _maxY);

		if (positionX)
		{
			spawnPosition = new Vector3(transform.position.x + 15, spawnPosition.y, 0);
		}
		else
		{
			spawnPosition = new Vector3(transform.position.x - 12, spawnPosition.y, 0);
		}

		Enemy enemy = Instantiate(_prefabEnemy, spawnPosition, Quaternion.identity);
		enemy.Target = _player;
		_enemies.Add(enemy);
		_currentEnemies++;
		
		if (_currentEnemies < _numberOfEnemies)
		{
			Invoke("SpawnEnemy", _spawnTime);
		}

		if (_currentEnemies >= _numberOfEnemies)
		{
            foreach (var item in _enemies)
            {
				item.Friends = _enemies;

			}
		}
	}

	public void StartSpawn()
	{
		onStartSpawn?.Invoke();
	}

	public void AllKill()
	{
		onAllKill?.Invoke();
	}
}
