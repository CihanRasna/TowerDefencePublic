using UnityEditor;
using Vanta.Core;
using Vanta.Utilities;


namespace Vanta.Characters
{

    [CustomEditor(typeof(CharacterManager))]
    public class CharacterManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var manager = (CharacterManager)target;
            var characterInfo = EditorUtilities.GetPrivateField<CharacterInfo.Dictionary>(manager, "_characterInfo");
            var characterCount = characterInfo?.Count ?? 0;
            EditorGUILayout.LabelField("Character Count: " + characterCount, EditorStyles.boldLabel);
        }
    }

}
