using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private int _score;

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
        Debug.Log($"ќчки: {_score}");
    }

    public void Reset()
    {
        _score = 0;
        Debug.Log("—чЄт сброшен.");
    }
}
