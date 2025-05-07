using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _attackSprite;
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private float _attackInterval = 3f;
    [SerializeField] private float _attackDistance = 15f;
    [SerializeField] private float _spriteResetDelay = 0.5f;

    private Transform _player;
    private Transform _bulletPoolParent;
    private SpriteRenderer _spriteRenderer;
    private AttackAudio _enemyAudio;

    private bool _canAttack;

    public void Setup(Transform player, Transform bulletPoolParent)
    {
        _player = player;
        _bulletPoolParent = bulletPoolParent;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAudio = GetComponent<AttackAudio>();

        _bulletSpawner.SetPoolParent(_bulletPoolParent);
    }

    public void EnableAttack()
    {
        _canAttack = true;
        InvokeRepeating(nameof(FireBullet), _attackInterval, _attackInterval);
    }

    public void DisableAttack()
    {
        _canAttack = false;
        CancelInvoke(nameof(FireBullet));
    }

    public void ResetState()
    {
        if (_spriteRenderer != null)//не убирается
            _spriteRenderer.sprite = _defaultSprite;
    }

    private void FireBullet()
    {
        if (_canAttack == false)
            return;

        float distanceToPlayer = transform.position.x - _player.position.x;

        if (distanceToPlayer < _attackDistance)
        {
            PlayAttackVisual();

            Bullet bullet = _bulletSpawner.Fire(transform.position, false);

            bullet.SetOwner(transform);
        }
    }

    private void PlayAttackVisual()
    {
        _spriteRenderer.sprite = _attackSprite;
        _enemyAudio?.AttackSound();

        Invoke(nameof(ResetSprite), _spriteResetDelay);
    }

    private void ResetSprite()
    {
        _spriteRenderer.sprite = _defaultSprite;
    }
}
