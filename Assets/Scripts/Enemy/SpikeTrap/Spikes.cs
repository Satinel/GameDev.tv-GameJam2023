using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] int damage = 1;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>())
        {
            other.GetComponent<PlayerHealth>().DealDamage(damage);
        }
    }
}
