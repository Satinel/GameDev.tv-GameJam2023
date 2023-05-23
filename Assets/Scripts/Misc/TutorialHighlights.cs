using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHighlights : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
