using UnityEngine;

public class SkillShotW : ProjectileBase
{
    private Player player; // プレイヤーの参照
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 敵にダメージを与える
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); // 自分の弾を消す
        }
        else if (other.CompareTag("EnemyProjectile"))
        {
            // 敵の弾を消す
            Destroy(other.gameObject);
            Destroy(gameObject); // 自分の弾も消す
        }
    }
}
