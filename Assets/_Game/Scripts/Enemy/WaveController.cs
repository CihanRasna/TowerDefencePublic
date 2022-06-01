using System.Collections;
using System.Collections.Generic;
using _Game.Levels.Base;
using _Game.Scripts;
using UnityEngine;
using Vanta.Levels;

public class WaveController : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    [SerializeField] private float waitBetweenDifferentWaves = 3f;

    private IEnumerator Start()
    {
        var level = (LevelManager.Instance.currentLevel) as Level;
        yield return new WaitUntil(() => level.state == BaseLevel.State.Started);
        foreach (var w in waves)
        {
            yield return new WaitForSeconds(waitBetweenDifferentWaves);
            for (var j = 0; j < w.waves.Count; j++)
            {
                yield return new WaitForSeconds(w.waves[j].waitTimeAfterAllSpawned);
                for (var k = 0; k < w.waves[j].count; k++)
                {
                    var enemy = Instantiate(w.waves[j].enemy,level.transform);
                    enemy.enemyWeightAction += WeightAction;
                    yield return new WaitForSeconds(w.waves[j].waitTimeBetweenSpawn);
                }
            }
        }
        level.Success();
    }

    private void WeightAction(int w)
    {
        
    }
}
