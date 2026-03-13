using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float attackDelay;
    [SerializeField] private float noticeDistance = 10;
    private NavMeshAgent _agent;
    private EnemyAnimationController _enemyAnimation;
    private EnemyState enemyState = EnemyState.idle;
    private float attackTimer;
    private float lastAttackTime;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimation = GetComponent<EnemyAnimationController>();
        Debug.Log(_agent.stoppingDistance);
    }
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.chase:
                ChasePlayer();
                break;
            case EnemyState.attack:
                AttackPlayer();
                break;
        }

    }
    private void Idle()
    {
        _enemyAnimation.Idle();
        _agent.isStopped = true;
        if (Vector3.Distance(target.position, transform.position) < noticeDistance)
        {
            enemyState = EnemyState.chase;
        }
    }
    private void ChasePlayer()
    {

        if (_agent.remainingDistance <= _agent.stoppingDistance & !_agent.isStopped) 
        {
            enemyState = EnemyState.attack;
        }
        _enemyAnimation.Chase();
        _agent.destination = target.position;
        _agent.isStopped = false;
    }
    private void AttackPlayer()
    { 
        _agent.isStopped = true;
        _enemyAnimation.Attack();      
        //enemyState = EnemyState.chase;
    }
}

public enum EnemyState
{
    idle,
    attack,
    chase
}
