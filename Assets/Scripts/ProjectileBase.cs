using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 10;
    private Vector3 direction;

    void Start()
    {
        // オブジェクトが生成されてからlifeTime秒後に自動的に削除される
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    void Update()
    {
        // 弾を指定された方向に移動させる
        transform.position += direction * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 敵にダメージを与える
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(100); // ダメージ値は必要に応じて調整
            }

            // 弾を削除する
            Destroy(gameObject);
        }
    }
}
