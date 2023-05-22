using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] Transform[] _doors;
    [SerializeField] Material _activateMaterial;
    [SerializeField] float _moveSpeed = 10f;

    MeshRenderer _renderer;
    bool _hasActivated = false;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OpenDoors();
        }
    }

    void OpenDoors()
    {
        if(_hasActivated) { return; }

        _hasActivated = true;
        _renderer.material = _activateMaterial;
        transform.rotation = Quaternion.Euler(0f, 90f, 180f);
        //TODO Play a sound!
        foreach (Transform door in _doors)
        {
            StartCoroutine(RaiseDoor(door));
        }
    }

    IEnumerator RaiseDoor(Transform door)
    {
        //TODO Start another sound
        while(door.localPosition.y < 4f)
        {
            door.localPosition += new Vector3(0f, 0.1f, 0f) * _moveSpeed * Time.deltaTime;
            yield return null;
        }
        //TODO Stop that other sound
    }

}
