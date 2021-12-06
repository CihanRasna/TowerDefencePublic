using System.Reflection;
using UnityEngine;
using UnityEditor;
using Vanta.Core;
using Vanta.Utilities;



namespace Vanta.Characters
{
    
    [CustomEditor(typeof(BaseCharacter), true)]
    public class CharacterEditor : Editor
    {
        private SerializedProperty characterTypeProp;
        private GameObject weaponToInstantiate;

        private void OnEnable()
        {
            characterTypeProp = serializedObject.FindProperty("_type");
        }

        public override void OnInspectorGUI()
        {
            var character = (BaseCharacter) target;
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character Type:", GUILayout.Width(90));
            EditorGUILayout.PropertyField(characterTypeProp, GUIContent.none);
            GUILayout.EndHorizontal();
            
            /*
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            weaponToInstantiate = EditorGUILayout.ObjectField("Weapon:", weaponToInstantiate, typeof(GameObject), false) as GameObject;
            if (GUILayout.Button("Instantiate"))
            {
                var info = CharacterManager.Instance.InfoForCharacter(character.type);
                if (info.prefab == null)
                {
                    EditorUtility.DisplayDialog("VantaPlay Games", "Prefab property of this character is null. Assign a preset for this character from Vanta\\Character Settings menu.", "OK");
                }

                var prefab = info.prefab.GetComponent<CharacterPrefab>();
                var parent = prefab.leftHand;
                if (parent == null)
                {
                    parent = character.transform;
                }

                var weaponObj = PrefabUtility.InstantiatePrefab(weaponToInstantiate, parent);
            }
            GUILayout.EndHorizontal();
            */

            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                MethodInfo method = character.GetType().GetMethod("ApplyType", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(character, null);
            }
        
            EditorGUILayout.Space();
            EditorUtilities.HorizontalLine(Color.gray);
            EditorGUILayout.Space();
        
            base.OnInspectorGUI();
        }
    
    }

}