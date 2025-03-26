using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(ScoreCounter), typeof (CollisionHandler))]
public class Player : MonoBehaviour
{
    private PlayerMover _playerMover;
    private ScoreCounter _scoreCounter;
    private CollisionHandler _collisionHandler;

    public event Action GameOver;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _scoreCounter = GetComponent<ScoreCounter>();
        _collisionHandler = GetComponent<CollisionHandler>();
    }

    private void OnEnable()
    {
        _collisionHandler.CollisionDetected += ProcessCollision;
        GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _collisionHandler.CollisionDetected -= ProcessCollision;
        GameOver -= OnGameOver;
    }

    private void ProcessCollision(IInteractable interactable)
    {
        interactable.Interact();


        if (interactable is Earth)
        {
            GameOver?.Invoke();
        }
        else if (interactable is ScoreZone)
        {
            _scoreCounter.Add();
        }
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
