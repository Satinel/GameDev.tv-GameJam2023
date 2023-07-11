using UnityEngine;
using Cinemachine;

public class CameraColliderHandler : MonoBehaviour
{
    [SerializeField] CinemachineCollider _cineCollider;

    const string _verTightSpace = "VerticalTightSpace";

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(_verTightSpace))
        {
            _cineCollider.enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(_verTightSpace))
        {
            _cineCollider.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(_verTightSpace))
        {
            _cineCollider.enabled = true;
        }
    }
}
