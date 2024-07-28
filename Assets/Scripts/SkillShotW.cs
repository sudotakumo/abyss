using UnityEngine;

public class SkillShotW : ProjectileBase
{
    private Player player; // ƒvƒŒƒCƒ„[‚ÌQÆ
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // “G‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚é
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); // ©•ª‚Ì’e‚ğÁ‚·
        }
        else if (other.CompareTag("EnemyProjectile"))
        {
            // “G‚Ì’e‚ğÁ‚·
            Destroy(other.gameObject);
            Destroy(gameObject); // ©•ª‚Ì’e‚àÁ‚·
        }
    }
}
