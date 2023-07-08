using UnityEngine;

public class Hider : MonoBehaviour
{
    public bool IsHidden { get; private set; } = false;

    // int _alertedEnemies = 0;

    public void AttemptStealth()
    {
        // if(_alertedEnemies <= 0)
        // {
            IsHidden = true;
        // }
    }

    public void LeaveStealth()
    {
        IsHidden = false;
    }

    // public void AdjustAlertedEnemiesCount(int count)
    // {
    //     _alertedEnemies += count;
    // }
}
