using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour, IDisappearable
{
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private EnemyAttack _enemyAttack;

    private Transform _bulletPoolParent;
    private Transform _player;
    private float _timeOffset;
    private bool _isDisappearing;

    private float _timeOffsetMin = 0f;
    private float _timeOffsetMax = 10f;
    private float _localScaleModificator = 0.35f;

    public event Action<IDisappearable> Disappeared;

    public void Initialize(Transform player, Transform poolParent)
    {
        _player = player;
        _bulletPoolParent = poolParent;
        _timeOffset = UnityEngine.Random.Range(_timeOffsetMin, _timeOffsetMax);

        _enemyMover.Setup(_player, _timeOffset);
        _enemyMover.OnApproachComplete = _enemyAttack.EnableAttack;

        _enemyAttack.Setup(_player, _bulletPoolParent);
    }

    private void OnEnable()
    {
        _isDisappearing = false;
        transform.localScale = new Vector3(_localScaleModificator, _localScaleModificator, 1f);

        _enemyMover.ResetState();
        _enemyAttack.ResetState();
    }

    private void OnDisable()
    {
        _isDisappearing = false;
        _enemyAttack.DisableAttack();
        _enemyMover.ResetState();
    }

    private void Update()
    {
        _enemyMover.Move();
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

    public void StartMovementDelay(float delay)
    {
        StartCoroutine(StartMovementAfterDelay(delay));
    }

    private IEnumerator StartMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _enemyMover.StartFollowing();
    }
}
