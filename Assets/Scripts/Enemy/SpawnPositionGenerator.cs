using UnityEngine;
using System.Collections.Generic;

public class SpawnPositionGenerator : ISpawnPositionProvider
{
    //private const int DefaultMaxTrackedPositions = 5;
    //private readonly Vector2 _spawnRangeX;
    //private readonly Vector2 _spawnRangeY;
    //private readonly float _minDistance;
    //private readonly int _maxTrackedPositions;

    //private readonly int _maxAttempts = 10;
    //private readonly float _fallbackOffsetX = 25f;
    //private readonly float _fallbackY = -1f;

    //private readonly List<Vector3> _trackedPositions = new();

    private const int DefaultMaxTrackedPositions = 5;
    private Vector2 _spawnRangeX;
    private Vector2 _spawnRangeY;
    private float _minDistance;
    private int _maxTrackedPositions;

    private int _maxAttempts = 10;
    private float _fallbackOffsetX = 25f;
    private float _fallbackY = -1f;

    private readonly List<Vector3> _trackedPositions = new();

    public SpawnPositionGenerator(Vector2 spawnRangeX, Vector2 spawnRangeY, 
        float minDistance, int maxTracked = DefaultMaxTrackedPositions)
    {
        _spawnRangeX = spawnRangeX;
        _spawnRangeY = spawnRangeY;
        _minDistance = minDistance;
        _maxTrackedPositions = maxTracked;
    }

    public Vector3 GetValidPosition(Transform player)
    {
        for (int i = 0; i < _maxAttempts; i++)
        {
            float randomX = Random.Range(Mathf.Max(_minDistance, _spawnRangeX.x), 
                _spawnRangeX.y);
            float randomY = Random.Range(_spawnRangeY.x, _spawnRangeY.y);

            Vector3 position = new(player.position.x + randomX, player.position.y + randomY, 0);

            if (IsFarEnough(position))
            {
                Track(position);
                return position;
            }
        }

        Vector3 fallback = new(player.position.x + _fallbackOffsetX, _fallbackY, 0);

        Track(fallback);

        return fallback;
    }

    private bool IsFarEnough(Vector3 position)
    {
        foreach (var tracked in _trackedPositions)
        {
            if (Vector3.Distance(tracked, position) < _minDistance)
                return false;
        }

        return true;
    }

    private void Track(Vector3 position)
    {
        if (_trackedPositions.Count >= _maxTrackedPositions)
            _trackedPositions.RemoveAt(0);

        _trackedPositions.Add(position);
    }
}
