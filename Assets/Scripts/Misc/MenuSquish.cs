using System.Collections;
using UnityEngine;

public class MenuSquish : MonoBehaviour
{

    [SerializeField] float _squishInterval = 5f;
    [SerializeField] float _squishAmount = 0.01f;
    [SerializeField] float _squishRoutineAmount = 0.1f;
    [SerializeField] float _coroutineDuration = 0.8f;
    
    bool _isSquished = false;
    bool _isSquishing = false;
    bool _inTightSpace = false;
    bool _isHiding = false;
    bool _canSquish = true;
    float _routineDelta = 0f;
    float _count = 0;


    void Update()
    {
        _count += Time.deltaTime;
        if(_count >= _squishInterval)
        {
            SideSquish();
            _count = 0;
        }
    }

    void SideSquish()
    {
        if(_isSquishing || _isHiding || !_canSquish || Time.timeScale == 0) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquishing = true;
            _routineDelta = 0f;
            StartCoroutine(SideRoutine());
        }
    }

    IEnumerator SideRoutine()
    {
        while(_routineDelta < _coroutineDuration)
        {
            transform.localScale = new Vector3(transform.localScale.x - _squishRoutineAmount, 1f, 1f);
            _routineDelta += 0.05f;
            yield return null;
        }
        _isSquishing = false;
        _isSquished = true;
        transform.localScale = new Vector3(_squishAmount, 1f, 1f);
    }

    void VerticalSquish()
    {
        if(_isSquishing || _isHiding || !_canSquish || Time.timeScale == 0) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquishing = true;
            _routineDelta = 0f;
            StartCoroutine(HeightRoutine());
        }
    }

    IEnumerator HeightRoutine()
    {

        int _routineDeltaInt = 0;

        while(_routineDeltaInt < 10) //This works! It's badly written but I'm on a time limit here
        {
            transform.localScale = new Vector3(1f, transform.localScale.y - (10 * _squishAmount), 1f);
            _routineDeltaInt++;
            yield return new WaitForSeconds(0.01f);
        }

        _isSquishing = false;
        _isSquished = true;
        transform.localScale = new Vector3(1f, _squishAmount, 1f);
    }

    void FrontSquish()
    {
        if(_isSquishing || _isHiding || !_canSquish || Time.timeScale == 0) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquished = true;
            transform.localScale = new Vector3(1f, 1f, _squishAmount);
        }
    }

    void Unsquish()
    {
        if(Time.timeScale == 0) { return; }

        transform.localScale = Vector3.one;
        _isSquished = false;
    }
}
