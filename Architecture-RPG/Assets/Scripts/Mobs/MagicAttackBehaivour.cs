using UnityEngine;

public class MagicAttackBehaivour : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private float speed = 1f;
    private Rigidbody _rb;
    private Vector3 _direction;

    public void SetParams(Transform inputPlayerTransform)
    {
        _playerTransform = inputPlayerTransform;
        Preapre();

    }
    
    void Preapre()
    {
        _rb = GetComponent<Rigidbody>();
        _direction = Vector3.Normalize(_playerTransform.position - transform.position);
        transform.forward = _direction;
    }
    
    void FixedUpdate()
    {
        _rb.AddForce(_direction * speed, ForceMode.Force);
    }
    
    void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
