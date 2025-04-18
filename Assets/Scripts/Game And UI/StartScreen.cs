using System;

public class StartScreen : Window
{
    private float _closeAlpha = 0f;
    private float _openAlpha = 1f;

    public event Action PlayButtonClicked;

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
        PlayButtonClicked?.Invoke();
    }
}
