using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [field:SerializeField] public Transform[] WayPoints { get; private set; }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < WayPoints.Length; i++)
        {
            Gizmos.DrawSphere(WayPoints[i].position, 0.5f);
            if(i < WayPoints.Length - 1)
            {
                Gizmos.DrawLine(WayPoints[i].position, WayPoints[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(WayPoints[i].position, WayPoints[0].position);
            }
        }
    }
}
