using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IDisappearable
{
    public event Action<IDisappearable> Disappeared;

    [SerializeField] private float _speed = 5f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Disappear();
    }

    public void Disappear()
    {
        Disappeared?.Invoke(this);
    }

    public void SetSprite(Sprite sprite)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = sprite;
            transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        }
    }
}