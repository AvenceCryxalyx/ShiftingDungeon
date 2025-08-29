using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.Collections;

public class AppInstance : SimpleSingleton<AppInstance>
{
    public static Player LocalPlayer { get { return Player.instance; } }
    public static GameMode ActiveGameMode { get { return instance.activeGameMode; } }

    public string FirstScene;

    private GameMode activeGameMode;

    private void Start()
    {
        StartCoroutine(StartTask());
    }

    IEnumerator StartTask()
    {
        yield return null;
        if (!string.IsNullOrEmpty(FirstScene)) yield return LoadGameMode(FirstScene);
    }

    public static T GetActiveGameMode<T>()
        where T : GameMode
    {
        return instance.activeGameMode as T;
    }

    /// <summary>
    /// Loads the scene for the specified GameMode
    /// This unloads active GameMode
    /// </summary>
    /// <param name="gameMode">Must be included in BuildSettings</param>
    /// <returns></returns>

    /// <summary>
    /// Loads the scene for the specified GameMode
    /// This unloads active GameMode
    /// </summary>
    /// <param name="gameMode">Must be included in BuildSettings</param>
    /// <param name="data">Optional data to be provided to the GameMode</param>
    /// <returns></returns>
    public static Coroutine LoadGameMode(string gameMode)
    {
        return instance.StartCoroutine(LoadGameModeTask(gameMode));
    }

    static IEnumerator LoadGameModeTask(string gameMode)
    {
        Assert.IsFalse(string.IsNullOrEmpty(gameMode), "GameMode id is null");

        // Unload active game mode
        if (instance.activeGameMode)
        {
            yield return instance.activeGameMode.Unload();
            yield return SceneManager.UnloadSceneAsync(instance.activeGameMode.gameObject.scene);
            instance.activeGameMode = null;
        }

        Resources.UnloadUnusedAssets();
        yield return null;
        GC.Collect();
        yield return null;

        // Load next game mode
        yield return SceneManager.LoadSceneAsync(gameMode, LoadSceneMode.Additive);
        // Configure active game mode
        instance.activeGameMode = FindObjectsByType<GameMode>(FindObjectsSortMode.None).Single();
    }

    public static Coroutine LoadFirstMode()
    {
        return LoadGameMode(instance.FirstScene);
    }
}