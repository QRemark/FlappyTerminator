using System;

public class BaseWindow : Window
{
    public event Action ButtonClicked;

    public override void OnButtonClick()
    {
        ButtonClicked?.Invoke();
    }
}
