using _Game.Scripts.Enemy;
using _Game.Scripts.Tower;
using DG.Tweening;
using UnityEngine;
using Vanta.Levels;

namespace _Game.Scripts.Projectiles
{
    [SelectionBase]
    public abstract class BaseProjectile : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected float damage;
        [SerializeField, HideInInspector] protected BaseEnemy target;
        [SerializeField, HideInInspector] protected GameObject hitParticle;
        [SerializeField, HideInInspector] protected float effectZone;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip soundFX;

        [HideInInspector] public BaseTower.Type bulletType;
        private Tweener _tweener;

        private void OnTriggerEnter(Collider other)
        {
            /*if (other.TryGetComponent<BaseEnemy>(out var enemy))
            {
                OnHit(enemy);
                enemy.TakeDamage(damage);
                enemy.GetStatusEffect(bulletType);
            }*/
        }

        private void OnHit(BaseEnemy enemy)
        {
            GetComponent<Collider>().enabled = false;
            DoYourOwnShit(enemy);
            _tweener.Kill();
            enemy.TakeDamage(damage);
            if (hitParticle)
            {
                Destroy(Instantiate(hitParticle, transform.position, Quaternion.identity, LevelManager.Instance.currentLevel.transform),2f);
            }
            EffectEnemiesInRadius();
        }

        private void EffectEnemiesInRadius()
        {
            Collider[] colliders = new Collider[15];
            Physics.OverlapSphereNonAlloc(transform.position, effectZone, colliders);
            //colliders = Physics.OverlapSphere(transform.position, effectZone);
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

        public void InitializeBullet(BaseTower myTower, float myDamage, float myEffectZone,float arriveTime, BaseEnemy myTarget,
            GameObject myParticle)
        {
            var worldScale = transform.lossyScale;
            var localScale = transform.localScale;
            var targetScale = new Vector3(localScale.x / worldScale.x , localScale.y / worldScale.y, localScale.z / worldScale.z);

            transform.localScale = targetScale * 2f;
            audioSource.clip = soundFX;
            audioSource.volume = AudioManager.Instance.FXSound;
            bulletType = myTower.towerType;
            effectZone = myEffectZone;
            damage = myDamage;
            target = myTarget;
            hitParticle = myParticle;
            if (hitParticle)
            {
                hitParticle.TryGetComponent<AudioSource>(out var source);
                if (source)
                {
                    source.volume = AudioManager.Instance.FXSound;
                }
            }
            
            audioSource.Play();
            ProjectileMovementOverrider(arriveTime);
        }

        protected virtual void ProjectileMovementOverrider(float arriveTime)
        {
            _tweener = transform.DOLocalMove(Vector3.zero + Vector3.up, arriveTime).OnUpdate(() =>
            {
                if (target)
                {
                    //_tweener.ChangeEndValue(target.transform.position + Vector3.up, true);
                    transform.LookAt(target.transform);
                }
                else
                {
                    _tweener.Kill();
                    DestroyImmediate(gameObject);
                }
            }).OnComplete(()=> OnHit(target));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, effectZone);
        }
    }
}