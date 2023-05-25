using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] int _damage;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>())
        {
            //TODO deal damage to playerhealth script which doesn't exist yet
            other.GetComponent<PlayerHealth>().DealDamage(_damage);
        }
    }


}
