using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] int _healthValue = 1;
    [SerializeField] float _anglesPerSecond = 90f;
    bool _isCollected = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() && !_isCollected)
        {
            _isCollected = true;
            other.GetComponent<PlayerHealth>().PickupHeart(_healthValue);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.y -= _anglesPerSecond * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }
}
