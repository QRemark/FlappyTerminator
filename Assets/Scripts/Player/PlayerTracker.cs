using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _offsetX;

    private void Update()
    {
        var position = transform.position;
        position.x = _player.transform.position.x + _offsetX;
        transform.position = position;
    }
}
