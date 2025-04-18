using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RestartScreen  : Window
{
    public event Action RestartButtonClicked;

    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private TMP_Text _topScoresText;

    private float _closeAlpha = 0f;
    private float _openAlpha = 1f;

    private string _topScoresTextFormat = "Топ 5 очков:\n\n";

    public override void Close()
    {
        WindowGroup.alpha = _closeAlpha;
        ActionButton.interactable = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = _openAlpha;
        ActionButton.interactable = true;

        _scoreCounter.SaveScore(); 

        ShowTopScores();
    }

    private void ShowTopScores()
    {
        List<int> topScores = _scoreCounter.GetTopScores();
        _topScoresText.text = _topScoresTextFormat;

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
