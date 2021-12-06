using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vanta.Core;
using Vanta.Utilities;



namespace Vanta.Levels
{
    public class LevelSettingsWindow : EditorWindow
    {
        private List<LevelInfo> editingInfo;

        private int selectedTabIdx = 0;

        // Levels Tab
        private int selectedLevelIdx = 0;
        
        // Ordering Tab
        private ReorderableList reorderableList;
        private Vector2 scrollPosition;
        
        // Config Tab
        private bool forceLevel = false;
        private int forceLevelIdx = -1;
        private bool forceSameLevel = false;



    #region Life Cycle

        [MenuItem("Vanta/Level Settings", false, 2)]
        public static void ShowWindow()
        {
            var instance = GetWindow<LevelSettingsWindow>(false, "Level Settings", true);
            instance.maxSize = new Vector2(330f, 500f);
            instance.minSize = instance.maxSize;
        }

        private void OnEnable()
        {
            ImportLevelInfo();
            
            reorderableList = new ReorderableList(editingInfo, typeof(LevelInfo), true, false, false, false);
            reorderableList.drawElementCallback += DrawOrderingElement;
            reorderableList.onReorderCallbackWithDetails += OnReorderCallback;
        }

        private void OnDisable()
        {
            reorderableList.drawElementCallback -= DrawOrderingElement;
            reorderableList.onReorderCallbackWithDetails -= OnReorderCallback;
        }

        private void OnGUI()
        {
            // Import actual level data
            if (editingInfo == null)
            {
                ImportLevelInfo();
            }
            
            
            // General UI Style
            var popupMargin = EditorStyles.popup.margin;
            popupMargin.top = 5;
            EditorStyles.popup.margin = popupMargin;

            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = true;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
            }
            
            var warningButtonStyle = new GUIStyle(GUI.skin.button);
            warningButtonStyle.normal.textColor = Color.red;


            // Tab
            selectedTabIdx = GUILayout.Toolbar (selectedTabIdx, new string[] {"Levels", "Ordering", "Config"});
            if (selectedTabIdx == 0)
            {
                DisplayLevelsTab();
            }
            else if (selectedTabIdx == 1)
            {
                DisplayOrderingTab();
            }
            else if (selectedTabIdx == 2)
            {
                DisplayConfigTab();
            }
            
            
            // Save
            EditorGUILayout.Space();
            EditorUtilities.HorizontalLine(Color.gray);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Do not forget to save changes before closing this window!");
            if (GUILayout.Button("Save Changes"))
            {
                SaveEditorData();
                ImportLevelInfo();
            }
            
            EditorGUILayout.Space();
            if (GUILayout.Button("Cancel", warningButtonStyle))
            {
                Close();
            }
            
            EditorGUILayout.Space();
            if (GUILayout.Button("Copy To Clipboard"))
            {
                ExportToJSON();
            }
        }

    #endregion



    #region Levels Tab

        private void DisplayLevelsTab()
        {
            // Style
            var warningButtonStyle = new GUIStyle(GUI.skin.button);
            warningButtonStyle.normal.textColor = Color.red;
            
            
            // Interaction Values
            if (editingInfo.Count > 0 && selectedLevelIdx == -1)
            {
                selectedLevelIdx = 0;
            }
            else if (selectedLevelIdx > editingInfo.Count - 1)
            {
                selectedLevelIdx = editingInfo.Count - 1;
            }
            
            LevelInfo selectedLevelInfo = null;
            
            
            // Level Selection
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Level", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            
            List<string> levelDisplayNames = new List<string>();
            for (int i = 0; i < editingInfo.Count; i++)
            {
                levelDisplayNames.Add((i + 1) + ". " + editingInfo[i].name);
            }
            
            selectedLevelIdx = EditorGUILayout.Popup("Select a level:", selectedLevelIdx, levelDisplayNames.ToArray());
            if (selectedLevelIdx >= 0)
            {
                selectedLevelInfo = editingInfo[selectedLevelIdx];
            }
            
            
            // New Level
            if (GUILayout.Button("New"))
            {
                var newLevelInfo = new LevelInfo();

                editingInfo.Add(newLevelInfo);
                selectedLevelIdx = editingInfo.Count - 1;
                selectedLevelInfo = newLevelInfo;
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            // Level Details
            if (selectedLevelInfo != null)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
                
                EditorGUILayout.LabelField("Id: " + selectedLevelInfo.id);
                
                selectedLevelInfo.name = EditorGUILayout.TextField("Level Name:", selectedLevelInfo.name);
                selectedLevelInfo.title = EditorGUILayout.TextField("Title:", selectedLevelInfo.title);
                selectedLevelInfo.description = EditorGUILayout.TextField("Description:", selectedLevelInfo.description);
                selectedLevelInfo.viewCount = EditorGUILayout.IntField("View Count:", selectedLevelInfo.viewCount);
                
                EditorGUILayout.LabelField("Thumbnail:");
                Rect thumbnailRect = new Rect(123, 169+41, 204, 80);
                selectedLevelInfo.thumbnail = EditorGUI.ObjectField(thumbnailRect, selectedLevelInfo.thumbnail, typeof(Sprite), false) as Sprite;
                GUILayout.Space(60);

                var prefabObj = EditorGUILayout.ObjectField("Prefab:", selectedLevelInfo.prefab, typeof(GameObject), false) as GameObject;
                if (prefabObj != null)
                {
                    if (prefabObj.GetComponent<BaseLevel>() != null)
                    {
                        selectedLevelInfo.prefab = prefabObj;
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("VantaPlay Games", "Selected prefab must contain a Level component!", "OK");
                    }   
                }
                
                EditorGUILayout.Space();
                if (GUILayout.Button("Delete Level", warningButtonStyle))
                {
                    editingInfo.RemoveAt(selectedLevelIdx);

                    if (editingInfo.Count == 0)
                    {
                        selectedLevelIdx = -1;
                        selectedLevelInfo = null;
                    }
                    else
                    {
                        if (selectedLevelIdx >= editingInfo.Count)
                        {
                            selectedLevelIdx = editingInfo.Count - 1;
                            selectedLevelInfo = editingInfo[selectedLevelIdx];
                        }    
                    }
                }
                
                EditorGUILayout.EndVertical();
            }
        }
        
