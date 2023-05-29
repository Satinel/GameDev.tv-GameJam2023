using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float _attackRange = 1f;
    [SerializeField] float _attackDelay = 2f;
    [SerializeField] float _chaseDuration = 15f;
    [SerializeField] float _searchDuration = 5f;
    [SerializeField] PatrolRoute _patrolRoute;
    [Min(1.1f)] [SerializeField] float _waypointRange;
    [SerializeField] Animator _characterAnimator;
    [SerializeField] Animator _textAnimator;
    [SerializeField] AudioClip _alertClip;
    [SerializeField] AudioClip _confuseClip;
    [SerializeField] AudioClip _angryClip;
    [SerializeField] AudioClip _swordSwingClip;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioSource _alertSource;

    readonly int BLEND_HASH = Animator.StringToHash("MoveBlend");
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    readonly int CONFUSED_HASH = Animator.StringToHash("Confused");
    readonly int AGGRO_HASH = Animator.StringToHash("Aggro");

    int _currentWaypoint = 0;
    NavMeshAgent _navAgent;
    [SerializeField] State _currentState;
    Vector3 _startPosition;
    Quaternion _startRotation;
    Vector3 _lastSeenPosition;
    Transform _attackTarget = null;
    float _chaseCooldown = 0f;
    float _aggroCooldown = 0f;
    bool _canAttack = true;

    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _characterAnimator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    void Start()
    {
        ResumeDefaultState();
    }

    void Update()
    {
        StateActivity();
        SetMoveAnimation();
    }

    void StateActivity()
    {
        
        switch (_currentState)
        {
            case State.Guarding:
                Guarding();
                break;
            case State.Patrolling:
                Patrolling();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Chasing:
                Chasing();
                break;
            case State.Searching:
                Searching();
                break;
            default:
                break;
        }
    }

    void Guarding()
    {
        if(transform.position != _startPosition)
        {
            _navAgent.destination = _startPosition;
        }
        else if(transform.rotation != _startRotation)
        {
            transform.rotation = _startRotation;
        }
    }

    void Patrolling()
    {
        _navAgent.destination = _patrolRoute.WayPoints[_currentWaypoint].position;

        if(Vector3.Distance(transform.position, _navAgent.destination) < _waypointRange)
        {
            if(_currentWaypoint + 1 < _patrolRoute.WayPoints.Length)
            {
                _currentWaypoint++;
            }
            else
            {
                _currentWaypoint = 0;
            }
        }
    }

    void Attacking()
    {
        if(_attackTarget == null)
        {
            ResumeDefaultState();
            return;
        }

        if (Vector3.Distance(transform.position, _attackTarget.position) > _attackRange || !_canAttack)
        {
            _navAgent.destination = _attackTarget.position;
        }
        else
        {
            _navAgent.destination = transform.position;
            _canAttack = false;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_swordSwingClip);
            _characterAnimator.SetTrigger(ATTACK_HASH);
            StartCoroutine(AttackCooldownRoutine());
        }

    }

    void Chasing()
    {
        if(_lastSeenPosition == Vector3.zero)
        {
            _currentState = State.Searching;
            return;
        }

        _navAgent.destination = _lastSeenPosition;

        if(Vector3.Distance(transform.position, _lastSeenPosition) < _attackRange)
        {
            if(_attackTarget != null && Vector3.Distance(transform.position, _attackTarget.position) < _attackRange)
            {
                Aggro(_attackTarget);
                return;
            }
        }
        
        _chaseCooldown += Time.deltaTime;

        if(_currentState == State.Chasing && (_chaseCooldown > _chaseDuration))
        {
            _navAgent.destination = transform.position;
            _textAnimator.SetTrigger(CONFUSED_HASH);
            _characterAnimator.SetTrigger(CONFUSED_HASH);
            _audioSource.Stop();
            _audioSource.PlayOneShot(_angryClip);
            _currentState = State.Searching;
        }

    }

    void Searching()
    {
        _aggroCooldown += Time.deltaTime;
        
        if(_aggroCooldown > _searchDuration)
        {
            ResumeDefaultState();
        }
    }

    void SetMoveAnimation()
    {
        Vector3 velocity = _navAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        _characterAnimator.SetFloat(BLEND_HASH, speed);
    }

    void ResumeDefaultState()
    {
        if (_patrolRoute.WayPoints.Length > 1)
        {
            _currentState = State.Patrolling;
        }
        else
        {
            _currentState = State.Guarding;
        }
        
        _navAgent.speed = walkSpeed;
    }

    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(_attackDelay);
        _canAttack = true;
    }

    public void Aggro(Transform target)
    {
        if(_currentState != State.Attacking && _currentState != State.Chasing)
        {
            _audioSource.Stop();
            _textAnimator.SetTrigger(AGGRO_HASH);
            _alertSource.PlayOneShot(_alertClip);
        }
        _aggroCooldown = 0f;
        _attackTarget = target;
        _lastSeenPosition = _attackTarget.position;
        _navAgent.speed = runSpeed;
        _currentState = State.Attacking;
    }

    public void Chase(Vector3 lastSeen)
    {
        _textAnimator.SetTrigger(CONFUSED_HASH);
        _audioSource.Stop();
        _audioSource.PlayOneShot(_confuseClip);

        _aggroCooldown = 0f;
        _chaseCooldown = 0f;
        _lastSeenPosition = lastSeen;
        _navAgent.speed = runSpeed;
        _currentState = State.Chasing;
    }
}
