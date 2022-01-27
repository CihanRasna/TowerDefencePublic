using _Game.Scripts.Enemy;
using _Game.Scripts.Tower;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Projectiles
{
    [SelectionBase]
    public abstract class BaseProjectile : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected float damage;
        [SerializeField, HideInInspector] protected Transform target;
        [SerializeField, HideInInspector] protected GameObject hitParticle;
        [SerializeField, HideInInspector] protected float effectZone;

        [HideInInspector] public BaseTower.Type bulletType;
        private Tweener _tweener = null;

        protected virtual void Start()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BaseEnemy>(out var enemy))
            {
                GetComponent<Collider>().enabled = false;
                DoYourOwnShit(enemy);
                _tweener.Kill();
                enemy.TakeDamage(damage);
                EffectEnemiesInRadius();
                // enemy.TakeDamage(damage);
                // enemy.GetStatusEffect(bulletType);
            }
        }

        private void EffectEnemiesInRadius()
        {
            var colliders = Physics.OverlapSphere(transform.position, effectZone);
            foreach (var col in colliders)
            {
                col.TryGetComponent<BaseEnemy>(out var e);
                if (e == null)
                {
                    continue;
                }

                e.GetStatusEffect(bulletType);
            }
        }

        protected abstract void DoYourOwnShit(BaseEnemy baseEnemy);

        public void InitializeBullet(BaseTower myTower, float myDamage, float myEffectZone, Transform myTarget,
            GameObject myParticle)
        {
            bulletType = myTower.towerType;
            effectZone = myEffectZone;
            damage = myDamage;
            target = myTarget;
            hitParticle = myParticle;
            ProjectileMovementOverrider();
        }

        protected virtual void ProjectileMovementOverrider()
        {
            _tweener = transform.DOMove(target.position, .2f).OnUpdate(() =>
            {
                if (target) _tweener.ChangeEndValue(target.position + Vector3.up, true);
                else
                {
                    _tweener.Kill();
                    DestroyImmediate(gameObject);
                }
            });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, effectZone);
        }
    }
}