using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableProperties
{
    [CreateAssetMenu(fileName = "StatusEffects", menuName = "ScriptableObjects/StatusEffectScriptableObject", order = 3)]
    public class StatusEffects : ScriptableObject
    {
        [TitleGroup("Ice Tower", alignment: TitleAlignments.Centered)]
        [HorizontalGroup("Ice Tower/Split")]
        [VerticalGroup("Ice Tower/Split/Left")]
        [BoxGroup("Ice Tower/Split/Left/Freeze Time"),HideLabel]
        public float freezeTime;
    
        [VerticalGroup("Ice Tower/Split/Right")] 
        [BoxGroup("Ice Tower/Split/Right/Freeze Ratio"),HideLabel]
        public float freezeRatio;
    
        [TitleGroup("Fire Tower", alignment: TitleAlignments.Centered)]
        [HorizontalGroup("Fire Tower/Split")]
        [VerticalGroup("Fire Tower/Split/Left")]
        [BoxGroup("Fire Tower/Split/Left/Burn Time"),HideLabel]
        public float burnTime;
    
        [VerticalGroup("Fire Tower/Split/Right")] 
        [BoxGroup("Fire Tower/Split/Right/Burn Ratio"),HideLabel]
        public float burnRatio;
    
        [TitleGroup("Poison Tower", alignment: TitleAlignments.Centered)]
        [HorizontalGroup("Poison Tower/Split")]
        [VerticalGroup("Poison Tower/Split/Left")]
        [BoxGroup("Poison Tower/Split/Left/Poison Time"),HideLabel]
        public float poisonTime;
    
        [VerticalGroup("Poison Tower/Split/Center")] 
        [BoxGroup("Poison Tower/Split/Center/Poison SlowRatio"),HideLabel]
        public float poisonSlowRatio;
    
        [VerticalGroup("Poison Tower/Split/Right")] 
        [BoxGroup("Poison Tower/Split/Right/Poison Ratio"),HideLabel]
        public float poisonRatio;
    
        [TitleGroup("Teleport Tower", alignment: TitleAlignments.Centered)]
        [HorizontalGroup("Teleport Tower/Split")]
        [VerticalGroup("Teleport Tower/Split/Left")]
        [BoxGroup("Teleport Tower/Split/Left/Teleport Time"),HideLabel]
        public float teleportTime;
    
        [VerticalGroup("Teleport Tower/Split/Right")] 
        [BoxGroup("Teleport Tower/Split/Right/Teleport Ratio"),HideLabel]
        public float teleportRatio;
    }
}
