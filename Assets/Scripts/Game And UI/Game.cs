using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private RestartScreen _restartScreen;
    [SerializeField] private BackgroundMusic _backgroundMusic;

    private void OnEnable()
    {
        _startScreen.PlayButtonClicked += OnPlayButtonClicked;
        _restartScreen.RestartButtonClicked += OnRestartButtonClicked;
        _player.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _player.GameOver -= OnGameOver;
    }

    private void Start()
    {
        Time.timeScale = 0;
        _startScreen.Open();
    }

    private void OnRestartButtonClicked()
    {
        _restartScreen.Close();
        StartGame();
    }

    private void OnPlayButtonClicked()
    {
        _startScreen.Close();
        StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1;
        _player.Reset();
        _backgroundMusic.PlayMusic();
    }

    private void OnGameOver()
    {
        Debug.Log("Показываем экран рестарта...");
        _backgroundMusic.StopMusic();
        _restartScreen.Open();
    }
}
