using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using Vanta.Levels;

public class WaveController : MonoBehaviour
{
    [Serializable]
    public class SerializableEnemyList
    {
        public List<BaseEnemy> enemies;
    }
    
    public List<SerializableEnemyList> allEnemies = new List<SerializableEnemyList>();

    [SerializeField] private int currentWeight;

    private IEnumerator Start()
    {
        var level = LevelManager.Instance.currentLevel;
        yield return new WaitUntil(() =>level.state == BaseLevel.State.Started);
        for (var i = 0; i < 50; i++)
        {
            var enemy = Instantiate(allEnemies[0].enemies[0],level.transform);
            enemy.enemyWeightAction += CurrentWeightCalc;
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    private void CurrentWeightCalc(int a)
    {
        currentWeight += a;
    }
}
