using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Vanta.Core;
using Vanta.Utilities;



namespace Vanta.Characters
{
    public class CharacterSettingsWindow : EditorWindow
    {

        private int selectedCharacterIdx = -1;
        private OrderedDictionary<string, CharacterInfo> editingInfo;
        private bool isBusy = false;



    #region Life Cycle

        [MenuItem("Vanta/Character Settings", false, 1)]
        public static void ShowWindow()
        {
            var instance = GetWindow<CharacterSettingsWindow>(false, "Character Settings", true);
            instance.maxSize = new Vector2(330f, 600f);
            instance.minSize = instance.maxSize;
            instance.ImportCharacterInfo();
        }

        public void OnGUI()
        {
            if (isBusy)
            {
                GUILayout.Label("Processing...");
                return;
            }


            // Import actual character data
            if (editingInfo == null)
            {
                ImportCharacterInfo();
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
            
            
            // Interaction Values
            var characterNames = editingInfo.Keys.ToList();
            if (characterNames.Count > 0 && selectedCharacterIdx == -1)
            {
                selectedCharacterIdx = 0;
            }
            else if (selectedCharacterIdx > characterNames.Count - 1)
            {
                selectedCharacterIdx = characterNames.Count - 1;
            }
            
            string selectedCharacterName = null;
            CharacterInfo selectedCharacterInfo = null;


            // Character Selection
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Character", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            
            List<string> characterDisplayNames = new List<string>();
            for (int i = 0; i < characterNames.Count; i++)
            {
                characterDisplayNames.Add((i + 1) + ". " + characterNames[i]);
            }
            
            selectedCharacterIdx = EditorGUILayout.Popup("Select a character:", selectedCharacterIdx, characterDisplayNames.ToArray());
            if (selectedCharacterIdx >= 0)
            {
                selectedCharacterName = characterNames[selectedCharacterIdx];
                selectedCharacterInfo = editingInfo[selectedCharacterName];
            }
            
            
            // New Character
            if (GUILayout.Button("New"))
            {
                var newCharacterInfo = new CharacterInfo();

                var dummyCharacterIdx = 1;
                while (true)
                {
                    var dummyCharacterName = "NewCharacter" + dummyCharacterIdx++;
                    if (!characterNames.Contains(dummyCharacterName))
                    {
                        newCharacterInfo.name = dummyCharacterName;
                        break;
                    }
                }
                
                editingInfo.Add(newCharacterInfo.name, newCharacterInfo);
                characterNames.Add(newCharacterInfo.name);
                selectedCharacterIdx = characterNames.Count - 1;
                selectedCharacterInfo = newCharacterInfo;
            }
            
            EditorGUILayout.EndHorizontal();


            // Character Details
            if (selectedCharacterInfo != null)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
                
                EditorGUILayout.LabelField("Id: " + selectedCharacterInfo.id);
                
                selectedCharacterInfo.name = EditorGUILayout.TextField("Character Name:", selectedCharacterInfo.name);
                selectedCharacterInfo.title = EditorGUILayout.TextField("Title:", selectedCharacterInfo.title);
                selectedCharacterInfo.subscriberCount = EditorGUILayout.IntField("Subscriber Count:", selectedCharacterInfo.subscriberCount);
                
                EditorGUILayout.LabelField("Store Thumbnail:");
                Rect thumbnailRect = new Rect(123, 105 + 42 + 21, 204, 80);
                selectedCharacterInfo.storeThumbnail = EditorGUI.ObjectField(thumbnailRect, selectedCharacterInfo.storeThumbnail, typeof(Sprite), false) as Sprite;
                GUILayout.Space(60);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Store Category:");
                selectedCharacterInfo.storeCategory = (CharacterInfo.StoreCategory) EditorGUILayout.EnumPopup(selectedCharacterInfo.storeCategory);
                EditorGUILayout.EndHorizontal();
                
                var prefabObj = EditorGUILayout.ObjectField("Prefab:", selectedCharacterInfo.prefab, typeof(GameObject), false) as GameObject;
                if (prefabObj != null)
                {
                    if (prefabObj.GetComponent<CharacterPrefab>() != null)
                    {
                        selectedCharacterInfo.prefab = prefabObj;
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("VantaPlay Games", "Selected prefab must contain a Character.Prefab component!", "OK");
                    }   
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Delete Character", warningButtonStyle))
                {
                    editingInfo.Remove(selectedCharacterName);
                    characterNames = editingInfo.Keys.ToList();

                    if (characterNames.Count == 0)
                    {
                        selectedCharacterIdx = -1;
                        selectedCharacterName = null;
                        selectedCharacterInfo = null;
                    }
                    else
                    {
                        if (selectedCharacterIdx >= characterNames.Count)
                        {
                            selectedCharacterIdx = characterNames.Count - 1;
                            selectedCharacterName = characterNames[selectedCharacterIdx];
                            selectedCharacterInfo = editingInfo[selectedCharacterName];
                        }    
                    }
                }

                EditorGUILayout.EndVertical();
            }


            // Save
            EditorGUILayout.Space();
            EditorUtilities.HorizontalLine(Color.gray);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Do not forget to save changes before closing this window!");
            if (GUILayout.Button("Save Changes"))
            {
                isBusy = true;
                GenerateCharacterTypes();
                SaveEditorData();
                ImportCharacterInfo();
            }
            
            EditorGUILayout.Space();
            if (GUILayout.Button("Cancel", warningButtonStyle))
            {
                Close();
            }

        }
        
        void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }

