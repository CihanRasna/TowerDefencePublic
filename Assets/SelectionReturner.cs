using System;
using UnityEngine;

[Serializable]
public class SelectionReturner : MonoBehaviour
{
    [SerializeField] private Component selectableParent;
    
    //public object myParent;

    public Component ReturnSelectedParent()
    {
        return selectableParent;
    }

    // public T ReturnParent<T>()
    // {
    //     return (T) myParent;
    // }
}