    #endregion

            
            
    #region Ordering Tab

        private void DisplayOrderingTab()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Level Ordering", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            reorderableList.DoLayoutList();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private void OnReorderCallback(ReorderableList list, int index, int newIndex)
        {
            if (index == forceLevelIdx)
            {
                forceLevelIdx = newIndex;
            }
        }
        
        private void DrawOrderingElement(Rect rect, int index, bool active, bool focused)
        {
            var info = editingInfo[index];
            EditorGUI.LabelField(rect, info.name);
        }
            
    #endregion



    #region Config Tab

        private void DisplayConfigTab()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Force Level", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
                
            List<string> levelDisplayNames = new List<string>();
            for (int i = 0; i < editingInfo.Count; i++)
            {
                levelDisplayNames.Add((i + 1) + ". " + editingInfo[i].name);
            }

            forceLevel = EditorGUILayout.ToggleLeft("Enable", forceLevel);
            forceSameLevel = EditorGUILayout.ToggleLeft("Repeat same level", forceSameLevel);
            forceLevelIdx = EditorGUILayout.Popup("Select a level:", forceLevelIdx, levelDisplayNames.ToArray());
            if (forceLevelIdx < 0)
            {
                forceLevel = false;
                forceSameLevel = false;
            }
            else if (forceLevelIdx >= editingInfo.Count)
            {
                forceLevelIdx = editingInfo.Count - 1;
            }
            
            EditorGUILayout.EndVertical();
        }

    #endregion
        
        
        
    #region Import

        private void ImportLevelInfo()
        {
            editingInfo = new List<LevelInfo>();

            var manager = LevelManager.Instance;

            if (manager.levels != null)
            {
                foreach (var levelInfo in manager.levels)
                {
                    var info = levelInfo.Clone();
                    if (info.id.Length < 1)
                    {
                        info.id = Guid.NewGuid().ToString();
                    }
                    
                    editingInfo.Add(info);
                }
            }

            forceLevel = EditorUtilities.GetPrivateField<bool>(manager, "forceLevel");
            forceLevelIdx = EditorUtilities.GetPrivateField<int>(manager, "forceLevelIdx");
            forceSameLevel = EditorUtilities.GetPrivateField<bool>(manager, "forceSameLevel");
            
        }

    #endregion



    #region Save

        private void SaveEditorData()
        {
            var manager = LevelManager.Instance;
            EditorUtilities.SetPrivateField(manager, "_levels", editingInfo);
            EditorUtilities.SetPrivateField(manager, "forceLevel", forceLevel);
            EditorUtilities.SetPrivateField(manager, "forceLevelIdx", forceLevelIdx);
            EditorUtilities.SetPrivateField(manager, "forceSameLevel", forceSameLevel);
            EditorUtility.SetDirty(manager);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

    #endregion
        
        
        
    #region Export

        private void ExportToJSON()
        {
            var json = JsonHelper.ToJson(editingInfo.ToArray());
            EditorGUIUtility.systemCopyBuffer = json;
            EditorUtility.DisplayDialog("VantaPlay Games", "Level data has been copied to clipboard.", "OK");
        }

    #endregion
        
    }   
}