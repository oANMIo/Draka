using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	[SerializeField] private int _spawnID = 1;
	[SerializeField] private float _maxY = 1.0f;
	[SerializeField] private float _minY = -4.5f;
	[SerializeField] private int _numberOfEnemies = 2;
	[SerializeField] private float _spawnTime = 1.0f;
	[SerializeField] private Enemy _prefabEnemy;
	[SerializeField] private EnemyGuard _prefabEnemyGuard;
	[SerializeField] private Transform _spawnPointEnemyGuard;

	private int _currentEnemies;
	private bool _spawnEnemyGuard;
	private Camera _camera;
	private Player _player;
	private List<ItemDamage> _enemies = new List<ItemDamage>();

	public static Action onStartSpawn;
	public static Action onAllKill;

	private void Awake()
    {
		_camera = Camera.main;
		_currentEnemies = 0;
		_spawnEnemyGuard = false;
	}

	private void OnEnable()
	{
		ItemDamage.onDeathEnemy += UpdateCountEnemy; 
	}

	private void OnDisable()
	{
		ItemDamage.onDeathEnemy -= UpdateCountEnemy;
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
			SpawnEnemy();
		}
	}
	
	private void SpawnEnemy()
	{
        if (_prefabEnemyGuard != null && _spawnEnemyGuard == false)
        {
			EnemyGuard enemyGuard = Instantiate(_prefabEnemyGuard, _spawnPointEnemyGuard.position, Quaternion.identity);
			enemyGuard.Player = _player;
			enemyGuard.spawnID = _spawnID;
			_enemies.Add(enemyGuard);
			_currentEnemies++;
			_spawnEnemyGuard = true;
		}
		
		bool positionX = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
		//bool positionX = true;
		Vector3 spawnPosition;
		spawnPosition.y = UnityEngine.Random.Range(_minY, _maxY);

		if (positionX)
		{
			spawnPosition = new Vector3(transform.position.x + 17, spawnPosition.y, 0);
		}
		else
		{
			spawnPosition = new Vector3(transform.position.x - 17, spawnPosition.y, 0); 
		}

		Enemy enemy = Instantiate(_prefabEnemy, spawnPosition, Quaternion.identity);
		enemy.Target = _player;
		enemy.spawnID = _spawnID;
		_enemies.Add(enemy);
		_currentEnemies++;

		foreach (var item in _enemies)
		{
			item.Friends = _enemies;
		}

		if (_currentEnemies < _numberOfEnemies)
		{
			Invoke("SpawnEnemy", _spawnTime);
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
