using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreView;
    
    private int _score;

    private List<int> _topScores = new List<int>();

    public int CurrentScore => _score;

    public void RegisterEnemy(Enemy enemy)
    {
        enemy.Disappeared -= OnEnemyDisappeared;
        enemy.Disappeared += OnEnemyDisappeared;
    }

    private void OnEnemyDisappeared(IDisappearable disappearable)
    {
        Add(10);
    }

    public void Add(int points)
    {
        _score += points;
        _scoreView.UpdateScore(_score);
        Debug.Log($"ќчки: {_score}");
    }

    public void Reset()
    {
        _score = 0;
        _scoreView.UpdateScore(_score);
        Debug.Log("—чЄт сброшен.");
    }
    public void SaveScore()
    {
        _topScores.Add(_score);
        _topScores.Sort((a, b) => b.CompareTo(a)); 
        if (_topScores.Count > 5)
            _topScores.RemoveAt(5); 
    }

    public List<int> GetTopScores()
    {
        return new List<int>(_topScores);
    }
}
