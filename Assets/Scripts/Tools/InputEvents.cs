using System;

public static class InputEvents
{
    public static event Action PauseRequested;
    public static event Action RestartRequested;

    public static void RaisePause() => PauseRequested?.Invoke();
    public static void RaiseRestart() => RestartRequested?.Invoke();
}