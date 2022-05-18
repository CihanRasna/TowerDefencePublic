using System;
using System.Collections;
using _Game.Levels.Base;
using _Game.Scripts.ScriptableProperties;
using _Game.Scripts.Tower;
using DG.Tweening;
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
        public int enemyWeight;
        public UnityAction<int> enemyWeightAction;
        [SerializeField] private EnemyProperties enemyProperties;
        private StatusEffects statusEffects;
        private BaseTower _tower;
        [SerializeField] private Slider slider;
        

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

        [ShowInInspector] protected float health;
        [ShowInInspector] protected int goldPrize;
        private IEnumerator _currentStatusEffect = null;
        private float _multiplier = 1f;
        private float _maxHealth;

        public Vector3 SplinePercentPosition { get; private set; }

        protected virtual void Start()
        {
            InitializeEnemyLogic();
            enemyWeightAction.Invoke(enemyWeight);
            _maxHealth = health;
            slider.value = 1f;
            //canvas.worldCamera = Camera.main;
            //canvas.transform.DOLookAt(Camera.main.transform.position, Vector3.up);
        }

        public void SplineEndReached()
        {
            var level = LevelManager.Instance.currentLevel as Level;
            if (level.state == BaseLevel.State.Started)
            {
                level.TakeHit(1);
            }
        }

        private void Update()
        {
            var percent = splineFollower.GetPercent();
            SplinePercentPosition = splineFollower.EvaluatePosition(percent);
        }

        private void LateUpdate()
        {
            slider.transform.parent.rotation = Quaternion.identity;
            //canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            //Camera.main.transform.rotation * Vector3.down);

            var distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            slider.transform.localScale = Vector3.one * distance / 4750f;
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
            statusEffects = enemyProperties.statusEffects;
        }

        public void TakeDamage(float dmg)
        {
            myOutline.enabled = true;
            health -= dmg;
            health = Mathf.Clamp(health, 0, enemyProperties.health);
            slider.value = Mathf.Clamp01(health / _maxHealth);
            if (health == 0) Destroy(gameObject);
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
            var ratio = (100 - statusEffects.freezeRatio) / 100f;
            var newSpeed = normalSpeed * ratio;
            splineFollower.followSpeed = newSpeed;
            yield return new WaitForSeconds(statusEffects.freezeTime);
            
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
            var duration = statusEffects.burnTime;
            var burnRatio = statusEffects.burnRatio;
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
            var poisonSlowRatio = (100 - statusEffects.poisonSlowRatio) / 100f;
            var newSpeed = normalSpeed * poisonSlowRatio;
            splineFollower.followSpeed = newSpeed;

            var poisoningRatio = statusEffects.poisonRatio;
            var initialHealth = health;
            var healthRatio = initialHealth * 0.01f;

            var elapsedTime = 0.0f;
            var duration = statusEffects.poisonTime;

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
            currentStatus = CurrentStatus.Teleported;
            var initialSpeed = splineFollower.followSpeed;
            var currentPercent = (float) splineFollower.GetPercent();
            var targetPercent = currentPercent * (100 - statusEffects.teleportRatio) / 100f;
            var elapsedTime = 0.0f;
            var duration = statusEffects.teleportTime;
            splineFollower.followSpeed = 0f;

            while (true)
            {
                var progress = Mathf.Clamp01(elapsedTime / duration);
                var lerpAmount = Mathf.Lerp(currentPercent, targetPercent, progress);
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