using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RestartScreen  : Window
{
    public event Action RestartButtonClicked;

    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private TMP_Text _topScoresText;
    [SerializeField] private PlayerInput _playerInput;

    private string _topScoresTextFormat = "Топ 5 очков:\n\n";

    private void OnEnable()
    {
        _playerInput.RestartRequested += OnSpaceRestart;
    }

    private void OnDisable()
    {
        _playerInput.RestartRequested -= OnSpaceRestart;
    }

    private void OnSpaceRestart()
    {
        if (WindowGroup.interactable == false)
            return;

        RestartButtonClicked?.Invoke();
    }


    public override void Open()
    {
        base.Open();
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
