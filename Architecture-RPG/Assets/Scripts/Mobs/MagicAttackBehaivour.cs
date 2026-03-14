using UnityEngine;

public class MagicAttackBehaivour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1f;
    private Rigidbody _rb;
    private Vector3 _direction;
    void Awake()
    {
        player = FindAnyObjectByType<PlayerLifecycle>().gameObject;
        _rb = GetComponent<Rigidbody>();
        _direction = Vector3.Normalize(player.transform.position - transform.position);
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
