using UnityEngine;

public class ArrowProjectile : Projectile
{
    protected override void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
