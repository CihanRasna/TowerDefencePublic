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
        yield return new WaitUntil(() => LevelManager.Instance.currentLevel.state == BaseLevel.State.Started);
        for (var i = 0; i < 10; i++)
        {
            var enemy = Instantiate(allEnemies[0].enemies[0]);
            enemy.enemyWeightAction += CurrentWeightCalc;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    private void CurrentWeightCalc(int a)
    {
        currentWeight += a;
    }
}
