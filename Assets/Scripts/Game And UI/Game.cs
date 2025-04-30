using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private RestartScreen _restartScreen;
    [SerializeField] private BackgroundMusic _backgroundMusic;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private GameObject _bulletBarContainer;
    [SerializeField] private PauseScreen _pauseScreen;
    [SerializeField] private BackgroundLooper _backgroundLooper;

    private bool _isPaused = false;

    private void OnEnable()
    {
        _startScreen.PlayButtonClicked += OnPlayButtonClicked;
        _restartScreen.RestartButtonClicked += OnRestartButtonClicked;
        _player.GameOver += OnGameOver;
        _pauseScreen.ResumeButtonClicked += ResumeGame;
        InputEvents.PauseRequested += TogglePause;
    }

    private void OnDisable()
    {
        _player.GameOver -= OnGameOver;
        _pauseScreen.ResumeButtonClicked -= ResumeGame;
        InputEvents.PauseRequested -= TogglePause;
    }

    private void Start()
    {
        _startScreen.gameObject.SetActive(true);
        _restartScreen.gameObject.SetActive(false); 
        _pauseScreen.gameObject.SetActive(false); 

        Time.timeScale = 0;
        _scoreView.Hide();
        _bulletBarContainer.SetActive(false);
        _startScreen.Open();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_restartScreen.isActiveAndEnabled && !_startScreen.isActiveAndEnabled)
        {
            if (!_isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    private void TogglePause()
    {
        if (_startScreen.WindowGroup.alpha > 0f || _restartScreen.WindowGroup.alpha > 0f)
            return;

        if (!_isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
    {
        _pauseScreen.gameObject.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0f;
        _backgroundMusic.PauseMusic();
        _pauseScreen.Open();
    }

    private void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _backgroundMusic.ContinueMusic();
        _pauseScreen.Close();
    }

    private void OnRestartButtonClicked()
    {
        _restartScreen.Close();
        _pauseScreen.gameObject.SetActive(true);
        _pauseScreen.Close();
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
        _backgroundLooper.ResetBackground();
        _scoreView.Show();
        _backgroundMusic.PlayMusic();
        _bulletBarContainer.SetActive(true);
    }

    private void OnGameOver()
    {
        _restartScreen.gameObject.SetActive(true);
        _backgroundMusic.StopMusic();
        _scoreView.Hide();
        _bulletBarContainer.SetActive(false);
        _restartScreen.Open();
    }
}

