using System;

public static class InputEvents
{
    public static event Action PauseRequested;

    public static void RaisePause() => PauseRequested?.Invoke();
}
