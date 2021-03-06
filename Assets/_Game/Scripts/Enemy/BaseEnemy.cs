using System.Collections;
using System.Collections.Generic;
using _Game.Levels.Base;
using _Game.Scripts.ScriptableProperties;
using _Game.Scripts.Tower;
using Dreamteck;
using Dreamteck.Splines;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vanta.Levels;

namespace _Game.Scripts.Enemy
{
    [SelectionBase]
    public abstract class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private GameObject particle;
        [SerializeField] private List<GameObject> deadParticles;
        
        public int enemyWeight;
        public UnityAction<int> enemyWeightAction;
        [SerializeField] private EnemyProperties enemyProperties;
        private StatusEffects _statusEffects;
        private BaseTower _tower;

        private enum CurrentStatus
        {
            NotEffected,
            OnFire,
            Freezing,
            Poisoning,
            Teleported
        }

        [SerializeField] private CurrentStatus currentStatus;

        [field: SerializeField] public SplineFollower splineFollower { private set; get; }
        [SerializeField] private Outlinable myOutline;

        private float health;
        [ShowInInspector] private float currentHealth => health;
        [ShowInInspector] protected int goldPrize;
        private IEnumerator _currentStatusEffect = null;
        private float _multiplier = 1f;
        private float _maxHealth;
        private Animator _animator;

        public Vector3 SplinePercentPosition { get; private set; }

        protected virtual void Start()
        {
            InitializeEnemyLogic();
            enemyWeightAction.Invoke(enemyWeight);
            _maxHealth = health;
            _animator = GetComponent<Animator>();
        }

        public void SplineEndReached()
        {
            var level = LevelManager.Instance.currentLevel as Level;
            if (level.state == BaseLevel.State.Started)
            {
                level.TakeHit(1);
            }

            var go = Instantiate(particle, transform.position,Quaternion.identity, level.transform);
            go.TryGetComponent(out AudioSource source);
            if (source)
            {
                source.volume = AudioManager.Instance.FXSound;
            }
            go.transform.localScale = Vector3.one * 2f;
            var pos = go.transform.localPosition;
            go.transform.localPosition = new Vector3(pos.x, pos.y + 1, pos.z);
            Destroy(go,2f);
            Destroy(gameObject);
        }

