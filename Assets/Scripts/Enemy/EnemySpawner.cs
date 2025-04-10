using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : Spawner<Enemy>
{
    [SerializeField] private float _spawnInterval = 4f;
    [SerializeField] private Vector2 _spawnRangeX = new Vector2(10f, 20f);
    [SerializeField] private Vector2 _moveRange = new Vector2(0, 0f);
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _bulletPoolParent;
    [SerializeField] private float _minDistanceBetweenEnemies = 6f;
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
        Debug.Log($"������� ������ �����. ��������: {ActiveObjectsCount}, ����: {_poolMaxSize}");

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
                Debug.Log("���� ���������!");
            }
        }
    }

    private void OnEnemyDisappeared(IDisappearable disappearable)
    {
        _scoreCounter?.Add(10);

        if (_poolMaxSize < 5)
        {
            _poolMaxSize++;

            _pool.Resize(_poolMaxSize);
        }

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
            float randomX = Random.Range(_spawnRangeX.x, _spawnRangeX.y);
            float randomY = Random.Range(_moveRange.x, _moveRange.y);

            Vector3 position = new Vector3(
                _player.position.x + randomX,
                _player.position.y + randomY,
                0
            );

            if (IsPositionValid(position))
            {
                return position;
            }
        }

        return _player.position + new Vector3(25f, -1, 0);
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