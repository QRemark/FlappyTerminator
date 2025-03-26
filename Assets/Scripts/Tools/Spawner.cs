using UnityEngine;
using System;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IDisappearable
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _poolCapacity = 2;
    //[SerializeField] protected Transform _poolParent;
    [SerializeField] protected int _poolMaxSize = 3;

    protected Pool<T> _pool;

    public int ActiveObjectsCount => _pool.ActiveCount;
    public int TotalCreatedObjects => _pool.TotalCreated;
    public int TotalSpawned { get; protected set; }

    public event Action CountersUpdated;

    protected virtual void Awake()
    {
        if (_pool == null)
        {
            _pool = new Pool<T>();
           // _pool.Initialize(_prefab, _poolCapacity, _poolMaxSize, _poolParent);
            _pool.Initialize(_prefab, _poolCapacity, _poolMaxSize);
            _pool.PoolChanged += UpdateCounters;
        }
    }

    protected T GetObjectFromPool()
    {
        T obj = _pool.GetObject();

        if (obj == null)
            return null;

        TotalSpawned++;
        UpdateCounters();
        obj.Disappeared += ReturnObjectInPool;

        return obj;
    }

    protected virtual void UpdateCounters()
    {
        CountersUpdated?.Invoke();
    }

    private void ReturnObjectInPool(IDisappearable disappearedObject)
    {
        disappearedObject.Disappeared -= ReturnObjectInPool;
        _pool.ReleaseObject((T)disappearedObject);
        UpdateCounters();
    }

    public void Spawn(Vector3 position)
    {
        T obj = GetObjectFromPool();
        if (obj != null)
        {
            obj.transform.position = position;
        }
    }
}