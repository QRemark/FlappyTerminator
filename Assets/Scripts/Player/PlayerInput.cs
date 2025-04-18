using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action JumpRequested;
    public event Action AttackRequested;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpRequested?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            AttackRequested?.Invoke();
        }
    }
}
