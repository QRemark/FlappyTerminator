using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RestartScreen  : Window
{
    public event Action RestartButtonClicked;

    public override void Close()
    {
        WindowGroup.alpha = 0f;
        ActionButton.interactable = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1f;
        ActionButton.interactable = true;
        Debug.Log("Ёкран рестарта открыт");
    }

    public override void OnButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }
}
