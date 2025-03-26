using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private Transform _player;  
    [SerializeField] private float _speedFactor = 0.9f;

    private float _spriteWidth;
    private Transform[] _backgrounds;
    private Vector3 _lastPlayerPosition;

    private void Start()
    {
        
        _backgrounds = new Transform[3];

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i] = transform.GetChild(i);
        }
        
        _spriteWidth = _backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        _lastPlayerPosition = _player.position;
    }

    private void Update()
    {
        float playerDeltaX = _player.position.x - _lastPlayerPosition.x;

        foreach (var bg in _backgrounds)
        {
            bg.position += Vector3.right * playerDeltaX * _speedFactor;
        }

        foreach (var bg in _backgrounds)
        {
            if (bg.position.x < _player.position.x - _spriteWidth * 1.5f)
            {
                MoveBackgroundToRight(bg);
            }
        }


        _lastPlayerPosition = _player.position;
    }

    private void MoveBackgroundToRight(Transform bg)
    {
        Transform rightmost = _backgrounds[0];

        foreach (var background in _backgrounds)
        {
            if (background.position.x > rightmost.position.x)
            {
                rightmost = background;
            }
        }

        bg.position = rightmost.position + Vector3.right * _spriteWidth;
    }
}
