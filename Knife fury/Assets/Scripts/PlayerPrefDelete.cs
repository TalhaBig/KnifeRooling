#if UNITY_EDITOR

using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class PlayerPrefDelete : EditorWindow
{
    [MenuItem("Tools/Player Prefs Remover")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player data removed.");
    }
}
#endif