        private void Update()
        {
            var percent = splineFollower.GetPercent();
            SplinePercentPosition = splineFollower.EvaluatePosition(percent);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BaseTower>(out var tower))
            {
                _tower = tower;
                myOutline.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<BaseTower>(out var tower))
            {
                _tower = null;
                myOutline.enabled = false;
            }
        }

        private void InitializeEnemyLogic()
        {
            var level = LevelManager.Instance.currentLevel as Level;
            var currentSpline = level.spline;
            splineFollower.spline = currentSpline;
            splineFollower.followSpeed = enemyProperties.speed;
            health = enemyProperties.health;
            goldPrize = enemyProperties.goldPrize;
            _statusEffects = enemyProperties.statusEffects;
        }

        public void TakeDamage(float dmg)
        {
            _animator.SetTrigger("TakeHit");
            myOutline.enabled = true;
            health -= dmg;
            health = Mathf.Clamp(health, 0, enemyProperties.health);
            if (health == 0)
            {
                GetComponent<Collider>().enabled = false;
                splineFollower.follow = false;
                var level = LevelManager.Instance.currentLevel as Level;
                level.IncomeCurrency(goldPrize);
                var rnd = Random.Range(0, 4);
                var selectedParticle = deadParticles[rnd];
                var go = Instantiate(selectedParticle,transform.position,Quaternion.identity,level.transform);
                go.TryGetComponent(out AudioSource source);
                if (source)
                {
                    source.volume = AudioManager.Instance.FXSound;
                }
                go.transform.localScale = Vector3.one * 4f;
                _animator.SetTrigger("Die");
                Destroy(go, 2f);
                Destroy(gameObject,2f);
                Destroy(this);
            }
            enemyWeightAction.Invoke(-enemyWeight);
            if (!_tower)
            {
                myOutline.enabled = false;
            }
        }

        #region GetStatusEffects

        public void GetStatusEffect(BaseTower.Type towerType)
        {
            var lastEffect = _currentStatusEffect;
            splineFollower.followSpeed = enemyProperties.speed;
            StopAllCoroutines();
            _currentStatusEffect = null;

            _currentStatusEffect = towerType switch
            {
                BaseTower.Type.Ice => GetFreezeEffect(),
                BaseTower.Type.Fire => GetFireEffect(),
                BaseTower.Type.Poison => GetPoisonEffect(),
                BaseTower.Type.Teleport => GetTeleportEffect(),
                _ => null
            };
            if (_currentStatusEffect == null)
            {
                currentStatus = CurrentStatus.NotEffected;
            }
            // if (lastEffect != null && _currentStatusEffect != null && ReferenceEquals(lastEffect.Current, _currentStatusEffect.Current))
            // {
            //     _multiplier += 0.25f;
            // }
            // else // GOOD PRACTICE BUT FIRE TOWER BECAMES GOD AQ
            // {
            //     _multiplier = 1;
            // }

            if (_currentStatusEffect == null) return;
            StartCoroutine(_currentStatusEffect);
        }

        #endregion

        #region StatusEffects

        private IEnumerator GetFreezeEffect()
        {
            currentStatus = CurrentStatus.Freezing;
            var normalSpeed = enemyProperties.speed;
            var ratio = (100 - _statusEffects.freezeRatio) / 100f;
            var newSpeed = normalSpeed * ratio;
            splineFollower.followSpeed = newSpeed;
            yield return new WaitForSeconds(_statusEffects.freezeTime);
            
            var curSpeed = splineFollower.followSpeed;
            var totalDelayTime = 1f;
            var elapsedTime = 0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                var progress = Mathf.Clamp01(elapsedTime / totalDelayTime);
                splineFollower.followSpeed = Mathf.Lerp(curSpeed, normalSpeed, progress);
                if (progress >= 1f)
                {
                    break;
                }

                yield return null;
            }
            
        }

        private IEnumerator GetFireEffect()
        {
            currentStatus = CurrentStatus.OnFire;
            var elapsedTime = 0.0f;
            var duration = _statusEffects.burnTime;
            var burnRatio = _statusEffects.burnRatio;
            var initialHealth = health;
            var healthRatio = initialHealth * 0.01f;

            while (true)
            {
                var progress = Mathf.Clamp01(elapsedTime / duration);
                health = initialHealth - (burnRatio * progress) * healthRatio * _multiplier;
                if (progress >= 1)
                {
                    break;
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }

        private IEnumerator GetPoisonEffect()
        {
            currentStatus = CurrentStatus.Poisoning;

            var normalSpeed = enemyProperties.speed;
            var poisonSlowRatio = (100 - _statusEffects.poisonSlowRatio) / 100f;
            var newSpeed = normalSpeed * poisonSlowRatio;
            splineFollower.followSpeed = newSpeed;

            var poisoningRatio = _statusEffects.poisonRatio;
            var initialHealth = health;
            var healthRatio = initialHealth * 0.01f;

            var elapsedTime = 0.0f;
            var duration = _statusEffects.poisonTime;

            while (true)
            {
                var progress = Mathf.Clamp01(elapsedTime / duration);
                health = initialHealth - (poisoningRatio * progress) * healthRatio * _multiplier;
                if (progress >= 1)
                {
                    splineFollower.followSpeed = normalSpeed;
                    break;
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }

        private IEnumerator GetTeleportEffect()
        {
            var isTeleportTower = _tower as TeleportTower;
            if (isTeleportTower)
            {
                var value = isTeleportTower.towerProperties.damageForUpgrade;
                var tpDistance = (isTeleportTower.teleportLevel * value) + _statusEffects.teleportRatio;
            }
            
            currentStatus = CurrentStatus.Teleported;
            var initialSpeed = splineFollower.followSpeed;
            var currentPercent = splineFollower.GetPercent();
            var currentDistance = splineFollower.CalculateLength(0, currentPercent);
            var targetDistance = currentDistance - _statusEffects.teleportRatio;
            var targetPercent = splineFollower.Travel(0, targetDistance, Spline.Direction.Forward);
            var elapsedTime = 0.0f;
            var duration = _statusEffects.teleportTime;
            splineFollower.followSpeed = 0f;

            while (true)
            {
                var progress = Mathf.Clamp01(elapsedTime / duration);
                var lerpAmount = DMath.Lerp(currentPercent, targetPercent, progress);
                splineFollower.SetPercent(lerpAmount);
                if (progress >= 1)
                {
                    splineFollower.followSpeed = initialSpeed;
                    break;
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }

        #endregion
    }
}