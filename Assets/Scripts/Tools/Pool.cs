using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Linq;

public class Pool<T> where T : MonoBehaviour
{
    private Queue<T> _deactiveObjects;
    private T _prefab;
    private Transform _parent;
    private int _maxSize;
    private int _currentCount;

    public int TotalCreated { get; private set; }
    public int ActiveCount { get; private set; }

    public event Action PoolChanged;

    private void Awake()
    {
        _deactiveObjects = new Queue<T>();
    }

    //public void Initialize(T prefab, int initialSize, int maxSize, Transform parent = null)
    public void Initialize(T prefab, int initialSize, int maxSize)
    {
        _prefab = prefab;
        //_parent = parent;
        _maxSize = maxSize;
        _currentCount = 0;

        _deactiveObjects = new Queue<T>();

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Create();
            obj.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(obj);
        }
    }

    public void Resize(int newSize)
    {
        if (newSize > TotalCreated)
        {
            for (int i = TotalCreated; i < newSize; i++)
            {
                T newObject = UnityEngine.Object.Instantiate(_prefab);
                newObject.gameObject.SetActive(false);
                _deactiveObjects.Enqueue(newObject); 
            }
            TotalCreated = newSize;
        }
    }



    public T GetObject()
    {
        if (_deactiveObjects.Count > 0)
        {
            T @object = _deactiveObjects.Dequeue();
            @object.gameObject.SetActive(true);
            ActiveCount++;
            PoolChanged?.Invoke();

            return @object;
        }

        return null;
    }

    public void ReleaseObject(T @object)
    {
        if (!_deactiveObjects.Contains(@object))
        {
            @object.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(@object);
            ActiveCount--;
            PoolChanged?.Invoke();
        }
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