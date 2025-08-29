using System.Linq;
using UnityEngine;

public class SpawnRoom : MapArea
{
    [SerializeField]
    protected Transform[] Spawnpoints;

    public void Spawn(GameObject spawn, bool specificPoint = false, int index = 0)
    {
        if (!specificPoint)
        {
            spawn.transform.position = Spawnpoints[UnityEngine.Random.Range(0, Spawnpoints.Length)].transform.position;
        }
        else
        {
            spawn.transform.position = Spawnpoints[index].transform.position; ;
        }
    }
}
