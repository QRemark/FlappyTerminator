using UnityEngine;

public class BulletSpawner : Spawner<Bullet>
{
    [SerializeField] private Sprite[] _bulletSprites;

    public Transform PoolParent{ get; private set;  }

    public void Fire(Vector3 position)
    {
        Bullet bullet = GetObjectFromPool();

        if (bullet != null)
        {
            bullet.transform.position = position;

            if (_bulletSprites != null && _bulletSprites.Length > 0)
            {
                
                bullet.SetSprite(_bulletSprites[UnityEngine.Random.Range(0, _bulletSprites.Length)]);
            }
        }
    }

    public void SetPoolParent(Transform poolParent)
    {
        PoolParent = poolParent;
    }
}