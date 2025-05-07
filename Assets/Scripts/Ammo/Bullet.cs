using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IDisappearable
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private LayerMask _targetLayer;

    private SpriteRenderer _spriteRenderer;

    private float _localScaleModificator = 0.2f;
    private float _lifeTimer;

    private Transform _owner;

    public event Action<IDisappearable> Disappeared;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _lifeTimer = _lifeTime;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0)
        {
            Disappear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _owner)
            return;

        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                player.TriggerGameOver();
            }
            else if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.Disappear();
            }

            Disappear();
        }
    }

    public void Disappear()
    {
        Disappeared?.Invoke(this);
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
        transform.localScale = new Vector3(_localScaleModificator,
            _localScaleModificator, 1f);
    }

    public void SetOwner(Transform owner)
    {
        _owner = owner;
    }
}