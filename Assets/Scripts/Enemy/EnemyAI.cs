using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float _attackRange = 1f;
    [SerializeField] float _attackDelay = 2f;
    [SerializeField] float _chaseDuration = 15f;
    [SerializeField] float _searchDuration = 5f;
    [SerializeField] bool _patrols;
    // [SerializeField] Animator _textAnimator; //TODO set this up

    // readonly int AGGRO_HASH = Animator.StringToHash("Aggro");
    // readonly int CONFUSED_HASH = Animator.StringToHash("Confused");

    NavMeshAgent _navAgent;
    Animator _animator;
    [SerializeField] State _currentState;
    Vector3 _startPosition;
    Quaternion _startRotation;
    [SerializeField] Vector3 _lastSeenPosition;
    [SerializeField] Transform _attackTarget = null;
    float _chaseCooldown = 0f;
    float _aggroCooldown = 0f;
    bool _canAttack = true;

    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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
            _navAgent.enabled = false;
            transform.rotation = _startRotation;
            _navAgent.enabled = true;
        }
    }

    void Patrolling()
    {
        throw new NotImplementedException();
    }

    void Attacking()
    {
        if(!_canAttack) { return; }

        if(_attackTarget == null)
        {
            ResumeDefaultState();
            return;
        }

        if (Vector3.Distance(transform.position, _attackTarget.position) > _attackRange)
        {
            _navAgent.destination = _attackTarget.position;
        }
        else
        {
            _canAttack = false;
            //TODO Play attack animation here
            Debug.Log("I'm attacking the target!");
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
        _chaseCooldown += Time.deltaTime;

        if(_currentState == State.Chasing && (_chaseCooldown > _chaseDuration))
        {
            // textAnimator.SetTrigger(CONFUSED_HASH);
            //TODO play searching SFX?
            _currentState = State.Searching;
        }

    }

    void Searching()
    {
        _navAgent.destination = transform.position;
        _aggroCooldown += Time.deltaTime;
        //TODO play some sort of searching animation if possible and/or have the enemy look around somehow (rotating if nothing else)
        
        if(_aggroCooldown > _searchDuration)
        {
            ResumeDefaultState();
        }
    }

    void ResumeDefaultState()
    {
        if (_patrols)
        {
            _currentState = State.Patrolling;
        }
        else
        {
            _currentState = State.Guarding;
        }
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
            //textAnimator.SetTrigger(AGGRO_HASH);
            //TODO play aggro SFX
        }
        _aggroCooldown = 0f;
        _attackTarget = target;
        _lastSeenPosition = _attackTarget.position;
        _currentState = State.Attacking;
    }

    public void Chase(Vector3 lastSeen)
    {
        if(_currentState != State.Chasing && _currentState != State.Attacking)
        {
            //textAnimator.SetTrigger(AGGRO_HASH);
            //TODO play aggro SFX
        }
        _aggroCooldown = 0f;
        _chaseCooldown = 0f;
        _lastSeenPosition = lastSeen;
        _currentState = State.Chasing;
    }
}
