using System.Collections.Generic;
using UnityEngine;
using Vanta.Core;



namespace Vanta.Characters
{
    
    public abstract class BaseCharacter : VantaBehaviour
    {
        
        public CharacterType type
        {
            get => _type;
            set
            {
                _type = value;
                ApplyType();
            }
        }
        
        [HideInInspector] [SerializeField] private CharacterType _type;
        

        [HideInInspector] public CharacterPrefab prefab => _prefab;
        private CharacterPrefab _prefab;
        [HideInInspector] [SerializeField] private GameObject prefabContainer;



    #region Life Cycle

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            
        }

    #endregion



    #region Type
        
        virtual protected void ApplyType()
        {
            List<GameObject> leakedPrefabContainers = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name == "PrefabContainer")
                {
                    leakedPrefabContainers.Add(child.gameObject);
                }
            }

            if (leakedPrefabContainers.Count > 1)
            {
                foreach (var leakedPrefabContainer in leakedPrefabContainers)
                {
#if UNITY_EDITOR
                    DestroyImmediate(leakedPrefabContainer);
#else
                    Destroy(leakedPrefabContainer);
#endif
                }
                prefabContainer = null;
            }
            
            if (prefabContainer == null)
            {
                prefabContainer = new GameObject("PrefabContainer");
                prefabContainer.transform.parent = transform;
                prefabContainer.transform.SetAsFirstSibling();
                prefabContainer.transform.localPosition = Vector3.zero;
                prefabContainer.transform.localRotation = Quaternion.identity;
                prefabContainer.transform.localScale = Vector3.one;
            }

            foreach (Transform child in prefabContainer.transform)
            {
#if UNITY_EDITOR
                DestroyImmediate(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }

            var info = CharacterManager.Instance.InfoForCharacter(_type);
            if (info == null)
            {
                return;
            }

            if (info.prefab == null)
            {
                Debug.LogError("Prefab for selected character type is null! Assign a prefab to " + _type + " character from \"Vanta/Character Settings\" first.");
                return;
            }

            var prefabObj = Instantiate(info.prefab, prefabContainer.transform) as GameObject;
            //var prefabObj = PrefabUtility.InstantiatePrefab(info.prefab, prefabContainer.transform) as GameObject;
            //PrefabUtility.UnpackPrefabInstance(prefabObj, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            _prefab = prefabObj.GetComponent<CharacterPrefab>();
        }

    #endregion
        
    }

}