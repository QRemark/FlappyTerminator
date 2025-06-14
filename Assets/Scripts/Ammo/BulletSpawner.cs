using UnityEngine;

public class BulletSpawner : Spawner<Bullet>
{
    [SerializeField] private Sprite[] _bulletSprites;
    [SerializeField] private Transform _localPoolParent;

    private int _enemyBuletsCount = 2;
    private int _playerBuletsCount = 6;

    public Transform PoolParent { get; private set; }

    protected override void Start()
    {
        var newPool = new Pool<Bullet>();
        ReplacePool(newPool);

        if (PoolParent == null && _localPoolParent != null)
            PoolParent = _localPoolParent;

        if (PoolParent != null)
            Pool.SetParent(PoolParent);

        int initialSize = GetComponent<Player>() != null ? _playerBuletsCount : PoolCapacity;
        Pool.Initialize(Prefab, initialSize, PoolMaxSize);
        Pool.PoolChanged += UpdateCounters;
    }

    public void Reset()
    {
        bool isPlayer = GetComponent<Player>() != null;
        int initialSize = isPlayer ? _playerBuletsCount : _enemyBuletsCount;

        Pool.ResetPool(Prefab, initialSize, PoolMaxSize);

        ClearActiveObjects();
    }

    public Bullet Shoot(Vector3 position)
    {
        Bullet bullet = GetObjectFromPool(true);

        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.gameObject.SetActive(true);

            if (_bulletSprites != null && _bulletSprites.Length > 0)
            {
                bullet.SetSprite(_bulletSprites[Random.Range(0, 
                    _bulletSprites.Length)]);
            }
        }

        return bullet;
    }

    public void SetPoolParent(Transform poolParent)
    {
        PoolParent = poolParent;
    }
}
