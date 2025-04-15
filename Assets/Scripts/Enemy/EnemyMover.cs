using System;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _moveRange = 1.5f;
    [SerializeField] private float _lerpSpeed = 1.0f;

    [Header("Приближение к игроку")]
    [SerializeField] private float _approachDuration = 1f;
    [SerializeField] private Vector2 _approachDistanceRange = new Vector2(6f, 13f);
    [SerializeField] private Vector2 _approachYOffsetRange = new Vector2(-4f, 4f);

    [Header("Следование за игроком")]
    [SerializeField] private float _followTargetUpdateInterval = 3f;
    [SerializeField] private float _followMinDistance = 15f;
    [SerializeField] private Vector2 _followYOffsetRange = new Vector2(-5f, 7f);

    private Transform _player;
    private float _timeOffset;

    private float _minY;
    private float _maxY;

    private float _smoothPlayerY;
    private Vector3 _smoothPosition;

    private float _movementTimer;
    private float _targetFollowDistance;
    private float _targetYOffset;
    private float _currentFollowDistance;
    private float _currentYOffset;

    private bool _isApproaching;
    private float _approachTime;
    private Vector3 _approachStartPosition;

    private bool _canFollow;

    public Action OnApproachComplete;

    private void Start()
    {
        var cam = Camera.main;
        var camHeight = cam.orthographicSize;
        var camY = cam.transform.position.y;

        _minY = camY - camHeight;
        _maxY = camY + camHeight - 1f;
    }

    public void Setup(Transform player, float timeOffset)
    {
        _player = player;
        _timeOffset = timeOffset;
        _smoothPlayerY = player.position.y;
        _smoothPosition = transform.position;
    }

    public void ResetState()
    {
        _isApproaching = false;
        _canFollow = false;
    }

    public void StartFollowing()
    {
        _targetFollowDistance = UnityEngine.Random.Range(_approachDistanceRange.x, _approachDistanceRange.y);
        _targetYOffset = UnityEngine.Random.Range(_approachYOffsetRange.x, _approachYOffsetRange.y);

        _currentFollowDistance = _targetFollowDistance;
        _currentYOffset = _targetYOffset;

        _approachStartPosition = transform.position;
        _approachTime = 0f;
        _isApproaching = true;
    }

    public void Move()
    {
        if (_player == null) return;

        if (_isApproaching)
        {
            HandleApproach();
        }
        else if (_canFollow)
        {
            HandleFollow();
        }
    }

    private void HandleApproach()
    {
        _approachTime += Time.deltaTime;
        float t = Mathf.Clamp01(_approachTime / _approachDuration);

        Vector3 targetPosition = GetApproachTargetPosition();
        transform.position = Vector3.Lerp(_approachStartPosition, targetPosition, t);

        if (t >= 1f)
        {
            _isApproaching = false;
            _canFollow = true;
            _smoothPosition = transform.position;
            OnApproachComplete?.Invoke();
        }
    }

    private void HandleFollow()
    {
        _movementTimer += Time.deltaTime;

        _smoothPlayerY = Mathf.Lerp(_smoothPlayerY, _player.position.y, Time.deltaTime * 2f);

        if (_movementTimer >= _followTargetUpdateInterval)
        {
            UpdateFollowTargets();
            _movementTimer = 0f;
        }

        UpdateCurrentFollowValues();

        float waveY = Mathf.Sin((Time.time + _timeOffset) * _speed) * _moveRange;
        float rawY = _smoothPlayerY + waveY + _currentYOffset;
        float clampedY = Mathf.Clamp(rawY, _minY, _maxY);

        Vector3 targetPos = new Vector3(
            _player.position.x + _currentFollowDistance,
            clampedY,
            0f
        );

        _smoothPosition = Vector3.Lerp(_smoothPosition, targetPos, Time.deltaTime * _lerpSpeed);
        transform.position = _smoothPosition;
    }

    private void UpdateFollowTargets()
    {
        _targetFollowDistance = UnityEngine.Random.Range(_followMinDistance, _followMinDistance); // Можно добавить min-max, если хочешь больше вариативности
        _targetYOffset = UnityEngine.Random.Range(_followYOffsetRange.x, _followYOffsetRange.y);
    }

    private void UpdateCurrentFollowValues()
    {
        _currentFollowDistance = Mathf.Lerp(
            _currentFollowDistance,
            _targetFollowDistance,
            Time.deltaTime * _lerpSpeed
        );

        _currentYOffset = Mathf.Lerp(
            _currentYOffset,
            _targetYOffset,
            Time.deltaTime * _lerpSpeed
        );
    }

    private Vector3 GetApproachTargetPosition()
    {
        return new Vector3(
            _player.position.x + _currentFollowDistance,
            _player.position.y + _currentYOffset,
            0f
        );
    }

    public Vector3 GetSmoothPosition()
    {
        return _smoothPosition;
    }
}
