using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _scorePanel; 

    private int _currentScore;

    private string _scoreTextFormat = "—чет: ";

    public void UpdateScore(int score)
    {
        _currentScore = score;
        _scoreText.text = $"{_scoreTextFormat}{_currentScore}";
    }

    public void Show()
    {
        _scorePanel.SetActive(true);
    }

    public void Hide()
    {
        _scorePanel.SetActive(false);
    }
}
