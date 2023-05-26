using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour // All thanks to https://www.youtube.com/watch?v=j1-OyLo77ss for this one!
{
    [SerializeField] float _radius;
    [Range(0,360)]
    [SerializeField] float _angle;
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstructionMask;
    [SerializeField] EnemyAI enemyAI;

    Transform _player;
    Hider _hider;
    bool _canSeePlayer;
    Vector3 _lastSeenPosition;

    IEnumerator Start()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            Check();
        }
    }

    void Check()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if(rangeChecks.Length > 0)
        {
            _player = rangeChecks[0].transform;
            if(_hider == null)
            {
                _hider = _player.GetComponent<Hider>();
            }

            if(_hider.IsHidden) { return; }

            Vector3 directionToTarget = (_player.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _player.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    _lastSeenPosition = _player.position;
                    if(!_canSeePlayer)
                    {
                        _hider.AdjustAlertedEnemiesCount(1);
                    }
                    _canSeePlayer = true;
                    enemyAI.Aggro(_player);
                }
                else
                {
                    if(_canSeePlayer)
                    {
                        _hider.AdjustAlertedEnemiesCount(-1);
                        enemyAI.Chase(_lastSeenPosition);
                    }
                    _canSeePlayer = false;
                }
            }
            else
            {
                if(_canSeePlayer)
                {
                    _hider.AdjustAlertedEnemiesCount(-1);
                    enemyAI.Chase(_lastSeenPosition);
                }
                _canSeePlayer = false;
            }
        }
        else if(_canSeePlayer)
        {
            _canSeePlayer = false;
            _player.GetComponent<Hider>().AdjustAlertedEnemiesCount(-1);
            enemyAI.Chase(_lastSeenPosition);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -_angle /2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, _angle /2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * _radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * _radius);

        if(_canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _player.position);
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        return new Vector3(MathF.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
