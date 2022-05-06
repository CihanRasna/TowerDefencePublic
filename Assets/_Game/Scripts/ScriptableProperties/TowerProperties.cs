using _Game.Scripts.Projectiles;
using _Game.Scripts.Tower;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableProperties
{
    [CreateAssetMenu(fileName = "TowerProperties", menuName = "ScriptableObjects/TowerPropertiesScriptableObject",
         order = 2), InlineEditor]
    public class TowerProperties : ScriptableObject
    {
        [Title("Tower Type", titleAlignment: TitleAlignments.Centered), EnumToggleButtons, HideLabel]
        public BaseTower.Type towerType;

        [Title("Tower Shooting Type", titleAlignment: TitleAlignments.Centered), EnumToggleButtons, HideLabel]
        public BaseTower.ShootingType shootingType;

        [Title("Tower Pricing", titleAlignment: TitleAlignments.Centered, bold: true)]
        [VerticalGroup("Tower Pricing")]
        [BoxGroup("Tower Pricing/Base Price")] public int towerPurchasePrice;
        [BoxGroup("Tower Pricing/Damage")] public int baseDamagePrice;
        [BoxGroup("Tower Pricing/Damage")] public float damageUpgradeMultiplier;
        [BoxGroup("Tower Pricing/FireRate")] public int baseFireRatePrice;
        [BoxGroup("Tower Pricing/FireRate")] public float fireRateUpgradeMultiplier;
        [BoxGroup("Tower Pricing/Radius")] public int baseRadiusPrice;
        [BoxGroup("Tower Pricing/Radius")] public float radiusUpgradeMultiplier;

        [TitleGroup("Projectile Properties", alignment: TitleAlignments.Centered)]
        [HorizontalGroup("Projectile Properties/Split")]
        [VerticalGroup("Projectile Properties/Split/Left")]
        [BoxGroup("Projectile Properties/Split/Left/Projectile"), HideLabel, PreviewField(ObjectFieldAlignment.Center)]
        public BaseProjectile baseProjectile;

        [VerticalGroup("Projectile Properties/Split/Right")]
        [BoxGroup("Projectile Properties/Split/Right/Hit Particle"), HideLabel,
         PreviewField(ObjectFieldAlignment.Center)]
        public GameObject hitParticle;

        [VerticalGroup("Settings")] [BoxGroup("Settings/ProjectileRadius")]
        public float projectileEffectZone = 1f;

        [BoxGroup("Settings/Damage")] public float damage;
        [BoxGroup("Settings/Damage")] public float damageForUpgrade;
        [BoxGroup("Settings/Damage")] public int damageMaxUpgradeLevel;
        [BoxGroup("Settings/FireRate")] public float fireRate;
        [BoxGroup("Settings/FireRate")] public float fireRatePerUpgrade;
        [BoxGroup("Settings/FireRate")] public int fireRateMaxUpgradeLevel;
        [BoxGroup("Settings/Range")] public float shootingRadius;
        [BoxGroup("Settings/Range")] public float radiusPerUpgrade;
        [BoxGroup("Settings/Range")] public int radiusMaxUpgradeLevel;
    }
}