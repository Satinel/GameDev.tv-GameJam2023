using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    // [SerializeField] MeshRenderer _lava; // It was an idea but no time to figure this out
    // [SerializeField] float _directionChangeDelay = 10;
    // [SerializeField] float _positiveChangeAmount = 0.5f;
    // [SerializeField] float _negativeChangeAmount = 1.5f;
    // float _changeAmount;
    // float _timeCheck = 0;
    // Material _lavaMaterial;
    // bool _isIncreasing;
    // Color _startingColor;
    // Color _currentColor;

    // void Awake()
    // {
    //     _lavaMaterial = _lava.material;
    //     _startingColor = _lavaMaterial.color;
    //     _changeAmount = _positiveChangeAmount;
    //     _currentColor = _startingColor;
    // }

    // void Update()
    // {
    //     if(_timeCheck < _directionChangeDelay)
    //     {
    //         _currentColor *= _changeAmount;
    //         _lavaMaterial.SetColor("_EmissionColor", _currentColor);
    //         _timeCheck += Time.deltaTime;
    //         Debug.Log("I'm working, hoeset.");
    //     }
    //     else
    //     {
    //         if(_isIncreasing)
    //         {
    //             _isIncreasing = false;
    //             _changeAmount = _negativeChangeAmount;
    //             _timeCheck = 0;
    //         }
    //         else
    //         {
    //             _isIncreasing = true;
    //             _changeAmount = _positiveChangeAmount;
    //             _timeCheck = 0;
    //         }
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>())
        {
            other.GetComponent<PlayerHealth>().DealDamage(42);
            GetComponent<Collider>().enabled = false;
        }
    }

}
