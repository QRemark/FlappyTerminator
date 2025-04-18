using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Sprite _attackSprite;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private BulletBar _bulletBar;

    private float _attackSpriteDuration = 0.5f;

    private AttackAudio _attackAudio;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _attackAudio = GetComponent<AttackAudio>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_bulletBar != null && _bulletSpawner != null)
            _bulletBar.Initialize(_bulletSpawner);
    }

    public void Attack()
    {
        if (_bulletSpawner != null && _bulletSpawnPoint != null)
        {
            Bullet bullet = _bulletSpawner.Fire(_bulletSpawnPoint.position, true);

            if (bullet != null)
            {
                bullet.SetOwner(transform);
                PlayAttackFeedback();
            }
        }
    }

    private void PlayAttackFeedback()
    {
        if (_spriteRenderer != null && _attackSprite != null)
        {
            _spriteRenderer.sprite = _attackSprite;
            _attackAudio?.AttackSound();
            Invoke(nameof(ResetSprite), _attackSpriteDuration);
        }
    }

    private void ResetSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = _defaultSprite;
        }
    }

    public void ResetAttack()
    {
        CancelInvoke(nameof(ResetSprite));
        ResetSprite();
    }
}
