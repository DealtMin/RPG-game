using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        agent.destination = target.position;
    }
}
