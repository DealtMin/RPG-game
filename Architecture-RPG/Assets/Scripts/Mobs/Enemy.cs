using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IMobController
{
    public GameObject MagicAttackPrefab => magicAttack;
    public Animator Animator { get; private set; }
    [SerializeField] private GameObject magicAttack;
    [SerializeField] private Transform magicSpawmPoint;
    [SerializeField] bool isRange = true;
    [SerializeField] private int MaxHealth = 120;
    [SerializeField] private float attackCoolDown = 1.5f;
    [SerializeField] private float noticeDistance = 10;
    [SerializeField] private float damageInvincibility = 1.5f;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem damageParticles;
    private EnemyStateMachine _stateMachine;
    private Transform _target;
    private NavMeshAgent _agent;
    private bool _attackReady = true;
    private bool _canDamage = true;
    private EnemyHealthController _healthController;

    private IAudioService _audio;
    public void Construct(Transform transform, int gamemode)
    {
        _target = transform;
        CreateStateMachine(gamemode);
    }

    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();

        _audio = ServiceLocator.Get<IAudioService>();
        _agent = GetComponent<NavMeshAgent>();
        _healthController = new EnemyHealthController(MaxHealth);
        _healthController.DeathEvent += Death;
    }
    void CreateStateMachine(int mode)
    {
        if (mode == (int)GameMode.easy)
        {
            _stateMachine = new EnemyPassiveStateMachine(this);
            return;
        }
        _stateMachine = new EnemyStateMachine(this);
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
    }
    void OnDestroy() => _healthController.DeathEvent -= Death;
    public void Damage(int damage)
    {
        if (_canDamage)
        {
            _canDamage = false;
            StartCoroutine(DamageCountDown(damageInvincibility));
            damageParticles.Play();
            _audio.PlaySound(hitClip);
            _healthController.Damage(damage);
        }
    }
    private IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;
    }
    public void Attack()
    {
        if (isRange)
        {
            GameObject newMagicBall = Instantiate(magicAttack, magicSpawmPoint.position, Quaternion.identity);
            MagicAttackBehaivour magicBeh = newMagicBall.GetComponent<MagicAttackBehaivour>();
            magicBeh.Construct(_target, gameObject.transform);
        }
    }
    public bool IsAttackReady() => _attackReady;
    public void SetAttackCoolDown()
    {
        if (_attackReady)
        {
            _attackReady = false;
            StartCoroutine(AttackPermission(attackCoolDown));
        }
    }
    private IEnumerator AttackPermission(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _attackReady = true;
    }
    public void Flee()
    {
        Vector3 direction = (transform.position - _target.position).normalized;
        Vector3 fleePosition = transform.position + direction * noticeDistance;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(fleePosition, out hit, noticeDistance, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
    public bool IsPlayerInView()
    {
        return Vector3.Distance(_target.position, transform.position) < noticeDistance; ;
    }
    public bool IsPlayerNear()
    {
        return Vector3.Distance(_target.position, transform.position) < _agent.stoppingDistance;
    }

    public void ChasePlayer()
    {
        _agent.destination = _target.position;
    }
    public void SetChaising(bool flag)
    {
        _agent.isStopped = !flag;
    }
    private void Death()
    {
        _stateMachine.ChangeState(new EnemyDeathState(_stateMachine));
        damageParticles.Play();

    }

    public IHealthController GetHealthController()
    {
        return _healthController;
    }

}
