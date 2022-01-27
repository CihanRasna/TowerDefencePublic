using _Game.Levels.Base;
using _Game.Scripts.Enemy;
using _Game.Scripts.Projectiles;
using Vanta.Levels;

public class FireProjectile : BaseProjectile
{
    protected override void DoYourOwnShit(BaseEnemy baseEnemy)
    {
        var level = LevelManager.Instance.currentLevel as Level;
        Destroy(Instantiate(hitParticle, transform.position, transform.rotation, level.transform), 2f);
        Destroy(gameObject);
    }
}