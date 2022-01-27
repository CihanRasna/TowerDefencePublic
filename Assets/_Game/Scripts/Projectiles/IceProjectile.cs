using _Game.Levels.Base;
using _Game.Scripts.Enemy;
using Vanta.Levels;

namespace _Game.Scripts.Projectiles
{
    public class IceProjectile : BaseProjectile
    {
        protected override void DoYourOwnShit(BaseEnemy enemy)
        {
            var level = LevelManager.Instance.currentLevel as Level;
            Destroy(Instantiate(hitParticle, transform.position, transform.rotation, level.transform), 2f);
            Destroy(gameObject);
        }
    }
}