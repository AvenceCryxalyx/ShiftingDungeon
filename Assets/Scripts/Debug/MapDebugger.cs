using System.Collections.Generic;
using UnityEngine;

public class MapDebugger : MonoBehaviour
{
    [SerializeField]
    private DungeonMaster dm;

    Timer test = new Timer(60);

    private void Start()
    {
        InitializeMap();
        test.EvtTimerUp += TestTimer;
        test.Start();
    }

    public void InitializeMap()
    {
        StartCoroutine(dm.InitializeDungeon());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            dm.Map.DoRoomSwaps(5);
        }
    }

    private void TestTimer()
    {
        Debug.Log("Timer off");
        test.Reset();
        test.Start();
    }
}
