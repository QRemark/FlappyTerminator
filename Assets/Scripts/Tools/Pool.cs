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

    public Pool()
    {
        _deactiveObjects = new Queue<T>();
        _activeObjects = new List<T>();
    }

    public event Action PoolChanged;

    public int TotalCreated { get; private set; }
    public int ActiveCount => _activeObjects.Count;

    public void Initialize(T prefab, int initialSize, int maxSize)
    {
        _prefab = prefab;
        _maxSize = maxSize;
        _currentCount = 0;

        TotalCreated = 0;

        _deactiveObjects.Clear();
        _activeObjects.Clear();

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Create();

            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                _deactiveObjects.Enqueue(obj);
            }
        }
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
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

    public T GetObject(bool activate)
    {
        if (_deactiveObjects.Count > 0)
        {
            T obj = _deactiveObjects.Dequeue();

            if (activate == true)
                obj.gameObject.SetActive(true);

            _activeObjects.Add(obj);
            PoolChanged?.Invoke();

            return obj;
        }

        return null;
    }
    
    public void ActivateObject(T obj)
    {
        if (obj.gameObject.activeSelf == false)
        {
            obj.gameObject.SetActive(true);
        }
    }

    public void ReleaseObject(T obj)
    {
        if (_deactiveObjects.Contains(obj) == false)
        {
            if (obj is Enemy enemy)
            {
                enemy.CancelInvoke();
                enemy.transform.position = Vector3.zero;
            }

            obj.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(obj);
            _activeObjects.Remove(obj);
            PoolChanged?.Invoke();
        }
    }

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

        T obj = UnityEngine.Object.Instantiate(_prefab, _parent);
        _currentCount++;
        TotalCreated++;

        return obj;
    }
}
