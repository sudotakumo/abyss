using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private Player player; // プレイヤーの参照

    void Start()
    {
        player = FindObjectOfType<Player>(); // プレイヤーを見つけて参照を取得
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 敵を倒したときにプレイヤーのダッシュを回復
        if (player != null)
        {
            player.EnableDash();
        }

        // 敵オブジェクトを削除
        Destroy(gameObject);
    }
}
