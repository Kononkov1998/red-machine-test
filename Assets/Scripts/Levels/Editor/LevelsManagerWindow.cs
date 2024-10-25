using UnityEditor;
using UnityEngine;

namespace Levels.Editor
{
    public class LevelsManagerWindow : EditorWindow
    {
        private int _levelIndex;

        [MenuItem("Tools/LevelsManager")]
        public static void ShowWindow()
        {
            GetWindow<LevelsManagerWindow>("Levels Manager");
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter Level Index", EditorStyles.boldLabel);
            _levelIndex = EditorGUILayout.IntField(_levelIndex);

            if (GUILayout.Button("Load"))
            {
                if (Application.isPlaying)
                    FindObjectOfType<LevelsManager>().GoToLevel(_levelIndex);
                else
                    Debug.LogError("You must be in playmode");
            }
        }
    }
}