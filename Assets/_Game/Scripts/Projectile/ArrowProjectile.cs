using _Game.Scripts.Enemy;
using UnityEngine;

public class ArrowProjectile : Projectile
{
    protected override void DoYourOwnShit(BaseEnemy enemy)
    {
        transform.parent = enemy.transform;
        Destroy(gameObject,3f);
    }
}
