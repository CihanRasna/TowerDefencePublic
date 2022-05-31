using System;
using System.Collections.Generic;
using _Game.Scripts.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts
{
    [Serializable] public struct ThisWave
    {
        public BaseEnemy enemy;
        public int count;
        public float waitTimeBetweenSpawn;
        public float waitTimeAfterAllSpawned;
    }

    [CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 4)]
    public class Wave : ScriptableObject
    {
        public List<ThisWave> waves = new List<ThisWave>();
    }
}