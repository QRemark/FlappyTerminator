using System;

public interface IGameInput
{
    event Action PauseRequested;
    event Action RestartRequested;
}
