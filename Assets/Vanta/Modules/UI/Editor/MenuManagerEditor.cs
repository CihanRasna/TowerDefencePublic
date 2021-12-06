using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vanta.Core;
using Vanta.Utilities;


namespace Vanta.UI
{

    [CustomEditor(typeof(MenuManager), true)]
    public class MenuManagerEditor : Editor
    {

    #region Life Cycle

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            var manager = MenuManager.Instance;
            var currentDevice = EditorUtilities.GetPrivateField<SafeArea.SimulationDevice>(manager, "_simulatingDevice");
            var newDevice = (SafeArea.SimulationDevice) EditorGUILayout.EnumPopup("Simulating Device:", currentDevice);
            if (newDevice != currentDevice)
            {
                EditorUtilities.SetPrivateField(manager, "_simulatingDevice", newDevice);
                EditorUtility.SetDirty(manager);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

            GUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            EditorUtilities.HorizontalLine(Color.gray);
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }

    #endregion
        
    }
    
}
