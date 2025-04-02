using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : Window
{
    public event Action PlayButtonClicked;

    public override void Close()
    {
        WindowGroup.alpha = 0f;
        ActionButton.interactable = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1f;
        ActionButton.interactable = true;
    }

    public override void OnButtonClick()
    {
        PlayButtonClicked?.Invoke();
    }
}
