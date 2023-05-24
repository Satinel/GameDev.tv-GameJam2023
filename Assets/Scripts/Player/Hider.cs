using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    public bool IsHidden { get; private set; } = false;

    int _alertedEnemies = 0;

    public void AttemptStealth()
    {
        if(_alertedEnemies <= 0)
        {
            IsHidden = true;
        }
        Debug.Log(IsHidden);
    }

    public void LeaveStealth()
    {
        IsHidden = false;
    }

    public void AdjustAlertedEnemiesCount(int count)
    {
        _alertedEnemies += count;
        Debug.Log(_alertedEnemies);
    }
}
