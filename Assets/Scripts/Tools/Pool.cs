using System.Collections.Generic;
using UnityEngine;
using System;

public class Pool<T> where T : MonoBehaviour
{
    private Queue<T> _deactiveObjects;
    private List<T> _activeObjects;
    private T _prefab;
    private Transform _parent;
    private int _maxSize;
    private int _currentCount;

    public int TotalCreated { get; private set; }
    
    public int ActiveCount => _activeObjects.Count;

    public List<T> ActiveObjects => _activeObjects;

    public event Action PoolChanged;

    private void Awake()
    {
        _deactiveObjects = new Queue<T>();
    }

  
    public void Initialize(T prefab, int initialSize, int maxSize)
    {
        _prefab = prefab;
  
        _maxSize = maxSize;
        _currentCount = 0;

        _deactiveObjects = new Queue<T>();
        _activeObjects = new List<T>();

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Create();
            obj.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(obj);
        }
    }

    public void ClearActiveObjects()
    {
        foreach (var obj in _activeObjects)
        {
            obj.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(obj); 
        }

        _activeObjects.Clear();
        PoolChanged?.Invoke();
    }

    
    public T GetPreparedObject()
    {
        if (_deactiveObjects.Count > 0)
        {
            T obj = _deactiveObjects.Dequeue();
            _activeObjects.Add(obj);
            PoolChanged?.Invoke();
            return obj;
        }

        return null;
    }

    public void ActivateObject(T obj)
    {
        if (!obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(true);
        }
    }
    

    public T GetObject()
    {
        if (_deactiveObjects.Count > 0)
        {
            T @object = _deactiveObjects.Dequeue();
            @object.gameObject.SetActive(true);
            
            _activeObjects.Add(@object);
            PoolChanged?.Invoke();

            return @object;
        }

        return null;
    }

    public void ReleaseObject(T @object)
    {
        if (!_deactiveObjects.Contains(@object))
        {
            var enemy = @object as Enemy;

            if (enemy != null)
            {
                enemy.CancelInvoke();
                enemy.transform.position = Vector3.zero;
            }

            @object.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(@object);
            _activeObjects.Remove(@object);
            PoolChanged?.Invoke();
        }
    }

    // Pool.cs
    public void ResetPool(T newPrefab, int initialSize, int maxSize)
    {
        foreach (var obj in _activeObjects) UnityEngine.Object.Destroy(obj.gameObject);
        foreach (var obj in _deactiveObjects) UnityEngine.Object.Destroy(obj.gameObject);

        _activeObjects.Clear();
        _deactiveObjects.Clear();

        Initialize(newPrefab, initialSize, maxSize);
    }

    private T Create()
    {
        if (_currentCount >= _maxSize)
            return null;

        T @object = UnityEngine.Object.Instantiate(_prefab, _parent);
        _currentCount++;

        TotalCreated++;

        return @object;
    }
}