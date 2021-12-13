using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffects", menuName = "ScriptableObjects/StatusEffectScriptableObject", order = 3), InlineEditor]
public class StatusEffects : ScriptableObject
{
    [TitleGroup("Ice Tower", alignment: TitleAlignments.Centered)]
    [HorizontalGroup("Ice Tower/Split")]
    [VerticalGroup("Ice Tower/Split/Left")]
    [BoxGroup("Ice Tower/Split/Left/Freeze Time"),HideLabel]
    public int freezeTime;
    
    [VerticalGroup("Ice Tower/Split/Right")] 
    [BoxGroup("Ice Tower/Split/Right/Freeze Ratio"),HideLabel]
    public float freezeRatio;
    
    [TitleGroup("Fire Tower", alignment: TitleAlignments.Centered)]
    [HorizontalGroup("Fire Tower/Split")]
    [VerticalGroup("Fire Tower/Split/Left")]
    [BoxGroup("Fire Tower/Split/Left/Burn Time"),HideLabel]
    public int burnTime;
    
    [VerticalGroup("Fire Tower/Split/Right")] 
    [BoxGroup("Fire Tower/Split/Right/Burn Ratio"),HideLabel]
    public float burnRatio;
    
    [TitleGroup("Magic Tower", alignment: TitleAlignments.Centered)]
    [HorizontalGroup("Magic Tower/Split")]
    [VerticalGroup("Magic Tower/Split/Left")]
    [BoxGroup("Magic Tower/Split/Left/Poison Time"),HideLabel]
    public int poisonTime;
    
    [VerticalGroup("Magic Tower/Split/Right")] 
    [BoxGroup("Magic Tower/Split/Right/Poison Ratio"),HideLabel]
    public float poisonRatio;
    
    [TitleGroup("Teleport Tower", alignment: TitleAlignments.Centered)]
    [BoxGroup("Teleport Tower/Teleport Place", true,true),HideLabel]
    public int teleportPlace;
}