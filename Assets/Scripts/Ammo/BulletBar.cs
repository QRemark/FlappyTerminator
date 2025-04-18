using UnityEngine;
using UnityEngine.UI;

public class BulletBar : MonoBehaviour
{
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private Slider _slider;

    public void Initialize(BulletSpawner spawner)
    {
        if (_bulletSpawner != null)
            _bulletSpawner.CountersUpdated -= UpdateBulletBar;

        _bulletSpawner = spawner;

        if (_bulletSpawner != null && _slider != null)
        {
            _slider.maxValue = _bulletSpawner.TotalCreatedObjects;
            _slider.value = _bulletSpawner.TotalCreatedObjects - _bulletSpawner.ActiveObjectsCount;

            _bulletSpawner.CountersUpdated += UpdateBulletBar;
        }
    }

    private void UpdateBulletBar()
    {
        if (_slider == null || _bulletSpawner == null)
            return;

        _slider.maxValue = _bulletSpawner.TotalCreatedObjects;
        _slider.value = _bulletSpawner.TotalCreatedObjects - _bulletSpawner.ActiveObjectsCount;
    }

    private void OnDestroy()
    {
        if (_bulletSpawner != null)
            _bulletSpawner.CountersUpdated -= UpdateBulletBar;
    }
}
