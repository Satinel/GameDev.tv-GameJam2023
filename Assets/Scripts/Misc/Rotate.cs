using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float _anglesPerSecond = 10f;
    [SerializeField] bool _isNegative;
    
    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;
        if(_isNegative)
        {
            rotation.y -= _anglesPerSecond * Time.deltaTime;
        }
        else
        {
            rotation.y += _anglesPerSecond * Time.deltaTime;
        }
        transform.localEulerAngles = rotation;
    }
}
