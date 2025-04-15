using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class EnemySpawner : Spawner<Enemy>
{
    [SerializeField] private float _spawnInterval = 4f;
    [SerializeField] private Vector2 _spawnRangeX = new Vector2(20f, 30f);
    [SerializeField] private Vector2 _moveRange = new Vector2(-1, 6f);
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _bulletPoolParent;
    [SerializeField] private float _minDistanceBetweenEnemies = 10f;
    [SerializeField] private ScoreCounter _scoreCounter;

    private List<Vector3> _lastPositions = new List<Vector3>();
    private int _maxTrackedPositions = 5;


    private void Start()
    {
        base.Start();

        InvokeRepeating(nameof(SpawnEnemy), _spawnInterval + 1f, _spawnInterval);
    }

    public void Reset()
    {
        _poolMaxSize = 3;
        _spawnInterval = 4;

        _pool.ResetPool(_prefab, 2, _poolMaxSize);

        foreach (var enemy in _pool.ActiveObjects)
        {
            BulletSpawner bulletSpawner = enemy.GetComponent<BulletSpawner>();

            if (bulletSpawner != null)
            {
                bulletSpawner.Reset();
            }
        }

        ClearActiveObjects();

        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), _spawnInterval, _spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (ActiveObjectsCount < _poolMaxSize && _player != null)
        {
            Enemy enemy = GetPreparedObjectFromPool();

            if (enemy != null)
            {
                Vector3 spawnPosition = GetValidSpawnPosition();

                enemy.transform.position = spawnPosition;
                enemy.Initialize(_player, _bulletPoolParent);
                enemy.StartMovementDelay(0.01f);

                ActivateObject(enemy);

                enemy.Disappeared -= OnEnemyDisappeared;
                enemy.Disappeared += OnEnemyDisappeared;


                TrackPositions(spawnPosition);
            }
        }
    }

    private void OnEnemyDisappeared(IDisappearable disappearable)
    {
        _scoreCounter?.Add(10);

        if (_spawnInterval > 1f)
        {
            _spawnInterval -= 0.4f;

            CancelInvoke(nameof(SpawnEnemy));

            InvokeRepeating(nameof(SpawnEnemy), _spawnInterval, _spawnInterval);
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            float minDistanceX = Mathf.Max(_minDistanceBetweenEnemies, _spawnRangeX.x);
            float randomX = Random.Range(minDistanceX, _spawnRangeX.y);
           
            float randomY = Random.Range(_minDistanceBetweenEnemies, _moveRange.y);

            Vector3 position = new Vector3(
                _player.position.x + randomX,
                _player.position.y + randomY,
                0
            );

            if (IsPositionValid(position))
            {
                Debug.Log($"Спавн врага: PlayerX={_player.position.x}, SpawnX={position.x}, randomX={randomX}");
                return position;
            }

            Debug.Log($"Спавн врага: PlayerX={_player.position.x}, SpawnX={position.x}, randomX={randomX}");
        }

        return new Vector3(_player.position.x+ 25f, -1, 0);
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (var trackedPos in _lastPositions)
        {
            if (Vector3.Distance(position, trackedPos) < _minDistanceBetweenEnemies)
            {
                return false;
            }
        }

        return true;
    }

    private void TrackPositions(Vector3 position)
    {
        if (_lastPositions.Count >= _maxTrackedPositions)
        {
            _lastPositions.RemoveAt(0);
        }

        _lastPositions.Add(position);
    }
}