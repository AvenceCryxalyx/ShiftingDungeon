using System;
using UnityEngine;

public class CoordinateReceiver : MonoBehaviour
{
    public Tuple<int,int> CurrentCoordinates { get; private set; }
    public Tuple<int,int> TargetCoordinates { get; private set; }

    public void SetTarget(Tuple<int,int> target)
    {
        TargetCoordinates = target;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MapArea area = collision.gameObject.GetComponent<MapArea>();
        if(collision != null && area != null)
        {
            CurrentCoordinates = area.Coordinates;
        }
    }
}
