using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(ScoreCounter), typeof (CollisionHandler))]

public class Player : MonoBehaviour
{
    private PlayerMover _playerMover;
    private ScoreCounter _scoreCounter;
    private CollisionHandler _collisionHandler;
    private BulletSpawner _bulletSpawner;
    private AttackAudio _playerAudio;


    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _attackSprite;
    
    private SpriteRenderer _spriteRenderer;


    public event Action GameOver;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _scoreCounter = GetComponent<ScoreCounter>();
        _collisionHandler = GetComponent<CollisionHandler>();
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAudio = GetComponent<AttackAudio>();
    }

    private void OnEnable()
    {
        _collisionHandler.CollisionDetected += ProcessCollision;
        GameOver += OnGameOver;

        if (_spriteRenderer != null && _defaultSprite != null)
        {
            _spriteRenderer.sprite = _defaultSprite;
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
    }

    private void OnDisable()
    {
        _collisionHandler.CollisionDetected -= ProcessCollision;
        GameOver -= OnGameOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Shoot();
        }
    }

    public void TriggerGameOver()
    {
        GameOver?.Invoke();
    }

    private void Shoot()
    {
        if (_bulletSpawner != null)
        {
            Vector3 spawnPosition = transform.position + transform.right * 0.5f; 
            Bullet bullet = _bulletSpawner.Fire(spawnPosition, true);

            if (bullet != null)
            {
                bullet.SetOwner(transform); 
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (_attackSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = _attackSprite;
            _playerAudio?.AttackSound();
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

    private void ProcessCollision(IInteractable interactable)
    {
        interactable.Interact();


        if (interactable is Earth)
        {
            GameOver?.Invoke();
        }
        //else if (interactable is ScoreZone)
        //{
        //    _scoreCounter.Add();
        //}
    }

    public void Reset()
    {
        _scoreCounter.Reset();
        _playerMover.Reset();
        Time.timeScale = 1; ////временно
    }
    private void OnGameOver()
    {
        Debug.Log("Игра остановлена.");
        Time.timeScale = 0; 
    }
}
