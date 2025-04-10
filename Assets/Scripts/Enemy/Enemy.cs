using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDisappearable
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _moveRange = 1.5f;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _attackSprite;
    [SerializeField] private BulletSpawner _bulletSpawner;

    private SpriteRenderer _spriteRenderer;
    private Transform _bulletPoolParent;
    private Transform _player;
    private AttackAudio _enemyAudio;

    private bool _canFollow = false;
    private float _timeOffset;

    private bool _isDisappearing = false;

    private float _currentFollowDistance;
    private float _targetFollowDistance;
    private float _currentYOffset;
    private float _targetYOffset;
    private float _movementTimer;
    private float _lerpSpeed = 1.5f;

    private bool _isApproaching = false;
    private Vector3 _approachStartPosition;
    private float _approachTime;
    private float _approachDuration = 1f;

    public event Action<IDisappearable> Disappeared;

    public void Initialize(Transform player, Transform poolParent)
    {
        _player = player;
        _bulletPoolParent = poolParent;
        _timeOffset = UnityEngine.Random.Range(-10f, 10f);

        _targetFollowDistance = UnityEngine.Random.Range(6f, 13f);
        _currentFollowDistance = _targetFollowDistance;
        _targetYOffset = UnityEngine.Random.Range(-4f, 4f);
        _currentYOffset = _targetYOffset;

        if (_bulletSpawner != null)
        {
            _bulletSpawner.SetPoolParent(_bulletPoolParent);
        }
    }

    private void Awake()
    {
        _enemyAudio = GetComponent<AttackAudio>();
    }

    private void OnEnable()
    {
        Debug.Log($"[ENEMY ENABLED] Position: {transform.position.x}, Player: {_player?.position.x}");
        _isDisappearing = false;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _defaultSprite;

        transform.localScale = new Vector3(0.35f, 0.35f, 1f);

        InvokeRepeating(nameof(FireBullet), 3f, 3f);
    }

    private void OnDisable()
    {
        _isDisappearing = false;
        CancelInvoke(nameof(FireBullet));
        _canFollow = false; 
    }

    private void Update()
    {
        if (_player == null) return;
       
        if (_isApproaching)
        {
            _approachTime += Time.deltaTime;
            float t = Mathf.Clamp01(_approachTime / _approachDuration);

            Vector3 targetPosition = new Vector3(
                _player.position.x + _currentFollowDistance,
                _player.position.y + _currentYOffset,
                0
            );

            transform.position = Vector3.Lerp(_approachStartPosition, targetPosition, t);

            if (t >= 1f)
            {
                _isApproaching = false;
                _canFollow = true;
            }

            return;
        }

        if (_canFollow)
        {
            _movementTimer += Time.deltaTime;

            if (_movementTimer >= 3f)
            {
                _targetFollowDistance = UnityEngine.Random.Range(6f, 13f);
                _targetYOffset = UnityEngine.Random.Range(-5f, 7f);
                _movementTimer = 0f;
            }

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

            float pingPongY = Mathf.PingPong(
                (Time.time + _timeOffset) * _speed,
                _moveRange
            );

            transform.position = new Vector3(
                _player.position.x + _currentFollowDistance,
                _player.position.y + pingPongY + _currentYOffset,
                0
            );
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out _))
        {
            Disappear();
        }
    }

    public void Disappear()
    {
        if (_isDisappearing) return;
        _isDisappearing = true;
        Disappeared?.Invoke(this);
    }

    public void Attack()
    {
        if (_attackSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = _attackSprite;
            _enemyAudio?.AttackSound();
            Invoke(nameof(ResetSprite), 0.5f);
        }
    }

    private void ResetSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = _defaultSprite;
        }
    }

    private void FireBullet()
    {
        if (_bulletSpawner != null)
        {
            Attack();
            Bullet bullet = _bulletSpawner.Fire(transform.position, false);

            if (bullet != null)
            {
                bullet.SetOwner(transform);
            }
        }
    }

    public void StartMovementDelay(float delay)
    {
        Invoke(nameof(StartFollowing), delay);
    }

    private void StartFollowing()
    {
        _isApproaching = true;
        _approachStartPosition = transform.position;
        _approachTime = 0f;
    }
}