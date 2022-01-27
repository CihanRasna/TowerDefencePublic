using _Game.Scripts.Enemy;

namespace _Game.Scripts.Projectiles
{
    public class ArrowProjectile : BaseProjectile
    {
        protected override void DoYourOwnShit(BaseEnemy enemy)
        {
            transform.parent = enemy.transform;
            Destroy(gameObject, 3f);
        }
    }
}