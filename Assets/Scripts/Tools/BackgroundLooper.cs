using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private Transform _player;  
    [SerializeField] private float _speedFactor = 1.9f;

    private int _backgroundCount = 3; 
    private int _initialIndex = 0;

    private float _spriteWidth;
    private float _repositionMultiplier= 1.1f;

    private Transform[] _backgrounds;

    private Vector3 _lastPlayerPosition;

    private void Start()
    {
        _backgrounds = new Transform[_backgroundCount];

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i] = transform.GetChild(i);
        }
        
        _spriteWidth = _backgrounds[_initialIndex].GetComponent<SpriteRenderer>().bounds.size.x;

        _lastPlayerPosition = _player.position;
    }

    private void Update()
    {
        float playerDeltaX = _player.position.x - _lastPlayerPosition.x;

        foreach (var picture in _backgrounds)
        {
            picture.position += Vector3.right * playerDeltaX * _speedFactor;
        }

        foreach (var picture in _backgrounds)
        {
            if (picture.position.x < _player.position.x - _spriteWidth * _repositionMultiplier)
            {
                MoveBackgroundToRight(picture);
            }
        }

        _lastPlayerPosition = _player.position;
    }

    public void ResetBackground()
    {
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i].position = new Vector3(
                _player.position.x + _spriteWidth * (i - _initialIndex),
                _backgrounds[i].position.y,
                _backgrounds[i].position.z);
        }

        _lastPlayerPosition = _player.position;
    }

    private void MoveBackgroundToRight(Transform picture)
    {
        Transform rightmost = _backgrounds[_initialIndex];

        foreach (var background in _backgrounds)
        {
            if (background.position.x > rightmost.position.x)
            {
                rightmost = background;
            }
        }

        picture.position = rightmost.position + Vector3.right * _spriteWidth;
    }
}
