using UnityEngine;

public class EnemySpawner : Spawner<Enemy>
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _bulletPoolParent;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Transform _enemyPoolParent;
    [SerializeField] private int _initialPoolSize = 5;

    private ISpawnPositionProvider _positionProvider;
    private ISpawnDifficultyScaler _difficultyScaler;

    private float _minDistance = 10f; 
    private float _initialSpawnInterval = 4f; 
    private float _movementDelay = 0.01f; 
    private int _pointsPerEnemy = 10; 

    private float _minX = 20f;
    private float _maxX = 30f;
    private float _minY = -1f;
    private float _maxY = 6f;

    private float _minSpamInterwal = 1f;
    private float _spamInterval = 0.4f;

    protected override void Start()
    {
        base.Start();

        _positionProvider = new SpawnPositionGenerator(
            new Vector2(_minX, _maxX),
            new Vector2(_minY, _maxY),
            minDistance: _minDistance
        );

        _difficultyScaler = new SpawnDifficultyScaler(_initialSpawnInterval, 
            _minSpamInterwal, _spamInterval);

        InvokeRepeating(nameof(SpawnEnemy), _difficultyScaler.CurrentInterval, 
            _difficultyScaler.CurrentInterval);
    }

    public void Reset()
    {
        _pool.ResetPool(_prefab, _initialPoolSize, _poolMaxSize);
        ClearActiveObjects();

        _difficultyScaler.Reset();

        RestartSpawnCycle();
    }

    private void SpawnEnemy()
    {
        if (ActiveObjectsCount >= _poolMaxSize || _player == null)
            return;

        Enemy enemy = GetPreparedObjectFromPool();

        if (enemy != null)
        {
            Vector3 spawnPosition = _positionProvider.GetValidPosition(_player);
            enemy.transform.position = spawnPosition;

            if (_enemyPoolParent != null)
                enemy.transform.SetParent(_enemyPoolParent);

            enemy.Initialize(_player, _bulletPoolParent);
            ActivateObject(enemy);
            enemy.StartMovementDelay(_movementDelay); 

            enemy.Disappeared -= OnEnemyDisappeared;
            enemy.Disappeared += OnEnemyDisappeared;
        }
    }

    private void OnEnemyDisappeared(IDisappearable disappearable)
    {
        _scoreCounter?.Add(_pointsPerEnemy);

        _difficultyScaler.AdjustDifficulty();

        RestartSpawnCycle();
    }

    private void RestartSpawnCycle()
    {
        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), _difficultyScaler.CurrentInterval, 
            _difficultyScaler.CurrentInterval);
    }
}
