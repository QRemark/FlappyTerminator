using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action JumpRequested;
    public event Action AttackRequested;

    private KeyCode _jumpKey = KeyCode.Space;
    private KeyCode _attackKey = KeyCode.V;
    private KeyCode _restartKey = KeyCode.Z;

    private KeyCode _pauseKey = KeyCode.Escape;

    private void Update()
    {
        if (Input.GetKeyDown(_jumpKey)) 
            JumpRequested?.Invoke();

        if (Input.GetKeyDown(_attackKey)) 
            AttackRequested?.Invoke();

        if (Input.GetKeyDown(_pauseKey)) 
            InputEvents.RaisePause();

        if (Input.GetKeyDown(_restartKey))
            InputEvents.RaiseRestart();
    }
}
