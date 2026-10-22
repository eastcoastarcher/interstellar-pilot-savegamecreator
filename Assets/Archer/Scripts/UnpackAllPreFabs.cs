using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UnpackAllPrefabs
{
    [MenuItem("Window/Unpack All Prefabs in Scene")]
    static void Execute()
    {
        var scene = SceneManager.GetActiveScene();
        int count = 0;

        foreach (var root in scene.GetRootGameObjects())
        {
            var instances = root.GetComponentsInChildren<Transform>(true);
            foreach (var t in instances)
            {
                var outermost = PrefabUtility.GetOutermostPrefabInstanceRoot(t.gameObject);
                if (outermost != null)
                {
                    PrefabUtility.UnpackPrefabInstance(
                        outermost,
                        PrefabUnpackMode.Completely,
                        InteractionMode.UserAction);
                    count++;
                }
            }
        }

        Debug.Log($"Unpacked {count} prefab(s) in scene '{scene.name}'.");
        EditorSceneManager.MarkSceneDirty(scene);
    }
}   