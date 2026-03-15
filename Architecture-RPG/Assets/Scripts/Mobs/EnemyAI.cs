using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform target;
    [SerializeField] private float attackCoolDown = 1.5f;
    [SerializeField] private float noticeDistance = 10;
    [SerializeField] private GameObject magicAttack;
    [SerializeField] private Transform magicSpawmPoint;
    private NavMeshAgent _agent;
    private EnemyAnimationController _enemyAnimation;
    private EnemyState enemyState = EnemyState.idle;
    private bool attackReady = true;
    private Transform _playerTransform;

    public void SetParams(Transform inputPlayerTransform)
    {
        _playerTransform = inputPlayerTransform;
    }

    void Awake()
    {
        target = FindAnyObjectByType<PlayerLifecycle>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimation = GetComponent<EnemyAnimationController>();
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
            case EnemyState.escape:
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
        if (Vector3.Distance(target.position, transform.position) > noticeDistance)
        {
            enemyState = EnemyState.idle;
        }
        _enemyAnimation.Chase();
        _agent.destination = target.position;
        _agent.isStopped = false;
    }
    private void AttackPlayer()
    {
        if (Vector3.Distance(target.position, transform.position) > _agent.stoppingDistance) 
        {
            enemyState = EnemyState.chase;
        }
        else
        {
            _agent.isStopped = true;
            _enemyAnimation.Attack(attackReady);
            if (attackReady)
            {
                attackReady = false;
                StartCoroutine(AttackPermission(attackCoolDown));
                if (enemyType == EnemyType.range)
                {
                    RangeAttack();
                }
            }   
        }
    }
    private void RangeAttack()
    {
        GameObject newMagicBall = Instantiate(magicAttack, magicSpawmPoint.position, Quaternion.identity);
        MagicAttackBehaivour magicBeh = newMagicBall.GetComponent<MagicAttackBehaivour>();
        magicBeh.SetParams(_playerTransform, gameObject.transform);
    }
    
    public void Death()
    {
        enemyState = EnemyState.death;
        _enemyAnimation.DeathAnimation();
        _agent.isStopped = true;
    }

    public IEnumerator AttackPermission(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        attackReady = true;        
    }
}

public enum EnemyState
{
    idle,
    attack,
    chase,
    escape,
    death
}
public enum EnemyType
{
    melee,
    range
}
