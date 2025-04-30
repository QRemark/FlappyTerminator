using UnityEngine;
using System;

public class PauseScreen : Window
{
    public event Action ResumeButtonClicked;

    public override void OnButtonClick()
    {
        ResumeButtonClicked?.Invoke();
    }
}
