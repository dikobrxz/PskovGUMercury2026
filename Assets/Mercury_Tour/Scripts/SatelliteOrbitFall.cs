using UnityEngine;

public class SatelliteOrbitFall : MonoBehaviour
{
    [Header("Цель")]
    [SerializeField] private Transform _planetTarget; // Объект планеты

    [Header("Настройки вращения")]
    [SerializeField] private float _rotationSpeed = 50f; // Скорость вращения

    [Header("Настройки падения")]
    [SerializeField] private float _approachSpeed = 1f; // Скорость приближения к планете
    [SerializeField] private float _scaleDecreaseSpeed = 0.2f; // Скорость уменьшения размера в секунду

    private float _currentDistance;
    private Vector3 _initialScale;
    private bool _isDestroyed = false;

    void Start()
    {
        if (_planetTarget == null)
        {
            Debug.LogError($"На объекте {gameObject.name} не задана планета (_planetTarget)!");
            enabled = false;
            return;
        }

        _currentDistance = Vector3.Distance(transform.position, _planetTarget.position);
        _initialScale = transform.localScale;
    }

    void Update()
    {
        if (_isDestroyed || _planetTarget == null) return;

        _currentDistance -= _approachSpeed * Time.deltaTime;

        transform.localScale -= Vector3.one * _scaleDecreaseSpeed * Time.deltaTime;

        if (_currentDistance <= 0.1f || transform.localScale.x <= 0.01f)
        {
            CollapseAndDestroy();
            return;
        }

        float angle = _rotationSpeed * Time.deltaTime;
        
        Vector3 direction = (transform.position - _planetTarget.position).normalized;
        
        Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * direction;

        transform.position = _planetTarget.position + (rotatedDirection * _currentDistance);

        transform.LookAt(_planetTarget);
    }

    private void CollapseAndDestroy()
    {
        _isDestroyed = true;
        transform.localScale = Vector3.zero;
        
        Debug.Log($"Спутник {gameObject.name} полностью уничтожен при падении!");
        
        gameObject.SetActive(false);
    }
}