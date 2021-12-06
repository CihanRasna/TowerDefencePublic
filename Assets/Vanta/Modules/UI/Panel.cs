using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vanta.Core;



namespace Vanta.UI
{

    public class Panel : VantaBehaviour
    {

        [HideInInspector] public Panel parent;
        [HideInInspector] public Panel[] children;
        
        [SerializeField] protected SafeArea _safeArea;
        private bool _safeAreaDidApply = false;
        private bool _waitingForSafeAreaToDisplay = false;



    #region Life Cycle

        protected virtual void Awake()
        {
            _safeArea = GetComponentInParent<SafeArea>();
            if (_safeArea != null)
            {
                _safeArea.didApplySafeArea += OnSafeAreaApply;
                _safeArea.didApplySafeArea += InvokeOnDisplayIfNecessary;
            }
        }

        protected virtual void Start()
        {
            var tempChildren = new List<Panel>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childPanel = transform.GetChild(i).GetComponent<Panel>();
                if (childPanel != null)
                {
                    tempChildren.Append(childPanel);
                }
            }

            children = tempChildren.ToArray();
            parent = transform.parent.GetComponent<Panel>();
        }

    #endregion



    #region Appearance

        public void Display()
        {
            gameObject.SetActive(true);

            if (_safeArea == null || _safeAreaDidApply)
            {
                OnDisplay();
            }
            else
            {
                _waitingForSafeAreaToDisplay = true;
            }
        }

        private void InvokeOnDisplayIfNecessary(SafeArea safeArea)
        {
            if (_waitingForSafeAreaToDisplay)
            {
                _waitingForSafeAreaToDisplay = false;
                OnDisplay();
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

    #endregion



    #region Events

        protected virtual void OnDisplay()
        {
            
        }

        protected virtual void OnSafeAreaApply(SafeArea safeArea)
        {
            _safeAreaDidApply = true;
        }

    #endregion

    }

}