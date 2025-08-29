using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameMode : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUnitPrefab;

    [SerializeField] private string[] additiveScenes = { };

    public string Id { get { return gameObject.scene.name; } }

    public GameObject Unit { get; private set; }

    public IDictionary<string, object> Data { get; set; }

    #region Virtual Methods
    protected virtual IEnumerator OnLoad() { yield break; }

    protected virtual IEnumerator OnUnload() { yield break; }

    protected virtual void OnInitializeUnit(GameObject unit) { }

    #endregion

    #region Static Methods
    public static GameMode Active { get { return AppInstance.ActiveGameMode; } }

    public static T GetActive<T>()
        where T : GameMode
    {
        return AppInstance.GetActiveGameMode<T>();
    }

    public static bool IsGameMode<T>()
        where T : GameMode
    {
        return AppInstance.ActiveGameMode is T;
    }
    #endregion

    private void Start()
    {
        StartCoroutine(LoadTask());
    }

    IEnumerator LoadTask()
    {
        SceneManager.SetActiveScene(gameObject.scene);

        GameObject unit = null;
        if (PlayerUnitPrefab)
        {
            unit = Instantiate(PlayerUnitPrefab);
            unit.name = PlayerUnitPrefab.name;
        }
        else
        {
            unit = new GameObject();
            unit.name = "UnitDefault";
        }
        Unit = unit;

        OnInitializeUnit(Unit);

        foreach (string sceneName in additiveScenes)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        yield return null;

        yield return StartCoroutine(OnLoad());
    }

    public Coroutine Unload()
    {
        return StartCoroutine(UnloadTask());
    }

    IEnumerator UnloadTask()
    {
        yield return StartCoroutine(OnUnload());

        // Unload additive scenes
        foreach (string sceneName in additiveScenes)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }
    }

}
