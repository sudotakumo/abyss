using UnityEngine;

public class SkillShotQ : ProjectileBase
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ダメージを与えるロジック
            other.GetComponent<Enemy>().TakeDamage(damage);
            // Qスキルの弾は当たっても消えない
        }
    }
}
