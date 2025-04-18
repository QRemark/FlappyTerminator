using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action JumpRequested;
    public event Action AttackRequested;

    private KeyCode _jumpKey = KeyCode.Space;
    private KeyCode _attackKey = KeyCode.V;

    private void Update()
    {
        if (Input.GetKeyDown(_jumpKey))
        {
            JumpRequested?.Invoke();
        }

        if (Input.GetKeyDown(_attackKey))
        {
            AttackRequested?.Invoke();
        }
    }
}
