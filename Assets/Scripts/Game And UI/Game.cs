using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private RestartScreen _restartScreen;
    [SerializeField] private BackgroundMusic _backgroundMusic;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private GameObject _bulletBarContainer;

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
        _scoreView.Hide();
        _bulletBarContainer.SetActive(false);
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
        _scoreView.Show();
        _backgroundMusic.PlayMusic();
        _bulletBarContainer.SetActive(true);
    }

    private void OnGameOver()
    {
        _backgroundMusic.StopMusic();
        _scoreView.Hide();
        _bulletBarContainer.SetActive(false);
        _restartScreen.Open();
    }
}
