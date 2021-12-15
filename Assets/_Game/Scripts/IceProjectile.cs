using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanta.Levels;

public class IceProjectile : Projectile
{
    protected override void DoYourOwnShit(BaseEnemy enemy)
    {
        var level = LevelManager.Instance.currentLevel as Level;
        Destroy(Instantiate(hitParticle,transform.position,transform.rotation,level.transform),2f);
        Destroy(gameObject);
    }
}
