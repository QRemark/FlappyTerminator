using UnityEngine;
using System;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IDisappearable
{
    //[SerializeField] protected T _prefab;
    //[SerializeField] protected int _poolCapacity = 2;

    //[SerializeField] protected int _poolMaxSize = 3;

    //protected Pool<T> _pool;

    [SerializeField] private T _prefab;
    [SerializeField] private int _poolCapacity = 2;

    [SerializeField] private int _poolMaxSize = 3;

    private Pool<T> _pool;

    protected T Prefab => _prefab;
    protected int PoolCapacity => _poolCapacity;
    protected int PoolMaxSize => _poolMaxSize;
    protected Pool<T> Pool => _pool;

    public int ActiveObjectsCount => _pool.ActiveCount;
    public int TotalCreatedObjects => _pool.TotalCreated;
    public int TotalSpawned { get; protected set; }

    public event Action CountersUpdated;

    protected virtual void Start()
    {
        if (_pool == null)
        {
            _pool = new Pool<T>();
    
            _pool.Initialize(_prefab, _poolCapacity, _poolMaxSize);
            _pool.PoolChanged += UpdateCounters;
        }
    }

    public void ClearActiveObjects()
    {
        _pool.ClearActiveObjects();
    }

    protected void ReplacePool(Pool<T> newPool)
    {
        _pool = newPool;
        _pool.PoolChanged += UpdateCounters;
    }

    protected T GetObjectFromPool1(bool activate)
    {
        T obj = _pool.GetObject1(activate);

        if (obj == null)
            return null;

        TotalSpawned++;
        UpdateCounters();
        obj.Disappeared += ReturnObjectInPool;

        return obj;
    }
    //
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
    
    protected T GetPreparedObjectFromPool()
    {
        T obj = _pool.GetPreparedObject();

        if (obj == null)
            return null;

        TotalSpawned++;
        UpdateCounters();
        obj.Disappeared += ReturnObjectInPool;

        return obj;
    }
    //
    protected void ActivateObject(T obj)
    {
        _pool.ActivateObject(obj);
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
}