using UnityEngine;
using System;

public class PauseScreen : Window
{
    public event Action ResumeButtonClicked;

    private float _closeAlpha = 0f;
    private float _openAlpha = 1f;

    public override void Close()
    {
        WindowGroup.alpha = _closeAlpha;
        ActionButton.interactable = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = _openAlpha;
        ActionButton.interactable = true;
    }

    public override void OnButtonClick()
    {
        ResumeButtonClicked?.Invoke();
    }
}
