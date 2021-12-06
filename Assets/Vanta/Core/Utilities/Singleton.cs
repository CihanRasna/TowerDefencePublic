using UnityEngine;



namespace Vanta.Core
{
    
    public abstract class Singleton<T> : VantaBehaviour where T : Component
    {

        private static T instance;



    #region Life Cycle

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
#if !UNITY_EDITOR
            DontDestroyOnLoad(gameObject);
#endif
            }
            else
            {
                Destroy(gameObject);
            }
        }

    #endregion



    #region Pattern

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

    #endregion

    }

}