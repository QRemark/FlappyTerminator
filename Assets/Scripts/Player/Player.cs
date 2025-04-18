using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(PlayerInput), typeof(PlayerAttack))]
[RequireComponent(typeof(CollisionHandler))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _playerBulletPoolParent;
    [SerializeField] private EnemySpawner _enemySpawner;

    private PlayerMover _playerMover;
    private PlayerInput _playerInput;
    private PlayerAttack _playerAttack;
    private CollisionHandler _collisionHandler;
    private ScoreCounter _scoreCounter;
    private BulletSpawner _bulletSpawner;

    private Vector3 _startPosition;

    public event Action GameOver;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAttack = GetComponent<PlayerAttack>();
        _collisionHandler = GetComponent<CollisionHandler>();
        _scoreCounter = GetComponent<ScoreCounter>();
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        _startPosition = transform.position;

        if (_bulletSpawner != null && _playerBulletPoolParent != null)
        {
            _bulletSpawner.SetPoolParent(_playerBulletPoolParent);
        }
    }

    private void OnEnable()
    {
        _playerInput.JumpRequested += _playerMover.Jump;
        _playerInput.AttackRequested += _playerAttack.Attack;
        _collisionHandler.CollisionDetected += ProcessCollision;
        GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _playerInput.JumpRequested -= _playerMover.Jump;
        _playerInput.AttackRequested -= _playerAttack.Attack;
        _collisionHandler.CollisionDetected -= ProcessCollision;
        GameOver -= OnGameOver;
    }

    private void ProcessCollision(IInteractable interactable)
    {
        interactable.Interact();

        if (interactable is Earth)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        GameOver?.Invoke();
    }

    public void Reset()
    {
        _scoreCounter.Reset();
        _playerMover.Reset();
        _playerAttack.ResetAttack();
        _enemySpawner?.Reset();
        transform.position = _startPosition;
        Time.timeScale = 1;
    }

    private void OnGameOver()
    {
        _enemySpawner.Reset();
        _bulletSpawner.Reset();
        Debug.Log("Игра остановлена.");
        Time.timeScale = 0;
    }
}
