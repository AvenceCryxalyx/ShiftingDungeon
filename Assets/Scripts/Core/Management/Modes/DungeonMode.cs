using System.Collections;
using UnityEngine;

public class DungeonMode : GameMode
{
    [SerializeField]
    private string onExitGameMode;

    [SerializeField]
    private DungeonMaster dungeonMaster;
    private PlayerUnitController playerUnitController;
    private CameraController cameraController;

    public Timer ShiftTimer { get ; private set; }


    public static DungeonMaster Master
    {
        get
        {
            return GameMode.GetActive<DungeonMode>().dungeonMaster;
        }
    }

    protected override IEnumerator OnLoad()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        yield return StartCoroutine(dungeonMaster.InitializeDungeon());
        yield return new WaitUntil(() => dungeonMaster.Map.IsInitialized);

        SpawnRoom spawn = dungeonMaster.Map.GetAreaType(MapAreaSO.MapAreaType.Entry) as SpawnRoom;
        spawn.Spawn(playerUnitController.gameObject);
        cameraController.Initialize(playerUnitController.gameObject);
        playerUnitController.SetGravityEnable(true);

        yield return base.OnLoad();
        yield return new WaitForSeconds(2f);
        ShiftTimer.Start();
    }

    protected override IEnumerator OnUnload()
    {
        cameraController.StopFollow();
        dungeonMaster.UnloadDungeon();
        ShiftTimer.EvtTimerUp -= OnTimerUp;
        ShiftTimer = null;
        StopAllCoroutines();
        yield return base.OnUnload();
    }

    public void BeginExitProcedures()
    {
        ExitDungeon();
    }

    public void ExitDungeon()
    {
        AppInstance.LoadGameMode(onExitGameMode);
    }

    protected override void OnInitializeUnit(GameObject unit)
    {
        playerUnitController = unit.GetComponent<PlayerUnitController>();
        playerUnitController.SetGravityEnable(false);
        int random = UnityEngine.Random.Range(0, 10);
        dungeonMaster.Map.SelectAreasToSwap(random);
        ShiftTimer = new Timer(60f);
        ShiftTimer.EvtTimerUp += OnTimerUp;
    }

    private void OnTimerUp()
    {
        dungeonMaster.Map.DoRoomSwaps();
        StartCoroutine(WaitForNextShiftQueue());
    }

    private IEnumerator WaitForNextShiftQueue()
    {
        yield return new WaitForSeconds(5f);
        int random = UnityEngine.Random.Range(0, 10);
        dungeonMaster.Map.SelectAreasToSwap(random);
        ShiftTimer.Restart();
    }
}
