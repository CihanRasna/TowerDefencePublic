using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enemy;
using _Game.Scripts.ScriptableProperties;
using _Game.Scripts.Projectiles;
using DG.Tweening;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Tower
{
    [SelectionBase]
    public abstract class BaseTower : MonoBehaviour
    {
        public enum Type
        {
            Archer,
            Ice,
            Fire,
            Poison,
            Teleport
        }

        public enum ShootingType
        {
            First,
            Random,
            Last
        }

        [SerializeField] protected List<GameObject> towerUpgradedMeshes;

        [ShowInInspector] protected List<BaseEnemy> potentialNextEnemies = new List<BaseEnemy>();
        [HideInInspector] public Type towerType;
        [HideInInspector] public ShootingType shootingType;
        [SerializeField] protected BaseEnemy currentEnemy;

        [SerializeField] public TowerProperties towerProperties;
        [SerializeField] public Transform shootingPoint;
        [SerializeField] protected new SphereCollider collider;
        [SerializeField] private Canvas myCanvas;
        [SerializeField] private Image radiusIndicatorImage;
        private Tweener _imageTweener;

        protected float projectileArriveTime;
        protected float damage;
        protected float firePerSecond;
        protected BaseProjectile baseProjectile;
        protected float projectileEffectZone;

        [HideInInspector] public int damageCurrentLevel = 1;
        [HideInInspector] public int fireRateCurrentLevel = 1;
        [HideInInspector] public int radiusCurrentLevel = 1;

        [HideInInspector] public int damageUpgradePrice;
        [HideInInspector] public int fireRateUpgradePrice;
        [HideInInspector] public int radiusUpgradePrice;

        public Outlinable myOutline;


        private float _lastFireTime = 99999f;
        private int totalSpentMoney;
        public int TotalSpentMoney => totalSpentMoney;

        private void OnEnable()
        {
            collider.radius = towerProperties.shootingRadius;
        }

        protected virtual void Awake()
        {
            towerProperties = Instantiate(towerProperties);
            transform.DOPunchScale(Vector3.up, 0.5f, 3);
        }

        protected virtual void Start()
        {
            InitializeTowerProperties();
        }

        protected virtual void Update()
        {
            _lastFireTime += Time.deltaTime;

            if (_lastFireTime > 1 / firePerSecond && potentialNextEnemies.Count > 0)
            {
                if (currentEnemy)
                {
                    _lastFireTime = 0f;
                    RepeatFire();
                }
                else if (!currentEnemy)
                {
                    _lastFireTime = 0f;
                    ShootingBehaviour();
                }
            }

            // if (_potentialNextEnemies.Count > 0 && !_potentialNextEnemies[0])
            // {
            //     _potentialNextEnemies.Remove(currentEnemy);
            // }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BaseEnemy>(out var enemy))
            {
                if (!potentialNextEnemies.Contains(enemy))
                {
                    potentialNextEnemies.Add(enemy);
                }
                // if (!currentEnemy)
                // {
                //     currentEnemy = enemy;
                //     _fireRoutine = null;
                //     StartCoroutine(_fireRoutine = RepeatFire());
                //     TowerHasTarget();
                // }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<BaseEnemy>(out var enemy)) return;

            if (potentialNextEnemies.Count > 0)
            {
                potentialNextEnemies.Remove(enemy);
            }

            currentEnemy = null;
        }

        private void InitializeTowerProperties()
        {
            damageCurrentLevel = 1;
            fireRateCurrentLevel = 1;
            radiusCurrentLevel = 1;
            totalSpentMoney = towerProperties.towerPurchasePrice;
            towerType = towerProperties.towerType;
            shootingType = towerProperties.shootingType;
            damage = towerProperties.damage;
            collider.radius = towerProperties.shootingRadius;
            firePerSecond = towerProperties.fireRate;
            projectileArriveTime = towerProperties.projectileArriveTime;
            baseProjectile = towerProperties.baseProjectile;
            projectileEffectZone = towerProperties.projectileEffectZone;
            damageUpgradePrice = towerProperties.baseDamagePrice;
            fireRateUpgradePrice = towerProperties.baseFireRatePrice;
            radiusUpgradePrice = towerProperties.baseRadiusPrice;
        }

        public (float damage, float firePerSecond, float radius, int sellPrice) RefValuesForUI()
        {
            return (damage, firePerSecond, collider.radius, totalSpentMoney);
        }

        private void RepeatFire()
        {
            //var level = LevelManager.Instance.currentLevel as Level;
            var transform1 = shootingPoint.transform;
            var go = Instantiate(baseProjectile, transform1.position, transform1.rotation, currentEnemy.transform);
            go.InitializeBullet(this, damage, projectileEffectZone, projectileArriveTime, currentEnemy,
                towerProperties.hitParticle);
        }

        protected virtual void TowerHasTarget()
        {
        }

        public void TowerHasSelected(bool currentSelected)
        {
            if (currentSelected)
            {
                myCanvas.gameObject.SetActive(true);
                radiusIndicatorImage.fillAmount = 0f;
                _imageTweener = radiusIndicatorImage.DOFillAmount(1, .5f).SetEase(Ease.InOutCirc);
            }
            else
            {
                myCanvas.gameObject.SetActive(false);
                _imageTweener.Kill();
                _imageTweener = null;
            }

            var setNewScale = Vector3.one * collider.radius / 5f;
            myCanvas.transform.localScale = setNewScale;
        }

        protected virtual void TowerUpgraded()
        {
            var maxMeshCount = towerUpgradedMeshes.Count;
            var totalUpgradeCount = (damageCurrentLevel + radiusCurrentLevel + fireRateCurrentLevel) - 3;
            var lastMeshIndex = totalUpgradeCount / 2;
            var desiredMeshIndex = Mathf.Min(maxMeshCount - 1, Mathf.Max(0, (totalUpgradeCount / 2)));

            if (desiredMeshIndex == lastMeshIndex)
                for (var i = 0; i < maxMeshCount; i++)
                    towerUpgradedMeshes[i].SetActive(i == desiredMeshIndex);
        }

        public void UpgradeDamage(float value)
        {
            TowerUpgraded();
            damage = towerProperties.damage += value;
            var newPrice = towerProperties.baseDamagePrice * towerProperties.damageUpgradeMultiplier *
                           damageCurrentLevel;
            totalSpentMoney += damageUpgradePrice;
            damageUpgradePrice = (int) newPrice;
        }

        public void UpgradeFireRate(float value)
        {
            TowerUpgraded();
            firePerSecond = towerProperties.fireRate += value;
            var newPrice = towerProperties.baseFireRatePrice * towerProperties.fireRateUpgradeMultiplier *
                           fireRateCurrentLevel;
            totalSpentMoney += fireRateUpgradePrice;
            fireRateUpgradePrice = (int) newPrice;
        }

        public void UpgradeRadius(float value)
        {
            TowerUpgraded();
            collider.radius = towerProperties.shootingRadius += value;
            var newPrice = towerProperties.baseRadiusPrice * towerProperties.radiusUpgradeMultiplier *
                           radiusCurrentLevel;
            totalSpentMoney += radiusUpgradePrice;
            radiusUpgradePrice = (int) newPrice;
        }

        public void SellTower()
        {
            Destroy(gameObject);
        }


        private void ShootingBehaviour()
        {
            potentialNextEnemies.RemoveAll(e => e == null);
            if (potentialNextEnemies.Count > 0)
            {
                currentEnemy = shootingType switch
                {
                    ShootingType.First => ShootFirstOne(),
                    ShootingType.Random => ShootRandom(),
                    ShootingType.Last => ShootLastOne(),
                    _ => null
                };
                RepeatFire();
            }
        }

        private BaseEnemy ShootFirstOne()
        {
            return potentialNextEnemies[0];
        }

        private BaseEnemy ShootLastOne()
        {
            var currentList = potentialNextEnemies;
            if (currentList.Count > 0)
            {
                return potentialNextEnemies.ToList()[potentialNextEnemies.Count - 1];
            }

            return null;
        }

        private BaseEnemy ShootRandom()
        {
            return potentialNextEnemies.ToList()[Random.Range(0, potentialNextEnemies.Count - 1)];
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, towerProperties.shootingRadius);
        }
    }
}