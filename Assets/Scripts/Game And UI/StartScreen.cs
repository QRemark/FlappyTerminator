using System;

public class StartScreen : Window
{
    public event Action PlayButtonClicked;

    public override void OnButtonClick()
    {
        PlayButtonClicked?.Invoke();
    }
}
