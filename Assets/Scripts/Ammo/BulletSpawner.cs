using UnityEngine;

public class BulletSpawner : Spawner<Bullet>
{
    [SerializeField] private Sprite[] _bulletSprites;

    public Transform PoolParent { get; private set; }

    protected override void Start()
    {
        base.Start();
        if (PoolParent != null)
        {
            //_pool.SetParent(PoolParent);
        }
    }

    public Bullet Fire(Vector3 position, bool isPlayerBullet)
    {
        Bullet bullet = GetObjectFromPool();

        if (bullet != null)
        {
            bullet.Initialize(isPlayerBullet);
            bullet.transform.position = position;
            bullet.gameObject.SetActive(true);

            if (_bulletSprites != null && _bulletSprites.Length > 0)
            {
                bullet.SetSprite(_bulletSprites[UnityEngine.Random.Range(0, _bulletSprites.Length)]);
            }
        }

        return bullet;
    }

    public void Reset()
    {
        bool isPlayer = GetComponent<Player>() != null;
        int initialSize = isPlayer ? 10 : 2;

       // _pool.SetParent(PoolParent);
        _pool.ResetPool(_prefab, initialSize, _poolMaxSize);

        ClearActiveObjects();
    }

    public void SetPoolParent(Transform poolParent)
    {
        PoolParent = poolParent;
        //_pool.SetParent(poolParent);
    }
}
