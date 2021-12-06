using System.IO;
using UnityEditor;
using UnityEngine;



namespace Vanta.Core
{

    [ExecuteInEditMode]
    public class EditorEventManager : Singleton<EditorEventManager>
    {
        
#if UNITY_EDITOR

        private Texture2D logo;



    #region Life Cycle
        
        private void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowCallback;
        }


        private void OnDisable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowCallback;
        }

    #endregion



    #region Hierarchy Logo

        private void HierarchyWindowCallback(int instanceID, Rect selectionRect)
        {
            GameObject obj = (GameObject) EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null && obj.CompareTag("Vanta"))
            {
                if (logo == null)
                {
                    string[] assets = {Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar};
                    FileInfo[] myFile =
                        new DirectoryInfo("Assets").GetFiles("VantaSmall.png", SearchOption.AllDirectories);
                    string[] temp = myFile[0].ToString().Split(assets, 2, System.StringSplitOptions.None);
                    string pathToFile = "Assets" + Path.DirectorySeparatorChar + temp[1];
                    logo = (Texture2D) AssetDatabase.LoadAssetAtPath(pathToFile, typeof(Texture2D));
                }

                float height = selectionRect.height - 3;
                Graphics.DrawTexture(
                    new Rect(GUILayoutUtility.GetLastRect().width - selectionRect.height - 15,
                        selectionRect.y + ((selectionRect.height - height) / 2),
                        height,
                        height), logo);
            }
        }

    #endregion
        
#endif

    }

}
