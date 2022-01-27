using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using _Game.Scripts.Enemy;
using Vanta.Levels;

public class FireProjectile : Projectile
{
    protected override void DoYourOwnShit(BaseEnemy baseEnemy)
    {
        var level = LevelManager.Instance.currentLevel as Level;
        Destroy(Instantiate(hitParticle, transform.position, transform.rotation, level.transform), 2f);
        Destroy(gameObject);
    }
}