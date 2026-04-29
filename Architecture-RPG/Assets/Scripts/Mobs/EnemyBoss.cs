using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour, IDamagable, IMobController
{
    public Animator Animator { get; protected set; }
    public bool wasAttaked { get; protected set; }
    [SerializeField] private GameObject[] magicAttacks;
    [SerializeField] private float secondPhaseStoppingDistance = 2f;
    [SerializeField] private int MaxHealth = 120;
    [SerializeField] private float attackCoolDown = 1.5f;
    [SerializeField] private float noticeDistance = 10;
    [SerializeField] private float damageInvincibility = 1.5f;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private Transform[] magicSpawmPoints;
    private BossStateMachine _stateMachine;
    private Transform _target;
    private NavMeshAgent _agent;
    private bool _attackReady = true;
    private bool _canDamage = true;
    private BossHealthController _healthController;
    private ISettingsLoader _settings;
    [SerializeField] private int scoreValue=20;


    private IAudioService _audio;
    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        wasAttaked = false;
        
        _audio = ServiceLocator.Get<IAudioService>();
        _settings = ServiceLocator.Get<ISettingsLoader>();
        _target = FindAnyObjectByType<PlayerLifecycle>().transform;
        _agent = GetComponent<NavMeshAgent>();

        _healthController = new BossHealthController(MaxHealth);
        _healthController.SecondPhase += SecondPhase;
        _healthController.DeathEvent += Death;

        CreateStateMachine(_settings.LoadPlayMode());
    }
    void CreateStateMachine(int mode)
    {
        if (mode == (int)GameMode.easy) 
        {
            _stateMachine =  new BossPassiveStateMachine(this);
            return;
        }
        _stateMachine =  new BossStateMachine(this);
    }
    void Update()
    {
        _stateMachine?.CurrentState.LogicUpdate();
    }
    void OnDestroy()
    {
        _healthController.SecondPhase -= SecondPhase;
        _healthController.DeathEvent -= Death;
    }
    public void Damage(int damage)
    {
        if (_canDamage)
        {
            wasAttaked = true;
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

    public void RangeAttack()
    {
        int indx = RandomBetween(0, magicAttacks.Length);
        foreach (Transform magicSpawmPoint in magicSpawmPoints)
        {
            GameObject newMagicBall = Instantiate(magicAttacks[indx],
                magicSpawmPoint.position, Quaternion.identity);
            if (newMagicBall.gameObject.TryGetComponent(out MushroomBallBehaviour mushroomBall))
            {
                mushroomBall.Construct(_target, gameObject.transform);
            }
            else
            {
                newMagicBall.GetComponent<MagicAttackBehaivour>().Construct(_target, gameObject.transform);
            }
        }
    }
    int RandomBetween(int min, int max)
    {
        return Random.Range(min, max);
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
        ServiceLocator.Get<IScoreService>().AddScore(scoreValue);
        ServiceLocator.Get<IGameEventService>().NotifyEnemyDeath();
        _stateMachine.ChangeState(new BossDeathState(_stateMachine));
        damageParticles.Play();

    }

    public IHealthController GetHealthController()
    {
        return _healthController;
    }
    
    public void RestoreHealth(int health)
    {
        _healthController.RestoreHealth(health);
    }
    private void SetFightStateMachine(BossStateMachine fightSM)
    {
        _stateMachine = fightSM;
        _stateMachine.Initialize();
    }
    public void SecondPhase()
    {
        _agent.stoppingDistance = secondPhaseStoppingDistance;
        SetFightStateMachine(new Phase2BossFightStateMachine(this));
    }
    public void Destroy() => Destroy(gameObject);
}
