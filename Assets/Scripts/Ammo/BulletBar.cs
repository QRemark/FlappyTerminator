using UnityEngine;
using UnityEngine.UI;

public class BulletBar : MonoBehaviour
{
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private Slider _slider;

    public void Initialize(BulletSpawner spawner)
    {
        _bulletSpawner = spawner;

        _slider.maxValue = _bulletSpawner.TotalCreatedObjects;
        _slider.value = _bulletSpawner.TotalCreatedObjects - _bulletSpawner.ActiveObjectsCount;
    }

    private void OnEnable()
    {
        _bulletSpawner.CountersUpdated += UpdateBulletBar;
    }

    private void OnDisable()
    {
        _bulletSpawner.CountersUpdated -= UpdateBulletBar;
    }

    private void UpdateBulletBar()
    {

        _slider.maxValue = _bulletSpawner.TotalCreatedObjects;
        _slider.value = _bulletSpawner.TotalCreatedObjects - _bulletSpawner.ActiveObjectsCount;
    }
}
