using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class MainMenu : MonoBehaviour, IPointerClickHandler
    {
        public bool byName = true;
        public string sceneName;
        public int sceneID = 0;

        private void Start()
        {
            GetComponent<AudioSource>().DOFade(1f, 10f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            LoadScene();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                LoadScene();
            }
        }
        private void LoadScene()
        {
            if (byName)
            {
                if (string.IsNullOrEmpty(sceneName)) return;
                bl_SceneLoaderManager.LoadScene(sceneName);
            }
            else
            {
                bl_SceneLoaderManager.LoadSceneByID(sceneID);
            }
            gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MainMenu))]
    public class MainMenuSceneLoadEditor : Editor
    {
        public MainMenu script;
        string[] sceneNames;
        private void OnEnable()
        {
            script = (MainMenu)target;
            sceneNames = bl_SceneLoaderManager.Instance.GetSceneNames();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical("box");
            {
                script.byName = EditorGUILayout.ToggleLeft("By Name", script.byName, EditorStyles.toolbarButton);
                GUILayout.Space(2);
                
                if (script.byName)
                {
                    script.sceneName = EditorGUILayout.TextField("Scene Name", script.sceneName);
                }
                else
                    script.sceneID = EditorGUILayout.Popup("Scene", script.sceneID, sceneNames, EditorStyles.toolbarPopup);
            }
            GUILayout.Space(2);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}