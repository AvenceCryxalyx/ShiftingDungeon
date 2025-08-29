using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileUpdater : IProcessSceneWithReport
{
    public int callbackOrder => 0; // Defines the order of execution for multiple processors

    public void OnProcessScene(Scene scene, BuildReport report)
    {
#if !DEV_BUILD
        // Define the tags you want to exclude
        string[] tagsToExclude = { "EditorOnly", "DEBUG" };

        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            // Recursively check children if needed, or focus only on root objects
            CheckAndDestroyTaggedObjects(obj, tagsToExclude);
        }
#endif
    }

    private void CheckAndDestroyTaggedObjects(GameObject currentObject, string[] tagsToExclude)
    {
#if !DEV_BUILD
        foreach (string tag in tagsToExclude)
        {
            if (currentObject.CompareTag(tag))
            {
                Debug.Log($"Excluding GameObject: {currentObject.name} with tag: {tag}");
                Object.DestroyImmediate(currentObject); // Removes the object immediately
                return; // Object destroyed, no need to check other tags or children
            }
        }

        // Check children recursively
        foreach (Transform child in currentObject.transform)
        {
            CheckAndDestroyTaggedObjects(child.gameObject, tagsToExclude);
        }
#endif
    }
}