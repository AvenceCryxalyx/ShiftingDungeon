using System.Collections;
using UnityEngine;

public class DungeonMode : GameMode
{
    [SerializeField]
    private string onExitGameMode;

    private DungeonMaster dm;
    private PlayerUnitController playerUnitController;
    private CameraController cameraController;

    public Timer ShiftTimer { get ; private set; }

    protected override IEnumerator OnLoad()
    {
        dm = DungeonMaster.instance;
        cameraController = Camera.main.GetComponent<CameraController>();
        yield return StartCoroutine(dm.InitializeDungeon());
        yield return new WaitUntil(() => dm.Map.IsInitialized);
        SpawnRoom spawn = dm.Map.GetAreaType(MapAreaSO.MapAreaType.Entry) as SpawnRoom;
        spawn.Spawn(playerUnitController.gameObject);
        cameraController.Initialize(playerUnitController.gameObject);
        playerUnitController.SetGravityEnable(true);
        yield return base.OnLoad();
        yield return new WaitForSeconds(2f);
        ShiftTimer.Start();
    }

    protected override IEnumerator OnUnload()
    {
        dm.UnloadDungeon();
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
        ShiftTimer = new Timer(60f);
        ShiftTimer.EvtTimerUp += OnTimerUp;
    }

    private void OnTimerUp()
    {
        int random = UnityEngine.Random.Range(0, 10);
        DungeonMaster.instance.Map.DoRoomSwaps(random);
        StartCoroutine(WaitForNextShiftQueue());
    }

    private IEnumerator WaitForNextShiftQueue()
    {
        yield return new WaitForSeconds(5f);
        ShiftTimer.Restart();
    }
}
