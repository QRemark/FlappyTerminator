using UnityEngine;

public class BulletSpawner : Spawner<Bullet>
{
    [SerializeField] private Sprite[] _bulletSprites;

    public Transform PoolParent { get; private set; }

    private void Start()
    {
        base.Start();   
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
        ClearActiveObjects();
    }

    public void SetPoolParent(Transform poolParent)
    {
        PoolParent = poolParent;
    }
}
