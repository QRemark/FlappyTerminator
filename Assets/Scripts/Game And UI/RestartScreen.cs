using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RestartScreen  : Window
{
    public event Action RestartButtonClicked;

    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private TMP_Text _topScoresText;

    public override void Close()
    {
        WindowGroup.alpha = 0f;
        ActionButton.interactable = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1f;
        ActionButton.interactable = true;

        _scoreCounter.SaveScore(); 

        ShowTopScores();

        Debug.Log("Экран рестарта открыт");
    }

    private void ShowTopScores()
    {
        List<int> topScores = _scoreCounter.GetTopScores();
        _topScoresText.text = "Топ 5 очков:\n\n";

        for (int i = 0; i < topScores.Count; i++)
        {
            _topScoresText.text += $"{i + 1}. {topScores[i]}\n";
        }
    }

    public override void OnButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }
}
