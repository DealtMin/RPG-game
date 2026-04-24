using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour
{
    public Animator Animator { get; protected set; }
    public int MaxHealth { get; private set; } = 100;
    public int Health { get; private set; }
    [SerializeField] private float attackCoolDown = 1.5f;
    [SerializeField] private float noticeDistance = 10;
    private BossStateMachine? _stateMachine;
    private Transform target;
    private NavMeshAgent _agent;
    private bool attackReady = true;
    void Awake()
    {
        target = FindAnyObjectByType<PlayerLifecycle>().transform;
        Animator = GetComponent<Animator>();
        _stateMachine = new BossStateMachine(this);
        Health = MaxHealth;
    }
    public void SetFightStateMachine(BossStateMachine fightSM)
    {
        _stateMachine = fightSM;
        _stateMachine.Initialize();
    }

    public void TakeHit(int damage)
    {
        Health -= damage;
        Debug.Log($"Босс получил {damage} урона. HP: {Health}/{MaxHealth}");
        // При падении HP ниже 50% переходим во вторую стадию
        /*if (Health <= MaxHealth / 2 && _stateMachine is not Phase2BossFightStateMachine)
        {
            Console.WriteLine("\n=== БОСС ПЕРЕХОДИТ ВО ВТОРУЮ СТАДИЮ ===\n");
            SetFightStateMachine(new Phase2BossFightStateMachine(this));
        }*/
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
        _stateMachine?.CurrentState.AnimationUpdate();
    }
    void FixedUpdate()
    {
        _stateMachine?.CurrentState.PhysicsUpdate();
    }
    public void Say(string message) =>
    Debug.Log($"Босс: \"{message}\"");
    public bool IsPlayerInView()
    {
        return Vector3.Distance(target.position, transform.position) < noticeDistance;;
    }
    public bool IsPlayerNear()
    {
        return Vector3.Distance(target.position, transform.position) > _agent.stoppingDistance;
    }
    public bool IsAttackReady()
    {
        if (attackReady)
        {
            attackReady = false;
            StartCoroutine(AttackPermission(attackCoolDown));
            return !attackReady;
        }
        return attackReady;
    }
    public IEnumerator AttackPermission(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        attackReady = true;        
    }
}