        public void OnAfterAssemblyReload()
        {
            if (isBusy)
            {
                isBusy = false;
            }
        }
        
    #endregion

        

    #region Import

        private void ImportCharacterInfo()
        {
            editingInfo = new OrderedDictionary<string, CharacterInfo>();
            foreach (var characterType in EditorUtilities.GetEnumList<CharacterType>())
            {
                var characterInfo = CharacterManager.Instance.InfoForCharacter(characterType);
                if (characterInfo != null)
                {
                    editingInfo.Add(characterType.ToString(), characterInfo.Clone());                    
                }
            }
        }

    #endregion



    #region Save

        private void GenerateCharacterTypes()
        {
            // Compare character names
            var previousCharacterNames = Enum.GetNames(typeof(CharacterType)).ToList();
            var characterNames = editingInfo.Values.Select(c => c.name).ToList();
            if (characterNames.OrderBy(c => c).SequenceEqual(previousCharacterNames.OrderBy(c => c)))
            {
                if (isBusy)
                {
                    isBusy = false;
                }
                
                return;
            }
            
            
            // Generate Enum File
            string enumName = "CharacterType";
            string enumCode = EditorUtilities.CreateEnumString(enumName, characterNames, 1);

            var directoryPath =
                Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
            var templatePath = directoryPath + Path.DirectorySeparatorChar + "CharacterTypeTemplate.txt";
            if (!File.Exists(templatePath))
            {
                EditorUtility.DisplayDialog("Error", "Template file at " + templatePath + " is missing!", "OK");
                return;
            }

            var templateCode = File.ReadAllText(templatePath);
            templateCode = templateCode.Replace("#ENUM#", enumCode);

            var exportPath = Directory.GetParent(directoryPath).ToString() + Path.DirectorySeparatorChar + "CharacterType.auto.cs";
            using (StreamWriter writer = new StreamWriter(exportPath))
            {
                writer.Write(templateCode);
            }
            
            
            // Save Data
            AssetDatabase.ImportAsset(exportPath);
        }

        private void SaveEditorData()
        {
            var info = new CharacterInfo.Dictionary();

            var characterNames = editingInfo.Keys.ToList();
            for (int i=0; i<characterNames.Count; i++)
            {
                var characterName = characterNames[i];
                var characterType = (CharacterType)i;

                var clone = editingInfo[characterName].Clone();
                if (clone.id.Length < 1)
                {
                    clone.id = Guid.NewGuid().ToString();
                } 
                info.Add(characterType, clone);
            }

            var manager = CharacterManager.Instance;
            EditorUtilities.SetPrivateField(manager, "_characterInfo", info);
            EditorUtility.SetDirty(manager);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

    #endregion

    }

}