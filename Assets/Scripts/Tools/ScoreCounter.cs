using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreView;
    
    private int _score;
    private int _topScroresCount = 5;
    private int _resetScore = 0;

    private List<int> _topScores = new List<int>();

    public void Reset()
    {
        _score = _resetScore;
        _scoreView.UpdateScore(_score);
    }

    public void Add(int points)
    {
        _score += points;
        _scoreView.UpdateScore(_score);
    }

    public void SaveScore()
    {
        _topScores.Add(_score);
        _topScores.Sort((current, newScore) => newScore.CompareTo(current)); 

        if (_topScores.Count > _topScroresCount)
            _topScores.RemoveAt(_topScroresCount); 
    }

    public List<int> GetTopScores()
    {
        return new List<int>(_topScores);
    }
}
