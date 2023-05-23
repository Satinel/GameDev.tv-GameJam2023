using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool _IsActive = true;

    void OnTriggerEnter(Collider other)
    {
        if(!_IsActive) { return; }

        if(other.GetComponent<Mover>())
        {
            other.GetComponent<Mover>().ShowInteractable();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Mover>())
        {
            other.GetComponent<Mover>().HideInteractable();
        }
    }
}
